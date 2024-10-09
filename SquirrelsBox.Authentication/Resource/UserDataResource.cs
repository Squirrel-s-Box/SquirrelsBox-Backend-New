namespace SquirrelsBox.Authentication.Resource
{
    public class UserDataResource
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string UserPhoto { get; set; }
    }

    public class SaveUserDataResource
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string UserPhoto { get; set; }
    }
}
