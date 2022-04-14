using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngloAmerican.Api.Entities
{
    public class Commodity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<TradeLog> TradeLogs { get; set; } = new List<TradeLog>();

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
