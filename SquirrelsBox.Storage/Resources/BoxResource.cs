using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Resources
{
    public class BoxResource
    {
    }
    public class ReadBoxResource : DateAuditory
    {
        public string Name { get; set; }
    }

    public class SaveBoxResource
    {
        public string Name { get; set; }
    }

    public class UpdateBoxResource
    {
        public string Name { get; set; }
        public bool Favourite { get; set; }
        public bool State { get; set; }
    }

    public class DeleteBoxResource
    {
        public string Id { get; set; }
        public bool Cascade { get; set; }
    }
}
