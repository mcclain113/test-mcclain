using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Facility
    {
        public Facility()
        {
            Encounters = new HashSet<Encounter>();
            ProgramFacilities = new HashSet<ProgramFacility>();
            UserFacilities = new HashSet<UserFacility>();
        }

        public int FacilityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AddressId { get; set; }
        public string Phone { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Encounter> Encounters { get; set; }
        public virtual ICollection<ProgramFacility> ProgramFacilities { get; set; }
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        public virtual ICollection<UserFacility> UserFacilities { get; set; }
        public virtual ICollection<Birth> Births { get; set; } = [];
        public virtual ICollection<Patient> Patients { get; set; } = [];
    }
}
