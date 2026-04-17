using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public class AspNetPermission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ModuleOrForm { get; set; }

        // Navigation property
        public ICollection<AspNetRolePermission> RolePermissions { get; set; }
    }
}