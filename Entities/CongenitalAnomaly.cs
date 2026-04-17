using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class CongenitalAnomaly
    {
        public CongenitalAnomaly(){}
       
        public byte CongenitalAnomalyId { get; set; }

        [Required(ErrorMessage = "An Name for the Congenital Anomaly is required")]
        public string CongenitalAnomalyName { get; set; }

#nullable enable
        public string? CongenitalAnomalyDescription { get; set; }
#nullable disable
        public virtual ICollection<Newborn> Newborns { get; set; } = [];

    }
}