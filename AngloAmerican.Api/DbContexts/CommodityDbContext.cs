using AngloAmerican.Api.Entities;
using AngloAmerican.Api.Models;
using AngloAmerican.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace AngloAmerican.Api.DbContexts
{
    public class CommodityDbContext : DbContext
    {
        private readonly ITradeDataService _tradeDataService;
        private TradeData? _tradeData = null;

        public DbSet<Model> Models { get; set; } = null!;
        public DbSet<Commodity> Commodities { get; set; } = null!;
        public DbSet<TradeLog> TradeLogs { get; set; } = null!;

        public CommodityDbContext(
            DbContextOptions<CommodityDbContext> options, ITradeDataService tradeDataService) : base(options)
        {
            _tradeDataService = tradeDataService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_tradeData == null)
            {
                _tradeData = _tradeDataService.ExtractDataFromExcel();
            }

            modelBuilder.Entity<Model>().HasData(_tradeData.Models);
            modelBuilder.Entity<Commodity>().HasData(_tradeData.Commodities);
            modelBuilder.Entity<TradeLog>().HasData(_tradeData.TradeLogs);

            base.OnModelCreating(modelBuilder);
        }
    }
}
