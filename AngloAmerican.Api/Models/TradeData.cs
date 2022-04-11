using AngloAmerican.Api.Entities;

namespace AngloAmerican.Api.Models
{
    public class TradeData
    {
        public IEnumerable<Model> Models { get; }
        public IEnumerable<Commodity> Commodities { get; }
        public IEnumerable<TradeLog> TradeLogs { get; }

        public TradeData(IEnumerable<Model> models, IEnumerable<Commodity> commodities, IEnumerable<TradeLog> tradeLogs)
        {
            Models = models;
            Commodities = commodities;
            TradeLogs = tradeLogs;
        }
    }
}
