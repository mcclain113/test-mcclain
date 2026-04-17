using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class MedicationGenericName
    {
        public int MedicationGenericId {get; set;}
        public string GenericName {get; set;}
        public bool IsActive {get; set;}        
        public DateTime ModifiedDate {get; set;}
        public virtual ICollection<Medication> Medications { get; set; }   
        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
    }
}