using Base.Generic.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Models
{
    public class UserData : DateAuditory
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string UserPhoto { get; set; }
        public string UserCode { get; set; }
    }
}
