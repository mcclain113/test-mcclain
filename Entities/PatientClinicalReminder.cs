using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientClinicalReminder
    {
        public long PatientClinicalReminderId { get; set; }
        public long PatientAlertId { get; set; }
        public short ClinicalReminderId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ClinicalReminder ClinicalReminder { get; set; }
        public virtual PatientAlert PatientAlert { get; set; }
    }
}
