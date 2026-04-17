using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class NewbornViewModel
    {
        public string SearchTerm { get; set; }
        public int? NewbornId { get; set; }
        public int? BirthId { get; set; }
        
        public int? PersonId { get; set; }
        public bool IsExistingPatient { get; set; } = false;
        public DateTime? DateAndTimeOfBirth { get; set; }
        public decimal? BirthWeightInLbs { get; set; }
        public byte? GestationalAgeEstimateInWeeks { get; set; }
        public byte? SexId { get; set; }
        public byte? ApgarScoreAt1Minute { get; set; }
        public byte? ApgarScoreAt5Minutes { get; set; }
        public byte? ApgarScoreAt10Minutes { get; set; }
        public bool IsSingleBirth { get; set; } = true;
        public byte? BirthOrder { get; set; }
        public bool InfantTransferredWithin24Hours { get; set; } = false;
        public string NameOfFacilityTransferredTo { get; set; }
        public bool IsInfantStillLiving { get; set; } = true;
        public bool IsInfantBeingBreastfed { get; set; } = false;
        public bool? SsnrequestedForChild { get; set; }
        public string NewbornMrn { get; set; }
        public string Comments { get; set; }
        public bool IsTimeUnknown { get; set; } = false;
        public byte? Plurality { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public List<SelectListItem> SexOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AbnormalConditions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CongenitalAnomalies { get; set; } = new List<SelectListItem>();
        public List<byte> SelectedAbnormalConditionIds { get; set; } = new List<byte>();
        public List<byte> SelectedCongenitalAnomalyIds { get; set; } = new List<byte>();

        public bool NoAbnormalities { get; set; }
        public bool NoCongenitalAnomalies { get; set; }
        public bool HasPatientRecord => !string.IsNullOrWhiteSpace(NewbornMrn);
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim().Replace("  ", " ");

        public NewbornViewModel() { }

        public NewbornViewModel(Newborn newborn) : this()
        {
            if (newborn != null)
            {
                NewbornId = newborn.NewbornId;
                BirthId = newborn.BirthId;
                NewbornMrn = newborn.NewbornMrn;
                PersonId = newborn.PersonId; 
                
                IsExistingPatient = !string.IsNullOrWhiteSpace(newborn.NewbornMrn);
                
                DateAndTimeOfBirth = newborn.DateAndTimeOfBirth;
                BirthWeightInLbs = newborn.BirthWeightInLbs;
                GestationalAgeEstimateInWeeks = newborn.GestationalAgeEstimateInWeeks;
                SexId = newborn.SexId;
                ApgarScoreAt1Minute = newborn.ApgarScoreAt1Minute;
                ApgarScoreAt5Minutes = newborn.ApgarScoreAt5Minutes;
                ApgarScoreAt10Minutes = newborn.ApgarScoreAt10Minutes;
                IsSingleBirth = newborn.IsSingleBirth ?? true;
                BirthOrder = newborn.BirthOrder;
                InfantTransferredWithin24Hours = newborn.InfantTransferredWithin24Hours ?? false;
                NameOfFacilityTransferredTo = newborn.NameOfFacilityTransferredTo;
                IsInfantStillLiving = newborn.IsInfantStillLiving ?? true;
                IsInfantBeingBreastfed = newborn.IsInfantBeingBreastfed ?? false;
                SsnrequestedForChild = newborn.SsnrequestedForChild;
                Comments = newborn.Comments;

                if (newborn.NewbornMrnNavigation != null)
                {
                    FirstName = newborn.NewbornMrnNavigation.FirstName;
                    LastName = newborn.NewbornMrnNavigation.LastName;
                    MiddleName = newborn.NewbornMrnNavigation.MiddleName;
                }

                if (newborn.Birth != null)
                {
                    Plurality = newborn.Birth.Plurality;
                }

                IsTimeUnknown = !DateAndTimeOfBirth.HasValue;

                SelectedAbnormalConditionIds = newborn.AbnormalConditions?.Select(ac => ac.AbnormalConditionId).ToList() ?? new List<byte>();
                SelectedCongenitalAnomalyIds = newborn.CongenitalAnomalies?.Select(ca => ca.CongenitalAnomalyId).ToList() ?? new List<byte>();
                
                NoAbnormalities = !SelectedAbnormalConditionIds.Any();
                NoCongenitalAnomalies = !SelectedCongenitalAnomalyIds.Any();
            }
        }
        public Newborn ToEntity(Newborn existingEntity = null)
        {
            var entity = existingEntity ?? new Newborn();
            
            if (existingEntity != null && NewbornId.HasValue)
                entity.NewbornId = NewbornId.Value;
            
            if (BirthId.HasValue)
                entity.BirthId = BirthId.Value;
            
            entity.NewbornMrn = NewbornMrn;
            if (PersonId.HasValue)
                entity.PersonId = PersonId.Value;
            entity.DateAndTimeOfBirth = IsTimeUnknown ? null : DateAndTimeOfBirth;
            entity.BirthWeightInLbs = BirthWeightInLbs;
            entity.GestationalAgeEstimateInWeeks = GestationalAgeEstimateInWeeks;
            entity.SexId = SexId;
            entity.ApgarScoreAt1Minute = ApgarScoreAt1Minute;
            entity.ApgarScoreAt5Minutes = ApgarScoreAt5Minutes;
            entity.ApgarScoreAt10Minutes = ApgarScoreAt10Minutes;
            entity.IsSingleBirth = IsSingleBirth;
            entity.BirthOrder = BirthOrder;
            entity.InfantTransferredWithin24Hours = InfantTransferredWithin24Hours;
            entity.NameOfFacilityTransferredTo = NameOfFacilityTransferredTo;
            entity.IsInfantStillLiving = IsInfantStillLiving;
            entity.IsInfantBeingBreastfed = IsInfantBeingBreastfed;
            entity.SsnrequestedForChild = SsnrequestedForChild;
            entity.Comments = Comments;

            return entity;
        }

        public void UpdateNewbornConditions(
            Newborn newborn, 
            IEnumerable<AbnormalCondition> selectedAbnormalConditions,
            IEnumerable<CongenitalAnomaly> selectedCongenitalAnomalies)
        {
            if (newborn == null) return;

            // Update Abnormal Conditions
            if (newborn.AbnormalConditions == null)
                newborn.AbnormalConditions = new List<AbnormalCondition>();
            else
                newborn.AbnormalConditions.Clear();

            if (!NoAbnormalities && selectedAbnormalConditions != null)
            {
                foreach (var condition in selectedAbnormalConditions)
                {
                    newborn.AbnormalConditions.Add(condition);
                }
            }

            // Update Congenital Anomalies
            if (newborn.CongenitalAnomalies == null)
                newborn.CongenitalAnomalies = new List<CongenitalAnomaly>();
            else
                newborn.CongenitalAnomalies.Clear();

            if (!NoCongenitalAnomalies && selectedCongenitalAnomalies != null)
            {
                foreach (var anomaly in selectedCongenitalAnomalies)
                {
                    newborn.CongenitalAnomalies.Add(anomaly);
                }
            }
        }
    }
}