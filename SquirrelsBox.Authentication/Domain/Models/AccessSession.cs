using Base.Generic.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Models
{
    public class AccessSession : DateAuditory
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Code { get; set; }
        public string RefreshToken { get; set; }
        public int Attempt { get; set; }

        public IList<UserSession> SessionsTokens { get; set; }
        public IList<DeviceSession> DevicesTokens { get; set; }
    }
}
