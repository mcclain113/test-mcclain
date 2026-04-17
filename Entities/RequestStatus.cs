#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequestStatus
    {
        [Required(ErrorMessage ="Please enter the Release Status Id")]
        public string RequestStatusId { get; set; } = null!;

        [Required(ErrorMessage ="Please enter the Release Status Name")]
        public string RequestStatus1 { get; set; } = null!;

        public string? StatusDescription { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
