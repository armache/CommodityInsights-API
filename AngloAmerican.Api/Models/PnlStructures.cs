using System.Linq;

namespace AngloAmerican.Api.Models
{
    public struct PnlYtd
    {
        public int Year { get; }
        public List<PnlMonthly> MonthlyPnls { get; }
        public decimal ClosingPnl => MonthlyPnls.Sum(a => a.ClosingPnl);

        public PnlYtd(int year)
        {
            Year = year;
            MonthlyPnls = new List<PnlMonthly>();
        }
    }

    public struct PnlMonthly
    {
        public int Month { get; }
        public List<PnlDaily> DailyPnls { get; }
        public decimal ClosingPnl => DailyPnls.Sum(a => a.Pnl);

        public PnlMonthly(int month)
        {
            Month = month;
            DailyPnls = new List<PnlDaily>();
        }
    }

    public struct PnlDaily
    {
        public int Day { get; set; }
        public decimal Pnl { get; set; }
    }
}
