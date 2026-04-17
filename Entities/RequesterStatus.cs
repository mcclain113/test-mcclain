#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequesterStatus
    {
        [Required(ErrorMessage ="Please enter Requester Status Id")]
        public string RequesterStatusId { get; set; } = null!;

        [Required(ErrorMessage ="Please enter the Requester Status Name")]
        public string RequesterStatus1 { get; set; } = null!;

        public string? RequesterStatusDescription { get; set; }

        public virtual ICollection<Requester> Requesters { get; set; } = new List<Requester>();
    }
}
