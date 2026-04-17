using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.ViewModels.Alert
{
    public class AlertListViewModel
    {
        public string Mrn { get; set; } = string.Empty;
        public IEnumerable<AlertListItemViewModel> ActiveAlerts { get; set; } = Array.Empty<AlertListItemViewModel>();
        public IEnumerable<AlertListItemViewModel> InactiveAlerts { get; set; } = Array.Empty<AlertListItemViewModel>();
    }
}