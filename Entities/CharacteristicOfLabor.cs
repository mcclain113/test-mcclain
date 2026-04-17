using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class CharacteristicOfLabor
    {
        public CharacteristicOfLabor(){}
        
        public byte CharacteristicId { get; set; }

        [Required(ErrorMessage = "An Name for the Characteristic of Labor is required")]
        public string CharacteristicName { get; set; }

#nullable enable
        public string? CharacteristicDescription { get; set; }
#nullable disable
        public virtual ICollection<LaborAndDelivery> LaborAndDeliveries { get; set; } = [];
    }
}