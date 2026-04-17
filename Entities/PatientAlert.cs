using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientAlert
    {
        public PatientAlert()
        {
            EncounterAlerts = new HashSet<EncounterAlert>();
            PatientAdvancedDirectives = new HashSet<PatientAdvancedDirective>();
            PatientAllergies = new HashSet<PatientAllergy>();
            PatientClinicalReminders = new HashSet<PatientClinicalReminder>();
            PatientFallRisks = new HashSet<PatientFallRisk>();
            PatientRestrictions = new HashSet<PatientRestriction>();
        }

        public long PatientAlertId { get; set; }
        public int? AlertTypeId { get; set; }
        public string Mrn { get; set; }
        public DateTime LastModified { get; set; }
        public bool? IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }

        public virtual AlertType AlertType { get; set; }
        public virtual Patient MrnNavigation { get; set; }
        public virtual ICollection<EncounterAlert> EncounterAlerts { get; set; }
        public virtual ICollection<PatientAdvancedDirective> PatientAdvancedDirectives { get; set; }
        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
        public virtual ICollection<PatientClinicalReminder> PatientClinicalReminders { get; set; }
        public virtual ICollection<PatientFallRisk> PatientFallRisks { get; set; }
        public virtual ICollection<PatientRestriction> PatientRestrictions { get; set; }
    }
}
