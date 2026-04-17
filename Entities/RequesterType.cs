#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequesterType
    {
        public byte RequesterTypeId { get; set; }

        [Required(ErrorMessage ="Please enter the Requester Type Name")]
        public string RequesterType1 { get; set; } = null!;

        public string? RequesterTypeDescription { get; set; }

        public virtual ICollection<Requester> Requesters { get; set; } = new List<Requester>();
    }
}
