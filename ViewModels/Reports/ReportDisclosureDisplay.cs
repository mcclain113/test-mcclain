using System;
using Microsoft.AspNetCore.Mvc;

namespace IS_Proj_HIT.ViewModels.Reports
{
    public class ReportDisclosureDisplay
    {
        public string ReportName { get; set; }
        public string ReportMessage { get; set; } = null;
        public string ReportFile { get; set; }
        public string ErrorMessage { get; set; }
    }
}
