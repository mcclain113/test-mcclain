using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Discharge
    {
        public Discharge()
        {
            Encounters = new HashSet<Encounter>();
        }

        public int DischargeId { get; set; }
        public string WiPopCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Encounter> Encounters { get; set; }
    }
}
