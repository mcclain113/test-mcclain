using System;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Entities
{
    public partial class BirthFather
    {
        public int BirthFatherId { get; set; }
        
        public int FatherPersonId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        public byte? EducationId { get; set; }

        public byte? EthnicityId { get; set; }

#nullable enable
        public string? MiddleName { get; set; }

        public string? Suffix { get; set; }

        public string? FatherBirthplace { get; set; }

        public string? Ssn { get; set; }

        public virtual EducationLevel? Education { get; set; }

        public virtual Ethnicity? Ethnicity { get; set; }
#nullable disable

        public virtual ICollection<Birth> Births { get; set; } = new List<Birth>();

        public virtual ICollection<Race> Races { get; set; } = new List<Race>();
    }
}