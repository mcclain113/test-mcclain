using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PointOfOrigin
    {
        public PointOfOrigin()
        {
            Encounters = new HashSet<Encounter>();
        }

        public int PointOfOriginId { get; set; }
        public string WiPopCode { get; set; }
        public string Description { get; set; }
        public string Explaination { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Encounter> Encounters { get; set; }
    }
}
