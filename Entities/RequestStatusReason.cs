#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequestStatusReason
    {
        public byte RequestStatusReasonId { get; set; }

        [Required(ErrorMessage ="Please enter the Release Status Reason Name")]
        public string RequestStatusReason1 { get; set; } = null!;

        public string? StatusReasonDescription { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}