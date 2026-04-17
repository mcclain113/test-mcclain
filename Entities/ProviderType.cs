using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ProviderType
    {
        public ProviderType()
        {
            Physicians = new HashSet<Physician>();
        }

        public int ProviderTypeId { get; set; }

        [Required(ErrorMessage ="Please enter the Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
        

        public virtual ICollection<Physician> Physicians { get; set; }
    }
}
