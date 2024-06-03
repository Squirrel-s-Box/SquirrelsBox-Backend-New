using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Resources
{
    public class BoxSectionResource
    {
    }
    public class ReadBoxSectionRelationshipResource
    {
        public ReadSectionResource Section { get; set; }
    }

    public class ReadSectionResource : DateAuditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class SaveBoxSectionsListResource
    {
        public int BoxId { get; set; }
        public SaveSectionResource Section { get; set; }
    }

    public class SaveSectionResource
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class UpdateBoxSectionsListResource
    {
        public int? BoxId { get; set; }
        public UpdateSectionResource Section { get; set; }
    }

    public class UpdateSectionResource
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public bool State { get; set; }
    }
}
