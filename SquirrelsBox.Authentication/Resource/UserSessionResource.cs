namespace SquirrelsBox.Authentication.Resource
{
    public class UserSessionResource
    {
    }

    public class UpdateUserSessionResource
    {
        public string Token { get; set; }
        public string OldToken { get; set; }
    }
}
