using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class MaternalMorbidity
    {
        public MaternalMorbidity() { }

        public byte MaternalMorbidityId { get; set; }

        [Required(ErrorMessage = "An Name for the Maternal Morbidity is required")]
        public string MaternalMorbidityName { get; set; }

#nullable enable
        public string? MaternalMorbidityDescription { get; set; }
#nullable disable

        public virtual ICollection<LaborAndDelivery> LaborAndDeliveries { get; set; } = [];

    }
}