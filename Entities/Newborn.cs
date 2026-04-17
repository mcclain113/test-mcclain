using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IS_Proj_HIT.Entities
{
    public partial class Newborn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewbornId { get; set; }
        public int? BirthId { get; set; }
        public DateTime? DateAndTimeOfBirth { get; set; }
        public decimal? BirthWeightInLbs { get; set; }
        public byte? GestationalAgeEstimateInWeeks { get; set; }
        public byte? SexId { get; set; }
        public byte? ApgarScoreAt1Minute { get; set; }
        public byte? ApgarScoreAt5Minutes { get; set; }
        public byte? ApgarScoreAt10Minutes { get; set; }
        public bool? IsSingleBirth { get; set; }
        public byte? BirthOrder { get; set; }
        public bool? InfantTransferredWithin24Hours { get; set; }
        public bool? IsInfantStillLiving { get; set; }
        public bool? IsInfantBeingBreastfed { get; set; }
        public bool? SsnrequestedForChild { get; set; }
#nullable enable
        public string? NewbornMrn { get; set; }
        
        public int? PersonId { get; set; }
        public virtual Person? Person { get; set; }
        public string? NameOfFacilityTransferredTo { get; set; }
        public string? Comments { get; set; }
        public virtual Birth? Birth { get; set; }
        public virtual Patient? NewbornMrnNavigation { get; set; }
        public virtual Sex? Sex { get; set; }
#nullable disable
        public virtual ICollection<LaborAndDelivery> LaborAndDeliveries { get; set; } = [];
        public virtual ICollection<AbnormalCondition> AbnormalConditions { get; set; } = [];
        public virtual ICollection<CongenitalAnomaly> CongenitalAnomalies { get; set; } = [];
       
    }
}