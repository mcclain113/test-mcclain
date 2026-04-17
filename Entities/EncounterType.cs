using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class EncounterType
    {
        public EncounterType()
        {
            Encounters = new HashSet<Encounter>();
        }

        public int EncounterTypeId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Encounter> Encounters { get; set; }
    }
}
