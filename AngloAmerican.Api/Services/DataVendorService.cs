using AngloAmerican.Api.Entities;
using AngloAmerican.Api.Models;
using Microsoft.Extensions.Options;
using SpreadsheetLight;

namespace AngloAmerican.Api.Services
{
    public class DataVendorService : IDataVendorService
    {
        private readonly DataConfig _dataConfig;

        public DataVendorService(IOptions<DataConfig> dataConfig)
        {
            _dataConfig = dataConfig.Value;
        }

        public TradeData ExtractDataFromExcel()
        {
            var models = new HashSet<Model>(new ModelComparer());
            var commodities = new HashSet<Commodity>(new CommodityComparer());
            var tradeLogs = new List<TradeLog>();

            using (SLDocument sl = new SLDocument(_dataConfig.PathToExcelData))
            {
                var worksheetNames = sl.GetWorksheetNames();

                foreach (var worksheetName in worksheetNames)
                {
                    sl.SelectWorksheet(worksheetName);

                    var modelName = worksheetName.Split('_')[0];
                    var commodityName = worksheetName.Split('_')[1];

                    var model = new Model(modelName);

                    var commodity = new Commodity(commodityName);

                    if (models.TryGetValue(model, out var existingModel)) model = existingModel;
                    else models.Add(model);

                    if (commodities.TryGetValue(commodity, out var existingCommodity)) commodity = existingCommodity;
                    else commodities.Add(commodity);

                    var items = GetTradeLogs(sl, model.Id, commodity.Id);
                    tradeLogs.AddRange(items);
                }

                return new TradeData(models, commodities, tradeLogs);
            }

        }

        private IEnumerable<TradeLog> GetTradeLogs(SLDocument sl, Guid modelId, Guid commodityId)
        {
            var tradeLogs = new List<TradeLog>();

            var count = sl.GetCells().Count + 1;

            for (var i = 2; i < count; i++) //starting from 2nd line to skip the headers
            {
                var tradeLog = new TradeLog(
                    sl.GetCellValueAsDateTime($"A{i}"),
                    sl.GetCellValueAsString($"B{i}"),
                    sl.GetCellValueAsDecimal($"C{i}"),
                    sl.GetCellValueAsInt32($"D{i}"),
                    sl.GetCellValueAsInt32($"E{i}"),
                    sl.GetCellValueAsDecimal($"F{i}")
                );
                tradeLog.ModelId = modelId;
                tradeLog.CommodityId = commodityId;

                tradeLogs.Add(tradeLog);
            }

            return tradeLogs;
        }
    }
}
