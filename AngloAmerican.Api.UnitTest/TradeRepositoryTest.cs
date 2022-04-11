using AngloAmerican.Api.DbContexts;
using AngloAmerican.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AngloAmerican.Api.UnitTest
{
    public class TradeRepositoryTest: IClassFixture<BaseTradeRepositoryTest>
    {
        private Mock<ITradeDataService> _tradeDataServiceMock;
        private readonly IPnlService _pnlService;
        private readonly BaseTradeRepositoryTest _fixture;

        public TradeRepositoryTest(BaseTradeRepositoryTest fixture)
        {
            _fixture = fixture;
            _pnlService = new PnlService();

            _tradeDataServiceMock = new Mock<ITradeDataService>();
            _tradeDataServiceMock.Setup(a => a.ExtractDataFromExcel()).Returns(_fixture.MockedTradeData);
        }

        [Fact]
        public async Task GetTradeLogsAsync_ShouldReturn_Data_NoOlderThan_5_Days_FromLastTradeDate()
        {
            var options = _fixture.GetDbContextOptions();

            var lastTradeDate = _fixture.MockedTradeData.TradeLogs.Max(a => a.Date);
            var expected = _fixture.MockedTradeData.TradeLogs.Where(a => a.Date >= lastTradeDate.AddDays(-5)).Count();

            using (var context = new CommodityDbContext(options, _tradeDataServiceMock.Object))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new TradeRepository(context, _pnlService);

                var result = await sut.GetTradeLogsAsync(5);

                Assert.Equal(expected, result.Count());
            }
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturn_CorrectNumberOfProducts()
        {
            var options = _fixture.GetDbContextOptions();

            using (var context = new CommodityDbContext(options, _tradeDataServiceMock.Object))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new TradeRepository(context, _pnlService);
                var products = await sut.GetProductsAsync();

                var expectedNumOfProducts = _fixture.MockedTradeData.TradeLogs.DistinctBy(a => new { a.ModelId, a.CommodityId }).Count();
                Assert.Equal(expectedNumOfProducts, products.Count());
            }
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturn_Products_ThatContainCorrectYtds()
        {
            var options = _fixture.GetDbContextOptions();

            _tradeDataServiceMock.Setup(a => a.ExtractDataFromExcel()).Returns(_fixture.MockedTradeData);

            using (var context = new CommodityDbContext(options, _tradeDataServiceMock.Object))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new TradeRepository(context, _pnlService);

                var products = await sut.GetProductsAsync();

                products.ToList().ForEach(p =>
                {
                    var expectedProductYTD = _fixture.MockedTradeData.TradeLogs
                        .Where(a => a.ModelId == p.ModelId && a.CommodityId == p.CommodityId)
                        .Sum(a => a.PnlDaily);

                    var actual = p.HistoricalPnl.Sum(a => a.ClosingPnl);

                    Assert.Equal(expectedProductYTD, actual);
                });
            }
        }

        
    }
}
