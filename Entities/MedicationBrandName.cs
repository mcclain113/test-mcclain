using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class MedicationBrandName
    {
        public int MedicationBrandId {get; set;}
        public string BrandName {get; set;}
        public bool IsActive {get; set;}
        public DateTime ModifiedDate {get; set;}
        public virtual ICollection<Medication> Medications { get; set; }       

    }
}