using AngloAmerican.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngloAmerican.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ITradeRepository _tradeRepository;

        public ProductController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository ?? throw new ArgumentException(nameof(tradeRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _tradeRepository.GetProductsAsync();
            return Ok(products);
        }
    }
}
