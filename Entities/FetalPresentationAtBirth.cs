using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class FetalPresentationAtBirth
    {
        public FetalPresentationAtBirth() { }

        public byte FetalPresentationAtBirthId { get; set; }

        [Required(ErrorMessage = "An Name for the Fetal Presentation At Birth is required")]
        public string FetalPresentationName { get; set; }

#nullable enable
        public string? FetalPresentationDescription { get; set; }
#nullable disable

        public virtual ICollection<LaborAndDelivery> LaborAndDeliveries { get; set; } = [];

    }
}