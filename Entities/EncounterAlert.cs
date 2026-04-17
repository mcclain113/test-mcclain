using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class EncounterAlert
    {
        public long EncounterId { get; set; }
        public long PatientAlertId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Encounter Encounter { get; set; }
        public virtual PatientAlert PatientAlert { get; set; }
    }
}
