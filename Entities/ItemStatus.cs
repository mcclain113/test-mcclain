#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class ItemStatus
    {
        [Required(ErrorMessage ="Please enter the Item Status Id")]
        public string ItemStatusId { get; set; } = null!;

        [Required(ErrorMessage ="Please enter the Item Status Name")]
        public string ItemStatus1 { get; set; } = null!;

        public string? ItemStatusDescription { get; set; }

        public virtual ICollection<RequestedItem> RequestedItems { get; set; } = new List<RequestedItem>();
    }
}
