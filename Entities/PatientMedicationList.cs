using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientMedicationList
    {
        public long PatientMedicationListID { get; set; }

        public string Mrn { get; set; }

        // the next three properties were changed to nullable but then annotated as Required with an ErrorMessage which maintains a Required status needed by the database.  This is used in server-side validation if the jquery-validate-unobtrusive validation fails to load properly. Because of the data types, they cannot be both non-nullable and have the 'Required' annotation.
        [Required(ErrorMessage = "Please select a Medication")]
        public int? MedicationID { get; set; }

        [Required(ErrorMessage = "Please select a Frequency")]
        public short? FrequencyID { get; set; }

        [Required(ErrorMessage = "Please select a Ordering Provider")]
        public int? OrderingProviderID { get; set; }
        public string OtherFrequencyDescription { get; set; }

        [Required(ErrorMessage = "An Indication is required")]
        public string Indication { get; set; }

        [Required(ErrorMessage = "Please enter the name of the person creating this entry")]
        public string EnteredBy { get; set; }

        public DateTime DateEntered { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public string Comments { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Medication Medications { get; set; }
        public virtual MedicationFrequency MedicationFrequencies { get; set; }
        public virtual Physician Physicians { get; set; }
        public virtual Patient Patients { get; set; }
        
    }
}
