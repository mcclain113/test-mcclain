#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequestPriority
    {
        [Required(ErrorMessage ="Please enter Priority Id")]
        public string PriorityId { get; set; } = null!;

        [Required(ErrorMessage ="Please enter the Request Priority Name")]
        public string RequestPriority1 { get; set; } = null!;

        public string? PriorityDescription { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
