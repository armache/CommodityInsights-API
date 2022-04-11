using AngloAmerican.Api.Models;

namespace AngloAmerican.Api.Services
{
    public class PnlService : IPnlService
    {
        public IEnumerable<PnlYtd> ExtractHistoricalPnl(Dictionary<DateTime, decimal> pnlDictionary)
        {
            var historicalPnls = new List<PnlYtd>();

            var annualPnls = pnlDictionary.GroupBy(a => a.Key.Year);

            foreach (var pnl in annualPnls)
            {
                var ytd = new PnlYtd(pnl.Key);

                for (var i = 1; i <= 12; i++) //iterating months
                {
                    var startingMonth = pnl.OrderBy(a => a.Key).First().Key.Month;
                    if (i < startingMonth) continue;

                    var dailyPnls = pnl.Where(a => a.Key.Month == i)
                                       .Select(a => new PnlDaily { Day = a.Key.Day, Pnl = a.Value })
                                       .OrderBy(a => a.Day)
                                       .ToList();

                    if (!dailyPnls.Any()) break;

                    var pnlMonthly = new PnlMonthly(i);
                    pnlMonthly.DailyPnls.AddRange(dailyPnls);
                    ytd.MonthlyPnls.Add(pnlMonthly);
                }

                historicalPnls.Add(ytd);
            }

            return historicalPnls.OrderBy(a => a.Year);
        }
    }
}
