using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Religion
    {
        public Religion()
        {
            Patients = new HashSet<Patient>();
        }

        public short ReligionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
