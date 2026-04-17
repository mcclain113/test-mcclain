using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.MedicationDataEntry
{
    public class MedicationViewModel
    {
        public MedicationViewModel() {
        }
        public MedicationViewModel(Medication medication) {
            Medication = medication;
            GenericName = medication.MedicationGenericName;
            BrandName = medication.MedicationBrandName;
            DosageForm = medication.MedicationDosageForm;
            DeliveryRoute = medication.MedicationDeliveryRoute;
        }
        public Medication Medication { get; set; }
        public MedicationGenericName GenericName { get; set; }
        public MedicationBrandName BrandName { get; set; }
        public MedicationDosageForm DosageForm { get; set; }
        public MedicationDeliveryRoute DeliveryRoute { get; set; }
    }
}
