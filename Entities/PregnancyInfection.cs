using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class PregnancyInfection
    {
        public PregnancyInfection() { }

        public byte InfectionId { get; set; }

        [Required(ErrorMessage = "An Name for the Pregnancy Infection is required")]
        public string InfectionName { get; set; }

#nullable enable
        public string? InfectionDescription { get; set; }
#nullable disable

        public virtual ICollection<Prenatal> Prenatals { get; set; } = [];
    }
}