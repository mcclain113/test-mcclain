#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class DocumentRequested
    {
        public int DocumentRequestedId { get; set; }

        [Required(ErrorMessage ="Please enter the Document Name")]
        public string DocumentRequested1 { get; set; } = null!;

        public string? DocumentRequestedDescription { get; set; }

        public virtual ICollection<RequestedItem> RequestedItems { get; set; } = new List<RequestedItem>();
    }
}
