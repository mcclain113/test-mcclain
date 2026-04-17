using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Specialty
    {
        public Specialty()
        {
            Physicians = new HashSet<Physician>();
        }

        public int SpecialtyId { get; set; }

        [Required(ErrorMessage ="Please enter the Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Physician> Physicians { get; set; }
    }
}
