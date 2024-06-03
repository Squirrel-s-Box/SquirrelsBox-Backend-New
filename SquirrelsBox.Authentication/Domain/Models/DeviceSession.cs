using Base.Generic.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Models
{
    public class DeviceSession : DateAuditory
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }

        public AccessSession User { get; set; }
    }
}
