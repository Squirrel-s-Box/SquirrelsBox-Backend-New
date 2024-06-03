using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class Section : DateAuditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; }

        public ICollection<BoxSectionRelationship> BoxSectionsList { get; set; }
        public ICollection<SectionItemRelationship> SectionItemsList { get; set; }

    }
}
