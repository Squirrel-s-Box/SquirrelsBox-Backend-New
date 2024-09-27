using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class Box : DateAuditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserCodeOwner { get; set; }
        public bool Favourite { get; set; }
        public bool Active { get; set; }
        public string? UserCodeLog { get; set; }

        public ICollection<BoxSectionRelationship> BoxSectionsList { get; set; }
        public ICollection<SharedBox> SharedBoxes { get; set; }
    }
}
