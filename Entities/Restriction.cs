using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Restriction
    {
        public Restriction()
        {
            PatientRestrictions = new HashSet<PatientRestriction>();
        }

        public short RestrictionId { get; set; }

        [Required(ErrorMessage = "Please enter the Name for this Restriction")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select Yes or No")]
        public bool IsActive { get; set; }

        public virtual ICollection<PatientRestriction> PatientRestrictions { get; set; }
    }
}
