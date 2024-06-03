namespace SquirrelsBox.Authentication.Resource
{
    public class AccessRefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string Code { get; set; }
    }
}
