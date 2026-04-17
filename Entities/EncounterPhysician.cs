using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class EncounterPhysician
    {
        public long EncounterPhysiciansId { get; set; }
        public int PhysicianId { get; set; }
        public int PhysicianRoleId { get; set; }
        public DateTime LastModified { get; set; }
        public long? EncounterId { get; set; }

        public virtual Physician Physician { get; set; }
        public virtual PhysicianRole PhysicianRole { get; set; }

        public virtual Encounter Encounter { get; set; }
    }
}
