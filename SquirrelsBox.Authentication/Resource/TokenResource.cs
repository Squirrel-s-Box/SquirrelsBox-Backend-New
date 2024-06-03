namespace SquirrelsBox.Authentication.Resource
{
    public class TokenResource
    {
    }

    public class SaveFoundTokenByUserIdResource
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }

    public class SaveTokenResource
    {
        public string Token { get; set; }
    }
}
