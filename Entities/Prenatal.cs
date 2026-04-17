using System;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Entities
{

    public partial class Prenatal
    {
        public int PrenatalId { get; set; }

        public int? BirthId { get; set; }

        public bool? InfertilityTreatmentReceived { get; set; }

        public DateOnly? DateOfFirstVisit { get; set; }

        public DateOnly? DateOfLastVisit { get; set; }

        public byte? TotalNumberPrenatalVisits { get; set; }

        public decimal? MothersHeightInInches { get; set; }

        public decimal? PrepregnancyWeightInLbs { get; set; }

        public decimal? WeightAtDeliveryInLbs { get; set; }

        public DateOnly? DateLastPeriod { get; set; }

        public byte? PrevLiveBirthsAmountStillLiving { get; set; }

        public byte? PrevLiveBirthsAmountNotLiving { get; set; }

        public DateOnly? DateOfLastLiveBirth { get; set; }

        public DateOnly? DateOfLastOtherPregnancyOutcome { get; set; }

        public byte? NumberOfOtherPregnancyOutcomes { get; set; }

        public bool? ExternalCephalicVersion { get; set; }

        public bool? IsExternalCephalicVersionSuccessful { get; set; }

        public bool? PreviousCesareanDelivery { get; set; }

        public byte? NumberOfPriorCesareanBirths { get; set; }

        public bool? CervicalCerclageProcedure { get; set; }

        public bool? TocolysisProcedure { get; set; }

        public bool? SmokingThreeMonthsBeforePregnancy { get; set; }

        public bool? SmokingFirstTrimester { get; set; }

        public bool? SmokingSecondTrimester { get; set; }

        public bool? SmokingThirdTrimester { get; set; }

        public bool? IsWicrecipient { get; set; }

#nullable enable
        public string? DescInfertilityTreatment { get; set; }

        public string? DescPrenatalCare { get; set; }

        public virtual Birth? Birth { get; set; }
#nullable disable

        public virtual ICollection<PregnancyInfection> Infections { get; set; } = new List<PregnancyInfection>();

        public virtual ICollection<PregnancyRiskFactor> RiskFactors { get; set; } = new List<PregnancyRiskFactor>();
    }
}