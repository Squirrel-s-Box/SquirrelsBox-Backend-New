namespace SquirrelsBox.Authentication.Resource
{
    public class AssignedPermissionResource
    {
    }

    public class ListAssignedPermission
    {
        public int Code { get; set; }
    }

    public class SaveAssignedPermissionResource
    {
        public string UserCode { get; set; }
        //public int ElementId { get; set; }
        public int PermissionId { get; set; }
    }
}
