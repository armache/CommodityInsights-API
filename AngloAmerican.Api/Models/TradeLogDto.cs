namespace AngloAmerican.Api.Models
{
    public class TradeLogDto
    {
        public DateTime Date { get; set; }
        public string Contract { get; set; } = null!;
        public decimal Price { get; set; }
        public int Position { get; set; }
        public int NewTradeAction { get; set; }
        public decimal PnlDaily { get; set; }
        public string Model { get; set; } = null!;
        public string Commodity { get; set; } = null!;

        public TradeLogDto(DateTime date, string contract, decimal price, int position,
            int newTradeAction, decimal pnlDaily, string model, string commodity)
        { 
            Date = date;
            Contract = contract;
            Price = price;
            Position = position;
            NewTradeAction = newTradeAction;
            PnlDaily = pnlDaily;
            Model = model;
            Commodity = commodity;
        }
    }
}
