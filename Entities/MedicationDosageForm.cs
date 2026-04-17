using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class MedicationDosageForm
    {

        public short DosageFormId {get; set;}
        public string DosageForm {get; set;}
        public bool IsActive {get; set;}
        public DateTime DateModified {get; set;}

        public virtual ICollection<Medication> Medications { get; set; }   

    }
}