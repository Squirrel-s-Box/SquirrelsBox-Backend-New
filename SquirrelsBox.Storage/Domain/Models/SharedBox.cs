using Base.Generic.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class SharedBox : DateAuditory
    {
        public int Id { get; set; }
        public string UserCodeGuest { get; set; }
        public bool State { get; set; }

        public int BoxId { get; set; }
        public Box Box { get; set; }
        public ICollection<SharedBoxPermission> SharedBoxPermissions { get; set; }
    }
}
