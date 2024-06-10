using static System.Collections.Specialized.BitVector32;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class BoxSectionRelationship
    {
        public int Id { get; set; }
        public int BoxId { get; set; }
        public int SectionId { get; set; }

        public Box? Box { get; set; }
        public Section? Section { get; set; }

        public BoxSectionRelationship()
        {
            Section = new Section();
        }
    }
}
