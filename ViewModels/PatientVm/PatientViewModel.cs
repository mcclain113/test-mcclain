using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;

namespace  IS_Proj_HIT.ViewModels.PatientVm
{
    public class PatientViewModel
    {
        //Core Entities
        public Patient Patient {get; set;}
        public Person Person {get; set;}

        // Person-scoped items
        public Employment Employment {get; set;}
        public PersonContactDetail PersonContactDetail {get; set;}
        public PersonModeOfContact PersonModeOfContact {get; set;}
        public PersonContactTime PersonContactTime {get; set;}
        public List<PersonLanguage> PersonLanguages { get; set; } = new List<PersonLanguage>();
        public List<short> SelectedLanguageIds { get; set; } = new List<short>();
        public List<PersonRace> PersonRaces { get; set; } = new List<PersonRace>();
        public List<byte> SelectedRaceIds { get; set; } = new List<byte>();
        

        // Patient-scoped items
        public LegalStatus LegalStatus {get; set;}
        public PatientInsurance PrimaryInsurance {get; set;}
        public PatientInsurance SecondaryInsurance {get; set;}
        public PatientInsurance TertiaryInsurance {get; set;}

        // public PatientLanguage PatientLanguage {get; set;}
        // public PatientContactDetail PatientContactDetail {get; set;}
        // public PatientModeOfContact PatientModeOfContact {get; set;}
        // public PatientContactTime PatientContactTime {get; set;}
        
        // Addresses and UI helpers
        public Address EmploymentAddress {get; set;}
        public Address EmergencyContactAddress {get; set;} 
        public Address ResidenceAddress {get; set;}
        public Address MailingAddress {get; set;}
        public bool ResidenceSameAsMailing { get; set; }

        public List<PreferredModeOfContact> PreferredModeOfContacts { get; set; } 
        public List<int> SelectedPreferredModeOfContactIds { get; set; }

        public List<PreferredContactTime> PreferredContactTimes { get; set; } 
        public List<int> SelectedPreferredContactTimeIds { get; set; }

        public int? RequestId {get; set;}
        public List<PatientAlias> PatientAliases { get; set; } = new List<PatientAlias>();
        public List<PatientEmergencyContact> EmergencyContact {get; set;} = new List<PatientEmergencyContact>();
        
        // public List<PatientRace> PatientRaces { get; set; } = new List<PatientRace>();

        // Constructors
        public PatientViewModel()
        {
            PreferredModeOfContacts = new List<PreferredModeOfContact>();
            SelectedPreferredModeOfContactIds = new List<int>();
            PreferredContactTimes = new List<PreferredContactTime>();
            SelectedPreferredContactTimeIds = new List<int>();
            MailingAddress = new Address();
            ResidenceSameAsMailing = true;
            PersonRaces = new List<PersonRace>();
            SelectedRaceIds = new List<byte>();
            SelectedLanguageIds = new List<short>();
            PatientAliases = new List<PatientAlias>();
            EmergencyContact = new List<PatientEmergencyContact>();
        }
        
        public PatientViewModel
            (Patient patient, Person person, Employment employment, Address employmentAddress, 
                PatientInsurance primaryInsurance, PatientInsurance secondaryInsurance, PatientInsurance tertiaryInsurance,
                PersonContactDetail personContactDetail, Address residenceAddress, Address mailingAddress, 
                List<PatientEmergencyContact> emergencyContact,
                List<PatientAlias> patientAliases,
                List<PersonLanguage> personLanguages, 
                List<PersonRace> personRaces, 
                List<byte> selectedRaceIds,
                List<short> selectedLanguageIds){
            this.Patient = patient;
            this.Person = person;
            this.Employment = employment;
            this.EmploymentAddress = employmentAddress;
            this.PrimaryInsurance = primaryInsurance;
            this.SecondaryInsurance = secondaryInsurance;
            this.TertiaryInsurance = tertiaryInsurance;
            this.PersonContactDetail = personContactDetail;
            this.ResidenceAddress = residenceAddress;
            this.MailingAddress = mailingAddress;
            this.EmergencyContact = emergencyContact ?? new List<PatientEmergencyContact>();
            this.PatientAliases = patientAliases ?? new List<PatientAlias>();
            this.PersonLanguages = personLanguages ?? new List<PersonLanguage>();
            this.PersonRaces = personRaces ?? new List<PersonRace>();
            this.SelectedRaceIds = selectedRaceIds ?? new List<byte>();
            this.SelectedLanguageIds = selectedLanguageIds ?? new List<short>();

            this.SelectedPreferredModeOfContactIds = this.PersonContactDetail?.PersonModeOfContacts?.Select(m => m.ModeOfContactId).ToList()
            ?? new List<int>();
            this.SelectedPreferredContactTimeIds = this.PersonContactDetail?.PersonContactTimes?.Select(t => t.ContactTimeId).ToList()
            ?? new List<int>();

        }
    }
}