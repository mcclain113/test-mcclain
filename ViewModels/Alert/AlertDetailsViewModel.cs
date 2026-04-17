using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.ViewModels.Alert
{
    public class AlertDetailsViewModel
    {
        public long PatientAlertId { get; set; }
        public string Mrn { get; set; } = string.Empty;
        public string AlertTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
#nullable enable
        public string? Comments { get; set; }
        public string? AlertTypeDescription { get; set; }
        

        public string? AllergenName { get; set; }
        public string? ReactionName { get; set; }
        public string? FallRiskName { get; set; }
        public string? RestrictionName { get; set; }
    }
}
