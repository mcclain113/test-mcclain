using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class EducationLevel
    {
        public EducationLevel()
        {
            Patients = new HashSet<Patient>();
        }

        public byte EducationId { get; set; }
        public string EducationLevelName { get; set; }

#nullable enable
        public string? EducationLevelDescription { get; set; }

#nullable disable

        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
        public virtual ICollection<BirthFather> BirthFathers { get; set; } = [];
    }
}