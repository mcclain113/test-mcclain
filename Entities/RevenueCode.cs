using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public partial class RevenueCode
    {
        public RevenueCode()
        {
            ChargeDefinitions = new HashSet<ChargeDefinition>();
        }

        [Required(ErrorMessage ="Please enter a Revenue Code Id of 4 digits beginning with 0")]
        public string RevenueCodeID { get; set; }

        [Required(ErrorMessage ="Please enter a Short Description")]
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public virtual ICollection<ChargeDefinition>ChargeDefinitions{get; set;}
    }
}