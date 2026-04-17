using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.ViewModels.PatientVm
{
    public class PatientMedicationViewModel
    {
        public PatientMedicationList PatientMedication { get; set; }
        public Patient CurrentPatient { get; set; }
        public List<SelectListItem> MedicationsAvailable { get; set; }
        public List<SelectListItem> MedicationFrequencies { get; set; }
        public List<SelectListItem> Providers { get; set; }
        public PatientMedicationViewModel() {}
        public PatientMedicationViewModel(string Mrn) {
            PatientMedication = new();
            PatientMedication.Mrn = Mrn;
        }
        public PatientMedicationViewModel(PatientMedicationList pml) {
            PatientMedication = pml;
        }
    }
}