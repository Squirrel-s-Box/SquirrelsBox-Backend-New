using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Resources
{
    public class SharedBoxResource
    {
    }

    public class SaveSharedBoxResource
    {
        public string UserCodeGuest { get; set; }
        public int BoxId { get; set; }
    }

    public class ReadSharedBoxResource : DateAuditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
