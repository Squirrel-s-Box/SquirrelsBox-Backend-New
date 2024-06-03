namespace Base.Security.Sha256M
{
    public class JwtAccess
    {
        public string UserCode { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,  //1
        User    //2
    }

    public class JwtKeys
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
