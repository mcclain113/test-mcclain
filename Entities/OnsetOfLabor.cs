using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class OnsetOfLabor
    {
        public OnsetOfLabor(){}
       
        public byte OnsetOfLaborId { get; set; }

        [Required(ErrorMessage = "An Name for the Onset Of Labor is required")]
        public string OnsetOfLaborName { get; set; }

#nullable enable
        public string? OnsetOfLaborDescription { get; set; }
#nullable disable
        public virtual ICollection<LaborAndDelivery> LaborAndDeliveries { get; set; } = [];

    }
}