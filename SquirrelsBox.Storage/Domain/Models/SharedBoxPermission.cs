using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SquirrelsBox.Storage.Domain.Models
{
    public class SharedBoxPermission
    {
        public int Id { get; set; }

        public int SharedBoxId { get; set; }

        public int PermissionId { get; set; }

        public bool IsAllowed { get; set; }

        
        public SharedBox SharedBox { get; set; }
    }
}
