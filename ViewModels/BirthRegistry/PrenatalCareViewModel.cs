using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class PrenatalCareViewModel
    {
        // IDs
        public int? PrenatalId { get; set; }
        public int? BirthId { get; set; }

        // Date properties
        public DateTime? DateOfFirstVisit { get; set; }
        public DateTime? DateOfLastVisit { get; set; }
        public DateTime? DateLastPeriod { get; set; }
        public DateTime? DateOfLastLiveBirth { get; set; }
        public DateTime? DateOfLastOtherPregnancyOutcome { get; set; }

        // Measurements
        public byte? TotalNumberPrenatalVisits { get; set; }
        public decimal? MothersHeightInInches { get; set; }
        public decimal? PrepregnancyWeightInLbs { get; set; }
        public decimal? WeightAtDeliveryInLbs { get; set; }

        // Previous births
        public byte? PrevLiveBirthsAmountStillLiving { get; set; }
        public byte? PrevLiveBirthsAmountNotLiving { get; set; }
        public byte? NumberOfOtherPregnancyOutcomes { get; set; }

        // Procedures and treatments
        public bool InfertilityTreatmentReceived { get; set; } = false;
        public bool PreviousCesareanDelivery { get; set; } = false;
        public byte? NumberOfPriorCesareanBirths { get; set; }

        public List<string> SelectedInfections { get; set; } = new List<string>();

        public bool ExternalCephalicVersion { get; set; }

        public bool? IsExternalCephalicVersionSuccessful { get; set; }
        public bool CervicalCerclageProcedure { get; set; } = false;
        public bool TocolysisProcedure { get; set; } = false;

        // Smoking history
        public bool SmokingThreeMonthsBeforePregnancy { get; set; } = false;
        public bool SmokingFirstTrimester { get; set; } = false;
        public bool SmokingSecondTrimester { get; set; } = false;
        public bool SmokingThirdTrimester { get; set; } = false;

        // Other properties
        public bool? IsWicRecipient { get; set; }
        public string DescInfertilityTreatment { get; set; }
        public string DescPrenatalCare { get; set; }

        // Collections for dropdowns
        public List<SelectListItem> PregnancyInfections { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PregnancyRiskFactors { get; set; } = new List<SelectListItem>();

        // Selected values
        public List<byte> SelectedInfectionIds { get; set; } = new List<byte>();
        public List<byte> SelectedRiskFactorIds { get; set; } = new List<byte>();

        public bool NoPrenatalCare { get; set; }
        public bool NoRiskFactors { get; set; }
        public bool NoInfections { get; set; }
        public bool NoObstetricProcedures { get; set; }

        // Convenience properties
        public bool HasSmokingHistory => SmokingThreeMonthsBeforePregnancy || SmokingFirstTrimester || SmokingSecondTrimester || SmokingThirdTrimester;
        public bool HasPreviousBirths => (PrevLiveBirthsAmountStillLiving ?? 0) > 0 || (PrevLiveBirthsAmountNotLiving ?? 0) > 0;

        // Constructor for existing Prenatal entity
        public PrenatalCareViewModel(Prenatal prenatal) : this()
        {
            if (prenatal != null)
            {
                PrenatalId = prenatal.PrenatalId;
                BirthId = prenatal.BirthId;

                // Date conversions
                DateOfFirstVisit = prenatal.DateOfFirstVisit?.ToDateTime(TimeOnly.MinValue);
                DateOfLastVisit = prenatal.DateOfLastVisit?.ToDateTime(TimeOnly.MinValue);
                DateLastPeriod = prenatal.DateLastPeriod?.ToDateTime(TimeOnly.MinValue);
                DateOfLastLiveBirth = prenatal.DateOfLastLiveBirth?.ToDateTime(TimeOnly.MinValue);
                DateOfLastOtherPregnancyOutcome = prenatal.DateOfLastOtherPregnancyOutcome?.ToDateTime(TimeOnly.MinValue);

                // Measurements
                TotalNumberPrenatalVisits = prenatal.TotalNumberPrenatalVisits;
                MothersHeightInInches = prenatal.MothersHeightInInches;
                PrepregnancyWeightInLbs = prenatal.PrepregnancyWeightInLbs;
                WeightAtDeliveryInLbs = prenatal.WeightAtDeliveryInLbs;

                // Previous births
                PrevLiveBirthsAmountStillLiving = prenatal.PrevLiveBirthsAmountStillLiving;
                PrevLiveBirthsAmountNotLiving = prenatal.PrevLiveBirthsAmountNotLiving;
                NumberOfOtherPregnancyOutcomes = prenatal.NumberOfOtherPregnancyOutcomes;

                // Procedures
                InfertilityTreatmentReceived = prenatal.InfertilityTreatmentReceived ?? false;
                PreviousCesareanDelivery = prenatal.PreviousCesareanDelivery ?? false;
                NumberOfPriorCesareanBirths = prenatal.NumberOfPriorCesareanBirths;
                ExternalCephalicVersion = prenatal.ExternalCephalicVersion ?? false;
                IsExternalCephalicVersionSuccessful = prenatal.IsExternalCephalicVersionSuccessful;
                CervicalCerclageProcedure = prenatal.CervicalCerclageProcedure ?? false;
                TocolysisProcedure = prenatal.TocolysisProcedure ?? false;

                // Smoking history
                SmokingThreeMonthsBeforePregnancy = prenatal.SmokingThreeMonthsBeforePregnancy ?? false;
                SmokingFirstTrimester = prenatal.SmokingFirstTrimester ?? false;
                SmokingSecondTrimester = prenatal.SmokingSecondTrimester ?? false;
                SmokingThirdTrimester = prenatal.SmokingThirdTrimester ?? false;

                // Other properties
                IsWicRecipient = prenatal.IsWicrecipient;
                DescInfertilityTreatment = prenatal.DescInfertilityTreatment;
                DescPrenatalCare = prenatal.DescPrenatalCare;

                // Map selected infections and risk factors
                SelectedInfectionIds = prenatal.Infections?.Select(i => i.InfectionId).ToList() ?? new List<byte>();
                SelectedRiskFactorIds = prenatal.RiskFactors?.Select(rf => rf.RiskFactorId).ToList() ?? new List<byte>();

                NoPrenatalCare = !DateOfFirstVisit.HasValue && (!TotalNumberPrenatalVisits.HasValue || TotalNumberPrenatalVisits == 0);
                NoRiskFactors = !SelectedRiskFactorIds.Any() && 
                               !InfertilityTreatmentReceived && 
                               !PreviousCesareanDelivery && 
                               !HasSmokingHistory;
                NoInfections = !SelectedInfectionIds.Any();
                NoObstetricProcedures = !ExternalCephalicVersion && 
                                       !CervicalCerclageProcedure && 
                                       !TocolysisProcedure;
            }
        }

        public PrenatalCareViewModel() { }

        public Prenatal ToEntity(int birthId, Prenatal existingEntity = null)
        {
            var entity = existingEntity ?? new Prenatal();

            // Set required foreign key
            entity.BirthId = birthId;

            // Date conversions
            entity.DateOfFirstVisit = DateOfFirstVisit.HasValue ? DateOnly.FromDateTime(DateOfFirstVisit.Value) : null;
            entity.DateOfLastVisit = DateOfLastVisit.HasValue ? DateOnly.FromDateTime(DateOfLastVisit.Value) : null;
            entity.DateLastPeriod = DateLastPeriod.HasValue ? DateOnly.FromDateTime(DateLastPeriod.Value) : null;
            entity.DateOfLastLiveBirth = DateOfLastLiveBirth.HasValue ? DateOnly.FromDateTime(DateOfLastLiveBirth.Value) : null;
            entity.DateOfLastOtherPregnancyOutcome = DateOfLastOtherPregnancyOutcome.HasValue ? DateOnly.FromDateTime(DateOfLastOtherPregnancyOutcome.Value) : null;

            // Measurements
            entity.TotalNumberPrenatalVisits = TotalNumberPrenatalVisits;
            entity.MothersHeightInInches = MothersHeightInInches;
            entity.PrepregnancyWeightInLbs = PrepregnancyWeightInLbs;
            entity.WeightAtDeliveryInLbs = WeightAtDeliveryInLbs;

            // Previous births
            entity.PrevLiveBirthsAmountStillLiving = PrevLiveBirthsAmountStillLiving;
            entity.PrevLiveBirthsAmountNotLiving = PrevLiveBirthsAmountNotLiving;
            entity.NumberOfOtherPregnancyOutcomes = NumberOfOtherPregnancyOutcomes;

            // Procedures
            entity.InfertilityTreatmentReceived = InfertilityTreatmentReceived;
            entity.PreviousCesareanDelivery = PreviousCesareanDelivery;
            entity.NumberOfPriorCesareanBirths = NumberOfPriorCesareanBirths;
            entity.ExternalCephalicVersion = ExternalCephalicVersion;
            entity.IsExternalCephalicVersionSuccessful = IsExternalCephalicVersionSuccessful;
            entity.CervicalCerclageProcedure = CervicalCerclageProcedure;
            entity.TocolysisProcedure = TocolysisProcedure;

            // Smoking history
            entity.SmokingThreeMonthsBeforePregnancy = SmokingThreeMonthsBeforePregnancy;
            entity.SmokingFirstTrimester = SmokingFirstTrimester;
            entity.SmokingSecondTrimester = SmokingSecondTrimester;
            entity.SmokingThirdTrimester = SmokingThirdTrimester;

            // Other properties
            entity.IsWicrecipient = IsWicRecipient;
            entity.DescInfertilityTreatment = DescInfertilityTreatment;
            entity.DescPrenatalCare = DescPrenatalCare;

            // Note: Many-to-many collections handled in controller
            return entity;
        }
    }
}
