using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Allergen
    {
        public Allergen()
        {
            PatientAllergies = new HashSet<PatientAllergy>();
        }

        public int AllergenId { get; set; }

        [Required(ErrorMessage = "Please enter the Name for this Allergen")]
        public string AllergenName { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
    }
}
