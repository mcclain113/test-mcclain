using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Entities
{
    public partial class AbnormalCondition
    {
        public AbnormalCondition() { }

        public byte AbnormalConditionId { get; set; }

        [Required(ErrorMessage = "An Name for the Abnormal Condition is required")]
        public string AbnormalConditionName { get; set; }

#nullable enable
        public string? AbnormalConditionDescription { get; set; }
#nullable disable

        public virtual ICollection<Newborn> Newborns { get; set; } = [];

    }
}