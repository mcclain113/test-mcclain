using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.MedicationDataEntry
{
    public class MedicationListViewModel
    {
        public MedicationListViewModel() {
        }
        public MedicationListViewModel(List<Medication> medications) {
            Medications = medications;
        }
        public List<Medication> Medications { get; set; }
        public int PageNumber { get; set; }
        public string BrandSearch { get; set; }
        public string GenericSearch { get; set; }
    }
}
