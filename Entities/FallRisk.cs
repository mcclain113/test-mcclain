using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class FallRisk
    {
        public FallRisk()
        {
            PatientFallRisks = new HashSet<PatientFallRisk>();
        }

        public byte FallRiskId { get; set; }

        [Required(ErrorMessage = "Please enter the Name for this Fall Risk")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select Yes or No")]
        public bool IsActive { get; set; }

        public virtual ICollection<PatientFallRisk> PatientFallRisks { get; set; }
    }
}
