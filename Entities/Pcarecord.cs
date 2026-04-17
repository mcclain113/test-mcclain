using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Pcarecord
    {
        public Pcarecord()
        {
            CareSystemAssessments = new HashSet<CareSystemAssessment>();
            Pcacomments = new HashSet<Pcacomment>();
            PcapainAssessments = new HashSet<PcapainAssessment>();
        }

        public int Pcaid { get; set; }
        public long EncounterId { get; set; }
        public byte? PainLevelGoal { get; set; }
        public int? PainScaleTypeId { get; set; }
        public decimal? Temperature { get; set; }
        public int? TempRouteTypeId { get; set; }
        public byte? Pulse { get; set; }
        public int? PulseRouteTypeId { get; set; }
        public byte? Respiration { get; set; }
        public short? SystolicBloodPressure { get; set; }
        public short? DiastolicBloodPressure { get; set; }
        public byte? BloodPressureRouteTypeId { get; set; }
        [Range(1, 100)]
        public byte? PulseOximetry { get; set; }
        public int? O2deliveryTypeId { get; set; }
        public string OxygenFlow { get; set; }
        public decimal? PercentOxygenDelivered { get; set; }
        public decimal? Weight { get; set; }
        public string WeightUnits { get; set; }
        public decimal? Height { get; set; }
        public string HeightUnits { get; set; }
        public decimal? BodyMassIndex { get; set; }
        public byte? BmimethodId { get; set; }
        public decimal? HeadCircumference { get; set; }
        public string HeadCircumferenceUnits { get; set; }
        public DateTime? DateVitalsAdded { get; set; }
        public DateTime LastModified { get; set; }
        public int? EditedBy { get; set; }

        public virtual BloodPressureRouteType BloodPressureRouteType { get; set; }
        public virtual Bmimethod Bmimethod { get; set; }
        public virtual Encounter Encounter { get; set; }
        public virtual O2deliveryType O2deliveryType { get; set; }
        public virtual PainScaleType PainScaleType { get; set; }
        public virtual PulseRouteType PulseRouteType { get; set; }
        public virtual TempRouteType TempRouteType { get; set; }
        public virtual ICollection<CareSystemAssessment> CareSystemAssessments { get; set; }
        public virtual ICollection<Pcacomment> Pcacomments { get; set; }
        public virtual ICollection<PcapainAssessment> PcapainAssessments { get; set; }
    }
}
