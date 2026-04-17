using System;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.PatientVm
{
    public class PatientMedicationListViewModel
    {
        public string Mrn { get; set; } = string.Empty;
        public IEnumerable<PatientMedicationList> ActiveMeds { get; set; } = Array.Empty<PatientMedicationList>();
        public IEnumerable<PatientMedicationList> InactiveMeds { get; set; } = Array.Empty<PatientMedicationList>();
    }

}