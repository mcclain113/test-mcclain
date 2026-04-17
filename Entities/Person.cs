using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class Person
    {
        public Person()
        {
            // add as needed to initialize within empty constructor
            PersonLanguages = new HashSet<PersonLanguage>();
            PersonRaces = new HashSet<PersonRace>();
        }

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

#nullable enable
        public string? MiddleName { get; set; }
        public string? MaidenName { get; set; }
        public string? Suffix { get; set; }
        public short? ReligionId { get; set; }
        public string? MothersMaidenName { get; set; }
        public string? Ssn { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? DeathDate { get; set; }
        public byte? SexId { get; set; }
        public byte? GenderId { get; set; }

        public byte? MaritalStatusId { get; set; }
        public byte? EthnicityId { get; set; }
        public int? EmploymentId { get; set; }
        public byte? EducationId { get; set; }

        public int? EditedBy { get; set; }
        public byte? GenderPronounId { get; set; } = null;
        public string? Birthplace { get; set; }
#nullable disable
        public bool DeceasedLiving { get; set; }
        public bool InterpreterNeeded { get; set; }
        public bool IsCurrentMilitaryServiceMember { get; set; }
        public bool IsVeteran { get; set; }
        public DateTime LastModified { get; set; }

#nullable enable
        public virtual Employment? Employment { get; set; }
        public virtual Ethnicity? Ethnicity { get; set; }
        public virtual EducationLevel? EducationLevel { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual GenderPronoun? GenderPronoun { get; set; }
        public virtual MaritalStatus? MaritalStatus { get; set; }
        public virtual PersonContactDetail? PersonContactDetail { get; set; }
        public virtual Religion? Religion { get; set; }
        public virtual Sex? Sex { get; set; }
        
#nullable disable
        public virtual ICollection<PersonLanguage> PersonLanguages { get; set; }
        public virtual ICollection<PersonRace> PersonRaces { get; set; }
        
        // Allows you to find all children this woman has given birth to
        public virtual ICollection<Birth> BirthsAsMother { get; set; } = new List<Birth>();

        // Allows you to find all children this man has fathered
        public virtual ICollection<Birth> BirthsAsFather { get; set; } = new List<Birth>();

        // Contains the single birth record representing when this person was born
        public virtual ICollection<Birth> BirthsAsNewborn { get; set; } = new List<Birth>();
        
    }
}