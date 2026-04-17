#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class RequestReleaseFormat
    {
        public byte ReleaseFormatId { get; set; }

        [Required(ErrorMessage ="Please enter the Release Format Name")]
        public string RequestReleaseFormat1 { get; set; } = null!;

        public string? ReleaseFormatDescription { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
