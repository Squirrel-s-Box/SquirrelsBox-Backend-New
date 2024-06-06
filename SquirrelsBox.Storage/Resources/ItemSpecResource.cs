using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Resources
{
    public class ItemSpecResource
    {
    }

    public class ReadItemSpecRelationshipResource
    {
        public ReadSpecResource Item { get; set; }
    }

    public class ReadSpecResource : DateAuditory
    {
        public int Id { get; set; }
        public string HeaderName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    //public class SaveItemSpecListResource
    //{
    //    public int ItemId { get; set; }
    //    public SaveSpecResource Spec { get; set; }
    //}

    public class SaveSpecMassiveResource
    {
        public ICollection<SaveSpecResource> Specs { get; set; }
    }
    public class SaveSpecResource
    {
        public int ItemId { get; set; }
        public string HeaderName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    //public class UpdateItemSpecListResource
    //{
    //    public int? ItemId { get; set; }
    //    public UpdateSpecResource Spec { get; set; }
    //}

    public class UpdateSpecResource
    {
        public string HeaderName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}
