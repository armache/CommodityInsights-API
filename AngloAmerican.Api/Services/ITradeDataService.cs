using AngloAmerican.Api.Models;

namespace AngloAmerican.Api.Services
{
    public interface ITradeDataService
    {
        TradeData ExtractDataFromExcel();
    }
}
