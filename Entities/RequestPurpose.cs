#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequestPurpose
    {
        public byte PurposeId { get; set; }

        [Required(ErrorMessage ="Please enter the Request Purpose Name")]
        public string RequestPurpose1 { get; set; } = null!;

        public string? PurposeDescription { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
