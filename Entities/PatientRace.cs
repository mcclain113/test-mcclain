using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientRace
    {
        public byte RaceId { get; set; }
        public string Mrn { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Patient MrnNavigation { get; set; }
        public virtual Race Race { get; set; }
    }
}
