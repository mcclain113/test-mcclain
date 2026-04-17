using System;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Entities
{

    public partial class LaborAndDelivery
    {
        public int LaborAndDeliveryId { get; set; }

        public int NewbornId { get; set; }

        public byte? FetalPresentationAtBirthId { get; set; }

        public byte? FinalRouteAndMethodId { get; set; }
        public bool? TrialOfLaborBeforeCesarean { get; set; }
        
#nullable enable
        public string? Comments { get; set; }

        public virtual FetalPresentationAtBirth? FetalPresentationAtBirth { get; set; }

        public virtual FinalRouteAndMethodOfDelivery? FinalRouteAndMethod { get; set; }
#nullable disable

        public virtual Newborn Newborn { get; set; } = null!;

        public virtual ICollection<CharacteristicOfLabor> Characteristics { get; set; } = [];

        public virtual ICollection<MaternalMorbidity> MaternalMorbidities { get; set; } = new List<MaternalMorbidity>();

        public virtual ICollection<OnsetOfLabor> OnsetOfLabors { get; set; } = new List<OnsetOfLabor>();
    }
}