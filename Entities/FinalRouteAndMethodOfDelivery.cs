using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class FinalRouteAndMethodOfDelivery
    {
        public FinalRouteAndMethodOfDelivery() { }

        public byte FinalRouteAndMethodId { get; set; }

        [Required(ErrorMessage = "An Name for the Final Route And Method Of Delivery is required")]
        public string FinalRouteAndMethodName { get; set; }

#nullable enable
        public string? FinalRouteAndMethodDescription { get; set; }
#nullable disable

        public virtual ICollection<LaborAndDelivery> LaborAndDeliveries { get; set; } = [];

    }
}