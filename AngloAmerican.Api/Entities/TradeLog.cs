using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngloAmerican.Api.Entities
{
    public class TradeLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Contract { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public int NewTradeAction { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal PnlDaily { get; set; }

        [ForeignKey("ModelId")]
        public virtual Model Model { get; set; } = null!;
        public Guid ModelId { get; set; }

        [ForeignKey("CommodityId")]
        public virtual Commodity Commodity { get; set; } = null!;
        public Guid CommodityId { get; set; }


        public TradeLog(DateTime date, string contract, decimal price, int position, int newTradeAction, decimal pnlDaily)
        {
            Id = Guid.NewGuid();
            Date = date;
            Contract = contract;
            Price = price;
            Position = position;
            NewTradeAction = newTradeAction;
            PnlDaily = pnlDaily;
        }
    }
}
