using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngloAmerican.Api.Entities
{
    public class Model
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //[ForeignKey("CommodityId")]
        //public Commodity? Commodity { get; set; }
        //public Guid CommodityId { get; set; }

        //public ICollection<Commodity> Commodities { get; set; } = new List<Commodity>();

        //[ForeignKey("TradeLogId")]
        //public TradeLog TradeLog { get; set; }

        public ICollection<TradeLog> TradeLogs { get; set; } = new List<TradeLog>();
        //public ICollection<Commodity> Commodities { get; set; } = new List<Commodity>();

        public Model(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }

    public class ModelComparer : EqualityComparer<Model>
    {
        private IEqualityComparer<string> _c = EqualityComparer<string>.Default;

        public override bool Equals(Model? l, Model? r)
        {
            return _c.Equals(l?.Name, r?.Name);
        }

        public override int GetHashCode(Model rule)
        {
            return _c.GetHashCode(rule.Name);
        }
    }
}
