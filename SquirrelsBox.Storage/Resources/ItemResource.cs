using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Resources
{
    public class ItemResource
    {
    }

    public class ReadSectionItemRelationshipResource
    {
        public ReadItemResource Item { get; set; }
    }
    public class ReadItemResource : DateAuditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string ItemPhoto { get; set; }
    }

    public class SaveSectionItemResource
    {
        public int SectionId { get; set; }
        public SaveItemResource Item { get; set; }
        public IFormFile? Image { get; set; }
    }
    public class SaveItemResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string? ItemPhoto { get; set; }
    }

    public class UpdateSectionItemListResource
    {
        public int? SectionId { get; set; }
        public UpdateItemResource? Item { get; set; }
    }

    public class UpdateItemResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string ItemPhoto { get; set; }
    }
}
