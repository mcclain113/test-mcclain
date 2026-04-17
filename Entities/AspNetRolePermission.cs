namespace IS_Proj_HIT.Entities
{
    public class AspNetRolePermission
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        // Navigation properties
        public AspNetRole Role { get; set; }
        public AspNetPermission Permission { get; set; }
        }
}