using AngloAmerican.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngloAmerican.Api.Controllers
{
    [Route("api/trade-logs")]
    [ApiController]
    public class TradeLogController : ControllerBase
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeLogController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository ?? throw new ArgumentException(nameof(tradeRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetTradeLogs(int days = 5)
        {
            var tradeLogs = await _tradeRepository.GetTradeLogsAsync(days);
            return Ok(tradeLogs);
        }
    }
}
