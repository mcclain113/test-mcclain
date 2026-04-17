using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class ChargeDefinition
    {
        public ChargeDefinition()
        {
            OrderInfos = new HashSet<OrderInfo>();
        }

        public int ChargeID { get; set; }

        [Required(ErrorMessage = "Please enter a Short Description")]
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ServiceCode { get; set; } = "";

        [Required(ErrorMessage = "Please select a Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Please select a Revenue Code")]
        public string RevenueCodeID { get; set; }

        [Required(ErrorMessage = "Please enter a charge amount")]
        public decimal ChargeAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DateActivated { get; set; }
        public DateTime? DateDeactivated { get; set; } 

        public virtual Department Department { get; set; }
        public virtual RevenueCode RevenueCode { get; set; }

        public virtual ICollection<OrderInfo> OrderInfos {get; set;}
    }
}