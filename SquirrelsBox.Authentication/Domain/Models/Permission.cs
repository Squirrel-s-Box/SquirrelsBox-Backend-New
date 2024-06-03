namespace SquirrelsBox.Authentication.Domain.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Collection { get; set; }
        //public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
