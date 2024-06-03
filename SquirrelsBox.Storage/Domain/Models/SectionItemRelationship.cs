namespace SquirrelsBox.Storage.Domain.Models
{
    public class SectionItemRelationship
    {
        public int SectionId { get; set; }
        public int ItemId { get; set; }

        public Section Section { get; set; }
        public Item Item { get; set; }
    }
}
