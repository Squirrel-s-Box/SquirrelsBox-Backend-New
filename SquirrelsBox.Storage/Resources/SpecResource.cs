using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Resources
{
    public class SpecResource
    {
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

    public class UpdateSpecMassiveResource
    {
        public ICollection<UpdateSpecResource> Specs { get; set; }
    }
    public class UpdateSpecResource
    {
        public int Id { get; set; }
        public string HeaderName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public bool Active { get; set; }
    }

    public class DeleteSpecMassiveResource
    {
        public ICollection<int> Ids { get; set; }
    }
}
