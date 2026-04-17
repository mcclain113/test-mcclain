using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Gender
    {
        public  Gender()
        {
            Patients = new HashSet<Patient>();
        }

        public byte GenderId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}