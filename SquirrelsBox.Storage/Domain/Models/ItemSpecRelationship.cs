namespace SquirrelsBox.Storage.Domain.Models
{
    public class ItemSpecRelationship
    {
        public int ItemId { get; set; }
        public int SpecId { get; set; }

        public Item Item { get; set; }
        public Spec Spec { get; set; }
    }
}
