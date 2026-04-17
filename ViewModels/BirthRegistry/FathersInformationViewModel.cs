using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.Entities;
using System.Linq;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class FatherInformationViewModel
    {
        public int? BirthId { get; set; }
        // BirthFather Properties
        public int? BirthFatherId { get; set; } // Null for new, populated for existing
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; } // Separate field unlike mother
        public DateTime? DateOfBirth { get; set; } // DateOnly like BirthFather entity => Reversed!
        public string FatherBirthplace { get; set; }
        public string SSN { get; set; }
        public bool? HasPaternityAcknowledgement { get; set; } = null; // Nullable to enforce explicit selection

        // Dropdown Entity References (these can be nullable)
        public EducationLevel EducationLevel { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public List<byte> SelectedRaceIds { get; set; }

        // Dropdown Lists for UI
        public List<SelectListItem> EducationLevels { get; set; }
        public List<SelectListItem> Ethnicities { get; set; }
        public List<SelectListItem> Races { get; set; }
        public bool HasFatherInformation =>
            !string.IsNullOrWhiteSpace(FirstName) ||
            !string.IsNullOrWhiteSpace(LastName) ||
            !string.IsNullOrWhiteSpace(SSN);

        public bool NoneOfTheAboveRaces => SelectedRaceIds.Count == 0;

        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim().Replace("  ", " ");


        // Constructor - Initialize all reference types and collections
        public FatherInformationViewModel()
        {
            SelectedRaceIds = new List<byte>();
            EducationLevels = new List<SelectListItem>();
            Ethnicities = new List<SelectListItem>();
            Races = new List<SelectListItem>();
        }

        // Constructor for existing BirthFather with birth data (view/ edit existing father data)
        public FatherInformationViewModel(Person fatherPerson, Birth birth) : this()
        {
            if (birth != null)
            {
                BirthId = birth.BirthId;
                HasPaternityAcknowledgement = birth.PaternityAcknowledgementSigned;
            }

            if (fatherPerson != null)
            {
                BirthFatherId = fatherPerson.PersonId;
                FirstName = fatherPerson.FirstName;
                MiddleName = fatherPerson.MiddleName;
                LastName = fatherPerson.LastName;
                Suffix = fatherPerson.Suffix;
                DateOfBirth = fatherPerson.Dob;
                FatherBirthplace = fatherPerson.Birthplace;
                SSN = fatherPerson.Ssn;

                // Populate related entities if they exist
                EducationLevel = fatherPerson.EducationLevel;
                Ethnicity = fatherPerson.Ethnicity;

                // Handle race selections
                if (fatherPerson.PersonRaces != null)
                {
                    SelectedRaceIds = fatherPerson.PersonRaces.Select(r => r.RaceId).ToList();
                }
            }
        }

        public BirthFather ToEntity(BirthFather existingFather = null)
        {
            var father = existingFather ?? new BirthFather();

            father.FirstName = this.FirstName;
            father.MiddleName = this.MiddleName;
            father.LastName = this.LastName;
            father.Suffix = this.Suffix;
            //father.Dob = this.DateOfBirth;
            father.FatherBirthplace = this.FatherBirthplace;
            father.Ssn = this.SSN;
            
            if (this.EducationLevel?.EducationId != null && this.EducationLevel.EducationId > 0)
            {
                father.EducationId = (byte)this.EducationLevel.EducationId;
            }
            
            if (this.Ethnicity?.EthnicityId != null && this.Ethnicity.EthnicityId > 0)
            {
                father.EthnicityId = (byte)this.Ethnicity.EthnicityId;
            }

            return father;
        }

        public void UpdateBirthRecord(Birth birth, BirthFather birthFather = null)
        {
            // Always update paternity acknowledgement status
            birth.PaternityAcknowledgementSigned = this.HasPaternityAcknowledgement;

            // Only link father if paternity is acknowledged AND father entity exists
            if (this.HasPaternityAcknowledgement == true && birthFather != null)
            {
                birth.BirthFatherId = birthFather.BirthFatherId;
            }
            else
            {
                // Clear father reference if no paternity acknowledgement
                birth.BirthFatherId = null;
            }
        }
        
        // Update the BirthFather entity with race selections (requires repository access)
        public void UpdateFatherRaces(BirthFather birthFather, IEnumerable<Race> selectedRaces)
        {
            if (birthFather == null) return;

            // Initialize or clear existing races
            if (birthFather.Races == null)
            {
                birthFather.Races = new List<Race>();
            }
            else
            {
                birthFather.Races.Clear();
            }

            // Add selected races
            if (selectedRaces != null)
            {
                foreach (var race in selectedRaces)
                {
                    birthFather.Races.Add(race);
                }
            }
        }
    }
}