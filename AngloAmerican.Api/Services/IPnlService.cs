using AngloAmerican.Api.Models;

namespace AngloAmerican.Api.Services
{
    public interface IPnlService
    {
        IEnumerable<PnlYtd> ExtractHistoricalPnl(Dictionary<DateTime, decimal> pnlDictionary);
    }
}
