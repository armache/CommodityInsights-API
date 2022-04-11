using AngloAmerican.Api.Models;

namespace AngloAmerican.Api.Services
{
    public interface ITradeRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<TradeLogDto>> GetTradeLogsAsync(int days);
    }
}
