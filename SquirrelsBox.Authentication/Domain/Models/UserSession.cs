using Base.Generic.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Models
{
    public class UserSession : DateAuditory
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string? OldToken { get; set; }
        public int UserId { get; set; }

        public AccessSession User { get; set; }
    }
}
