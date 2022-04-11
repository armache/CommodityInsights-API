using AngloAmerican.Api.DbContexts;
using AngloAmerican.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AngloAmerican.Api.Services
{
    public class TradeRepository : ITradeRepository
    {
        private CommodityDbContext _dbContext;
        private readonly IPnlService _pnlService;
        private IEnumerable<Product>? _products = null;

        public TradeRepository(CommodityDbContext dbContext, IPnlService pnlService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _pnlService = pnlService ?? throw new ArgumentNullException(nameof(pnlService));
        }

        public async Task<IEnumerable<TradeLogDto>> GetTradeLogsAsync(int days)
        {
            var mostRecentLogDate = _dbContext.TradeLogs.Max(a => a.Date);
            var _tradeLogs = await (from tl in _dbContext.TradeLogs
                                join m in _dbContext.Models on tl.ModelId equals m.Id
                                join c in _dbContext.Commodities on tl.CommodityId equals c.Id
                                where tl.Date >= mostRecentLogDate.AddDays(-1 * days)
                                select new TradeLogDto(
                                    tl.Date,
                                    tl.Contract,
                                    tl.Price,
                                    tl.Position,
                                    tl.NewTradeAction,
                                    tl.PnlDaily,
                                    m.Name,
                                    c.Name
                                ))
                        .ToListAsync();

            return _tradeLogs.OrderByDescending(a => a.Date);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            if (_products == null)
            {
                var query = await (from tl in _dbContext.TradeLogs
                                   join m in _dbContext.Models on tl.ModelId equals m.Id
                                   join c in _dbContext.Commodities on tl.CommodityId equals c.Id
                                   select new { tl.ModelId, tl.CommodityId, tl.Date, tl.PnlDaily, ModelName = m.Name, CommodityName = c.Name })
                            .ToListAsync();

                var products = query.Select(p => new Product(p.ModelId, p.CommodityId, p.ModelName, p.CommodityName))
                                    .DistinctBy(a => new { a.ModelId, a.CommodityId }).ToList();

                products.ForEach(product =>
                {
                    var pnlDictionary = query.Where(p => p.ModelId == product.ModelId && p.CommodityId == product.CommodityId)
                        .Select(pnl => new { pnl.Date, pnl.PnlDaily })
                        .ToDictionary(a => a.Date, a => a.PnlDaily);

                    var pnlYtdList = _pnlService.ExtractHistoricalPnl(pnlDictionary);
                    product.HistoricalPnl.AddRange(pnlYtdList);
                });

                _products = products.OrderBy(a => a.ModelName).ThenBy(a => a.CommodityName);
            }

            return _products;
        }

        //private IEnumerable<PnlYtd> ExtractHistoricalPnl(Dictionary<DateTime, decimal> pnlDictionary)
        //{
        //    var historicalPnls = new List<PnlYtd>();

        //    var annualPnls = pnlDictionary.GroupBy(a => a.Key.Year);

        //    foreach (var pnl in annualPnls)
        //    {
        //        var ytd = new PnlYtd(pnl.Key);

        //        for (var i = 1; i <= 12; i++) //iterating months
        //        {
        //            var startingMonth = pnl.OrderBy(a => a.Key).First().Key.Month;
        //            if (i < startingMonth) continue;

        //            var dailyPnls = pnl.Where(a => a.Key.Month == i)
        //                               .Select(a => new PnlDaily { Day = a.Key.Day, Pnl = a.Value })
        //                               .OrderBy(a => a.Day)
        //                               .ToList();

        //            if (!dailyPnls.Any()) break;

        //            var pnlMonthly = new PnlMonthly(i);
        //            pnlMonthly.DailyPnls.AddRange(dailyPnls);
        //            ytd.MonthlyPnls.Add(pnlMonthly);
        //        }

        //        historicalPnls.Add(ytd);
        //    }

        //    return historicalPnls.OrderBy(a => a.Year);
        //}
    }
}
