using System.Text.Json.Serialization;

namespace AngloAmerican.Api.Models
{
    public class Product
    {
        [JsonIgnore]
        public Guid ModelId { get; }

        [JsonIgnore]
        public Guid CommodityId { get; }


        public string ModelName { get; }
        public string CommodityName { get; }
        public List<PnlYtd> HistoricalPnl { get; }

        public Product(Guid modelId, Guid commodityId, string modelName, string commodityName)
        {
            ModelId = modelId;
            CommodityId = commodityId;
            ModelName = modelName;
            CommodityName = commodityName;

            HistoricalPnl = new List<PnlYtd>();
        }
    }
}
