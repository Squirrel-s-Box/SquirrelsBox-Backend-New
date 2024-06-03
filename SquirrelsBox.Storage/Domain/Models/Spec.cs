using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class Spec : DateAuditory
    {
        public int Id { get; set; }
        public string HeaderName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public bool Active { get; set; }

        public ICollection<ItemSpecRelationship> ItemSpecsList { get; set; }
    }
}
