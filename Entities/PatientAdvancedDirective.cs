using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientAdvancedDirective
    {
        public long PatientAdvancedDirectiveId { get; set; }
        public long PatientAlertId { get; set; }
        public short AdvancedDirectiveId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual AdvancedDirective AdvancedDirective { get; set; }
        public virtual PatientAlert PatientAlert { get; set; }
    }
}
