using System;

namespace IS_Proj_HIT.ViewModels.Alert
{
    public class AlertEditViewModel
    {
        public long PatientAlertId { get; set; }
        public string Mrn { get; set; } = string.Empty;
        public string AlertTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModified { get; set; }
#nullable enable
        public DateTime? EndDate { get; set; }

        public string? Comments { get; set; }

        public string? AllergenName { get; set; }
        public string? ReactionName { get; set; }
        public string? FallRiskName { get; set; }
        public string? RestrictionName { get; set; }
    }
}