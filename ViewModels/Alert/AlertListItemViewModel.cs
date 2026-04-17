using System;

namespace IS_Proj_HIT.ViewModels.Alert
{
    public class AlertListItemViewModel
    {
        public long PatientAlertId { get; set; }
        public string AlertTypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EndDate { get; set; }
    }
}