using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngloAmerican.Api.Entities
{
    public class Model
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<TradeLog> TradeLogs { get; set; } = new List<TradeLog>();

        public Model(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }

    public class ModelComparer : EqualityComparer<Model>
    {
        private IEqualityComparer<string> comparer = EqualityComparer<string>.Default;

        public override bool Equals(Model? left, Model? right)
        {
            return comparer.Equals(left?.Name, right?.Name);
        }

        public override int GetHashCode(Model rule)
        {
            return comparer.GetHashCode(rule.Name);
        }
    }
}
