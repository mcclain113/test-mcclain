using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Priority
    {
        public Priority()
        {
            OrderInfos = new HashSet<OrderInfo>();
        }

        public int PriorityId { get; set; }
        
        [Required(ErrorMessage ="Please enter the Name")]
        public string PriorityName { get; set; }

        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
    }
}
