using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Employment
    {
        public Employment()
        {
            Patients = new HashSet<Patient>();
        }

        public int EmploymentId { get; set; }
        public string EmployerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public DateTime LastModified { get; set; }
        public int? AddressId { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
        public virtual Address Address { get; set; }
    }
}
