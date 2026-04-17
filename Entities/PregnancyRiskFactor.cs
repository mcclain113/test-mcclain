using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class PregnancyRiskFactor
    {
        public PregnancyRiskFactor(){}
        
        public byte RiskFactorId { get; set; }

        [Required(ErrorMessage = "An Name for the Pregnancy Risk Factor is required")]
        public string RiskFactorName { get; set; }

#nullable enable
        public string? RiskFactorDescription { get; set; }
#nullable disable
        public virtual ICollection<Prenatal> Prenatals { get; set; } = [];

    }
}