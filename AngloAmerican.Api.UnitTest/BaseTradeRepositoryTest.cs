using AngloAmerican.Api.DbContexts;
using AngloAmerican.Api.Entities;
using AngloAmerican.Api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AngloAmerican.Api.UnitTest
{
    public class BaseTradeRepositoryTest
    {
        public TradeData MockedTradeData { get; private set; }

        public BaseTradeRepositoryTest()
        {
            MockedTradeData = GetMockedTradeData();
        }

        public DbContextOptions<CommodityDbContext> GetDbContextOptions()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var options = new DbContextOptionsBuilder<CommodityDbContext>()
                .UseSqlite(conn).Options;

            return options;
        }

        private TradeData GetMockedTradeData()
        {
            var models = new Collection<Model> { new Model("Model1"), new Model("Model2") };
            var commodities = new Collection<Commodity> { new Commodity("Commodity1"), new Commodity("Commodity2") };
            var tradeLogs = new Collection<TradeLog>();
            var offestDate = new DateTime(2022, 4, 10);

            tradeLogs.Add(new TradeLog(offestDate.AddDays(-1), "Aug20", 1000, 0, 0, 120)
            {
                ModelId = models.First().Id,
                CommodityId = commodities.First().Id
            });
            tradeLogs.Add(new TradeLog(offestDate.AddDays(-5), "Aug20", 999, 0, 0, 144)
            {
                ModelId = models.Last().Id,
                CommodityId = commodities.Last().Id
            });
            tradeLogs.Add(new TradeLog(offestDate.AddDays(-15), "Aug20", 1001, 0, 0, 130)
            {
                ModelId = models.Last().Id,
                CommodityId = commodities.Last().Id
            });

            return new TradeData(models, commodities, tradeLogs);
        }
    }
}
