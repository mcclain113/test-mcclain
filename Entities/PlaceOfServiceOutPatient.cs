using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PlaceOfServiceOutPatient
    {
        public PlaceOfServiceOutPatient()
        {
            Encounters = new HashSet<Encounter>();
        }

        public int PlaceOfServiceId { get; set; }
        public byte WiPopCode { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Encounter> Encounters { get; set; }
    }
}
