using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Race
    {
        public Race()
        {
            PatientRaces = new HashSet<PatientRace>();
            PersonRaces = new HashSet<PersonRace>();
            
        }

        public byte RaceId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
        public int? WhiacCode { get; set; }

        public virtual ICollection<PatientRace> PatientRaces { get; set; }
        public virtual ICollection<PersonRace> PersonRaces { get; set; }
        public virtual ICollection<BirthFather> BirthFathers { get; set; } = [];
    }
}   