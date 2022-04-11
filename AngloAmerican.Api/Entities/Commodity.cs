using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngloAmerican.Api.Entities
{
    public class Commodity
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //[ForeignKey("ModelId")]
        //public Model? Model { get; set; }
        //public Guid ModelId { get; set; }

        //public ICollection<Model> Models { get; set; } = new List<Model>();
        //[ForeignKey("TradeLogId")]
        //public TradeLog TradeLog { get; set; }

        public ICollection<TradeLog> TradeLogs { get; set; } = new List<TradeLog>();
        //public ICollection<Model> Models { get; set; } = new List<Model>();

        public Commodity(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }

    public class CommodityComparer : EqualityComparer<Commodity>
    {
        private IEqualityComparer<string> _c = EqualityComparer<string>.Default;

        public override bool Equals(Commodity? l, Commodity? r)
        {
            return _c.Equals(l?.Name, r?.Name);
        }

        public override int GetHashCode(Commodity rule)
        {
            return _c.GetHashCode(rule.Name);
        }
    }
}
