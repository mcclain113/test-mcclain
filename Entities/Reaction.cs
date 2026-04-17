using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Reaction
    {
        public Reaction()
        {
            PatientAllergies = new HashSet<PatientAllergy>();
        }

        public int ReactionId { get; set; }

        [Required(ErrorMessage = "Please enter the Name for this Reaction")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
#nullable enable
        [Required(ErrorMessage = "Please select Yes or No")]
        public bool? AlertRequired { get; set; }
    #nullable disable

        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
    }
}
