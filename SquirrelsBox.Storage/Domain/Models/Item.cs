using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class Item : DateAuditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string? ItemPhoto { get; set; }
        public bool Active { get; set; }

        public ICollection<SectionItemRelationship> SectionItemsList { get; set; }
        public ICollection<Spec> Specs { get; set; }
    }
}
