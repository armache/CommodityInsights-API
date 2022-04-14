using AngloAmerican.Api.Models;

namespace AngloAmerican.Api.Services
{
    public interface IDataVendorService
    {
        TradeData ExtractDataFromExcel();
    }
}
