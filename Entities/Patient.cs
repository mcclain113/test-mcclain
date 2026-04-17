using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class Patient
    {
        public Patient()
        {
            PatientMedicationLists = new HashSet<PatientMedicationList>();
            Documents = new HashSet<Document>();
            Encounters = new HashSet<Encounter>();
            PatientAlerts = new HashSet<PatientAlert>();
            PatientAliases = new HashSet<PatientAlias>();
            PatientContactDetails = new HashSet<PatientContactDetail>();    // move to Person
            PatientEmergencyContacts = new HashSet<PatientEmergencyContact>();
            PatientLanguages = new HashSet<PatientLanguage>();  // move to Person
            PatientRaces = new HashSet<PatientRace>();  // move to Person
            PatientInsurances = new HashSet<PatientInsurance>();
        }

        public string Mrn { get; set; }
        public string FirstName { get; set; }   // move to Person
        public string LastName { get; set; }    // move to Person

#nullable enable
        public int? PersonId {get; set;}    // must have FK
        public string? MiddleName { get; set; } // move to Person
        public string? MaidenName { get; set; } // move to Person
        public short? ReligionId { get; set; }  // move to Person
        public string? MothersMaidenName { get; set; }  // move to Person
        public string? Ssn { get; set; }    // move to Person
        public DateTime? Dob { get; set; }  // move to Person
        public DateTime? DeathDate { get; set; }    // move to Person
        public byte? SexId { get; set; }    // move to Person
        public byte? GenderId { get; set; } // move to Person

        public byte? MaritalStatusId { get; set; }  // move to Person
        public byte? EthnicityId { get; set; }  // move to Person
        public int? EmploymentId { get; set; }  // move to Person
        public byte? EducationId { get; set; }  // move to Person

        public int? EditedBy { get; set; }
        public byte? LegalStatusId { get; set; }
        public byte? GenderPronounId { get; set; } = null;  // move to Person
        public string? Birthplace { get; set; } // move to Person
#nullable disable
        public bool DeceasedLiving { get; set; }    // move to Person
        public bool InterpreterNeeded { get; set; } // move to Person
        public bool IsCurrentMilitaryServiceMember { get; set; }    // move to Person
        public bool IsVeteran { get; set; } // move to Person
        public DateTime LastModified { get; set; }
        public int FacilityId { get; set; }

        
        public virtual Person Person { get; set; }  // singular relationship as Patient can have only one Person
        public virtual Employment Employment { get; set; }  // move to Person
        public virtual Ethnicity Ethnicity { get; set; }    // move to Person
        public virtual EducationLevel EducationLevel { get; set; }  // move to Person
        public virtual Gender Gender { get; set; }  // move to Person
        public virtual GenderPronoun GenderPronoun { get; set; }    // move to Person
        public virtual MaritalStatus MaritalStatus { get; set; }    // move to Person
        public virtual Religion Religion { get; set; }  // move to Person
        public virtual Sex Sex { get; set; }    // move to Person
        public virtual LegalStatus LegalStatus { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Encounter> Encounters { get; set; }
        public virtual ICollection<PatientAlert> PatientAlerts { get; set; }
        public virtual ICollection<PatientAlias> PatientAliases { get; set; }
        public virtual ICollection<PatientMedicationList> PatientMedicationLists { get; set; } 
        public virtual ICollection<PatientContactDetail> PatientContactDetails { get; set; }    // move to Person
        public virtual ICollection<PatientEmergencyContact> PatientEmergencyContacts { get; set; } 
        public virtual ICollection<PatientLanguage> PatientLanguages { get; set; }  // move to Person
        public virtual ICollection<PatientRace> PatientRaces { get; set; }  // move to Person
        public virtual ICollection<PatientInsurance> PatientInsurances { get; set; }
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        public virtual ICollection<Birth> Births { get; set; } = new List<Birth>();
        public virtual ICollection<Newborn> Newborns { get; set; } = new List<Newborn>();
    }
}
