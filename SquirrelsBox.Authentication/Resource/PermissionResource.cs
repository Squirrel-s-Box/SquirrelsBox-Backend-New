namespace SquirrelsBox.Authentication.Resource
{
    public class PermissionResource
    {
    }

    public class SavePermissionResource
    {
        public string Collection { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
