using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class VisitType
    {
        public VisitType()
        {
            AdmitOrders = new HashSet<AdmitOrder>();
        }

        public short VisitTypeId { get; set; }

        [Required(ErrorMessage ="Please enter the Name")]
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<AdmitOrder> AdmitOrders { get; set; }
    }
}
