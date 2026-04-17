using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Medication
    {
        //Icollection  if it has ID column then it get virtual 'medication', otherwise Icollection
        public virtual ICollection<PatientMedicationList> PatientMedicationLists { get; set; }
        public int MedicationId {get; set;}
        public string Ndc {get; set;}
        public int BrandNameId {get; set;}
        public int GenericNameId {get; set;}
        public string ActiveStrength  {get; set;}
        public string ActiveIngredientUnits {get; set;}
        public string ActiveStrengthUnits {get {return ActiveStrength + " " + ActiveIngredientUnits;}}
        public short DosageFormId {get; set;}
        public short DeliveryRouteId {get; set;}
        public bool IsActive {get; set;} 
        public DateTime ModifiedDate {get; set;}

        public virtual MedicationDeliveryRoute MedicationDeliveryRoute {get; set;}
        public virtual MedicationBrandName MedicationBrandName {get; set;}
        public virtual MedicationDosageForm MedicationDosageForm {get; set;}
        public virtual MedicationGenericName MedicationGenericName {get; set;}

        // public virtual PatientMedicationLists PatientMedicationList {get; set;}

    }
}