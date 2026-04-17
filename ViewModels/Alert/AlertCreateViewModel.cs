using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.Alert
{
    public class AlertCreateViewModel
    {
        // from PatientAlerts
        public long PatientAlertId { get; set; }
        public int? AlertTypeId { get; set; }
        public string Mrn { get; set; }
        public DateTime LastModified { get; set; }
        public bool? IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }

        // from PatientRestrictions
        public short RestrictionTypeId { get; set; }

        // from PatientFallRisks
        public long PatientFallRiskId { get; set; }
        public byte FallRiskId { get; set; }

        // from Restrictions
        public short RestrictionId { get; set; }
        public string Name { get; set; }
        public PatientFallRisk PatientFallRisk { get; set; }

        public IEnumerable<FallRisk> FallRisks { get; set; }

    }
}