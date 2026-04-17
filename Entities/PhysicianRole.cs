using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PhysicianRole
    {
        public PhysicianRole()
        {
            EncounterPhysicians = new HashSet<EncounterPhysician>();
        }

        public int PhysicianRoleId { get; set; }

        [Required(ErrorMessage ="Please enter the Role Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<EncounterPhysician> EncounterPhysicians { get; set; }
    }
}
