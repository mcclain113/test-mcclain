using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class BirthPlaceType
    {
        public BirthPlaceType(){}
        
        public byte BirthPlaceTypeId { get; set; }

        [Required(ErrorMessage = "An Name for the Birth Place Type is required")]
        public string BirthPlaceTypeName { get; set; }

#nullable enable
        public string? BirthPlaceDescription { get; set; }
#nullable disable
        public virtual ICollection<Birth> Births { get; set; } = [];

    }
}