using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.Entities;
using System.Linq;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class MothersInformationViewModel
    {
        // Person-based migration
        public int? PersonId { get; set; }
        public int? BirthId { get; set; }
        // Patient Search Properties
        public string SearchTerm { get; set; }
        public bool IsExistingPatient { get; set; }
        public string Mrn { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; } // Current Legal Name Suffix

        // Mother's Given Name at Birth (as on birth certificate)
        public string BirthFirstName { get; set; }
        public string BirthMiddleName { get; set; }
        public string BirthLastName { get; set; }
        public string BirthSuffix { get; set; }
        public bool IsMarried { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public string SSN { get; set; }

        // Address Information
        public bool MailingAddressSameAsResidential { get; set; } = true;
        // Dropdown: City, Township, Village, Outside city limits
        public string HouseholdInsideCityLimits { get; set; }
        public List<SelectListItem> HouseholdInsideCityLimitsOptions { get; set; }

        // Address Entities
        public Address ResidentialAddress { get; set; }
        public Address MailingAddress { get; set; }
        public string ResidentialApartmentNumber { get; set; }
        public string ResidentialCounty { get; set; }
        public string ResidentialCountry { get; set; }
        public string MailingApartmentNumber { get; set; }
        public string MailingCounty { get; set; }
        public string MailingCountry { get; set; }
        public string MotherTelephoneNumber { get; set; }

        // Dropdown Entity References
        public Religion Religion { get; set; }
        public int? ReligionId { get; set; } // For dropdown selection
        public Ethnicity Ethnicity { get; set; }
        public int? EthnicityId { get; set; } // For dropdown selection
        public EducationLevel EducationLevel { get; set; }
        public int? EducationId { get; set; } // For dropdown selection

        // Marriage Information
        public bool MotherMarriedInPast { get; set; }
        public bool MotherMarriedDuringPregnancy { get; set; }
        public bool IsFatherHusbandOfMother { get; set; }
        public string MaritalStatusName { get; set; }
        public bool? PaternityAcknowledgementSigned { get; set; }

        // Dropdown Lists for UI
        public List<SelectListItem> Religions { get; set; }
        public List<SelectListItem> Ethnicities { get; set; }
        public List<SelectListItem> EducationLevels { get; set; }
        public List<SelectListItem> Races { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<byte> SelectedRaceIds { get; set; }
        public bool NoneOfTheAboveRaces => !SelectedRaceIds.Any();
        public bool IsNewPatient => string.IsNullOrEmpty(Mrn);
        public bool HasRaceSelections => SelectedRaceIds.Count != 0;

        // Constructor - Initialize all reference types and collections
        public MothersInformationViewModel()
        {
            // Initialize Address objects to prevent null reference exceptions
            ResidentialAddress = new Address();
            MailingAddress = new Address();
            SelectedRaceIds = new List<byte>();
            HouseholdInsideCityLimitsOptions = new List<SelectListItem> {
                new SelectListItem { Text = "City", Value = "City" },
                new SelectListItem { Text = "Township", Value = "Township" },
                new SelectListItem { Text = "Village", Value = "Village" },
                new SelectListItem { Text = "Outside city limits", Value = "Outside" }
            };
            // Set default values
            MailingAddressSameAsResidential = true;
            IsExistingPatient = false;
        }

        // Populate from Person entity
        public void FromPerson(Person person)
        {
            if (person == null) return;
            PersonId = person.PersonId;
            FirstName = person.FirstName;
            MiddleName = person.MiddleName;
            LastName = person.LastName;
            DateOfBirth = person.Dob;
            BirthPlace = person.Birthplace;
            SSN = person.Ssn;
            Religion = person.Religion;
            ReligionId = person.ReligionId; // Set for dropdown
            Ethnicity = person.Ethnicity;
            EthnicityId = person.EthnicityId;
            EducationLevel = person.EducationLevel;
            EducationId = person.EducationId;
            // Map races
            if (person.PersonRaces != null)
                SelectedRaceIds = person.PersonRaces.Select(r => r.RaceId).ToList();
            // Map addresses (if using PersonContactDetail)
            if (person.PersonContactDetail != null)
            {
                if (person.PersonContactDetail.ResidenceAddress != null)
                    ResidentialAddress = person.PersonContactDetail.ResidenceAddress;
                if (person.PersonContactDetail.MailingAddress != null)
                {
                    MailingAddress = person.PersonContactDetail.MailingAddress;
                    MailingAddressSameAsResidential = false;
                }
                else
                {
                    MailingAddressSameAsResidential = true;
                    MailingAddress = ResidentialAddress;
                }
            }
        }

        // Update Person entity from ViewModel
        public void UpdatePerson(Person person)
        {
            if (person == null) return;
            if (!string.IsNullOrWhiteSpace(FirstName))
                person.FirstName = FirstName;
            if (!string.IsNullOrWhiteSpace(MiddleName))
                person.MiddleName = MiddleName;
            if (!string.IsNullOrWhiteSpace(LastName))
                person.LastName = LastName;
            if (DateOfBirth != null)
                person.Dob = DateOfBirth;
            if (!string.IsNullOrWhiteSpace(BirthPlace))
                person.Birthplace = BirthPlace;
            if (!string.IsNullOrWhiteSpace(SSN))
                person.Ssn = SSN;
            // Always update ReligionId, even if clearing
            if (ReligionId.HasValue)
                person.ReligionId = (short?)ReligionId;
            else if (Religion != null)
                person.ReligionId = Religion.ReligionId;
            else
                person.ReligionId = null;
            if (EthnicityId.HasValue)
                person.EthnicityId = (byte?)EthnicityId;
            else if (Ethnicity != null)
                person.EthnicityId = Ethnicity.EthnicityId;
            else
                person.EthnicityId = null;
            if (EducationId.HasValue)
                person.EducationId = (byte?)EducationId;
            else if (EducationLevel != null)
                person.EducationId = EducationLevel.EducationId;
            else
                person.EducationId = null;
            // Races and addresses handled separately in controller/repo
        }

        // Constructor for existing patient
        public MothersInformationViewModel(Patient patient) : this()
        {
            if (patient != null)
            {
                IsExistingPatient = true;
                Mrn = patient.Mrn;
                FirstName = patient.FirstName;
                MiddleName = patient.MiddleName;
                LastName = patient.LastName;
                DateOfBirth = patient.Dob;
                BirthPlace = patient.Birthplace;
                SSN = patient.Ssn;

                // Populate related entities if they exist
                Religion = patient.Religion;
                Ethnicity = patient.Ethnicity;
                EducationLevel = patient.EducationLevel;

                // Handle race selections
                if (patient.PatientRaces != null)
                {
                    SelectedRaceIds = patient.PatientRaces.Select(pr => pr.RaceId).ToList();
                }
            }
        }
        public MothersInformationViewModel(Patient patient, Birth birth) : this(patient)
        {
            if (birth != null)
            {
                BirthId = birth.BirthId;
                MotherMarriedInPast = birth.MotherMarriedInPast ?? false;
                MotherMarriedDuringPregnancy = birth.MotherMarriedDuringPregnancy ?? false;
                IsFatherHusbandOfMother = birth.IsFatherHusbandOfMother ?? false;
                PaternityAcknowledgementSigned = birth.PaternityAcknowledgementSigned;
            }
        }
        // For existing patient WITH birth data (Edit/View mode)
        public void UpdateBirthRecord(Birth birth)
        {
            birth.MotherMrn = this.Mrn;
            birth.MotherMarriedInPast = this.MotherMarriedInPast;
            birth.MotherMarriedDuringPregnancy = this.MotherMarriedDuringPregnancy;
            birth.IsFatherHusbandOfMother = this.IsFatherHusbandOfMother;

            // Update mother (Patient) info if available
            if (birth.MotherMrnNavigation != null)
            {
                var patient = birth.MotherMrnNavigation;
                patient.FirstName = this.FirstName;
                patient.MiddleName = this.MiddleName;
                patient.LastName = this.LastName;
                patient.Dob = this.DateOfBirth;
                patient.Birthplace = this.BirthPlace;
                patient.Ssn = this.SSN;
                // Religion, Ethnicity, EducationLevel are navigation properties
                if (this.Religion != null)
                    patient.ReligionId = this.Religion.ReligionId;
                if (this.Ethnicity != null)
                    patient.EthnicityId = this.Ethnicity.EthnicityId;
                if (this.EducationLevel != null)
                    patient.EducationId = this.EducationLevel.EducationId;
                // TODO: Update address, races, and other related info as needed
            }
        }
        // Convenience property for full name
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim().Replace("  ", " ");

        public bool MotherMarriedDuringPreganancyThroughBirth { get; internal set; }
    }
}