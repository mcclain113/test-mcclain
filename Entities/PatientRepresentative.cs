#nullable enable
using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public class PatientRepresentative
    {
        public int RepresentativeId { get; set; }

        public string RepresentativeFirstName { get; set; } = null!;

        public string RepresentativeLastName { get; set; } = null!;

        public string? RepresentativePhoneNumber { get; set; }

        public string? RepresentativeEmailAddress { get; set; }

        public string? Comments { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
