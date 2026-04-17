using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{

    public partial class MedicationFrequency
    {

        public short MedicationFrequencyId { get; set; }
        public string  FrequencyCode { get; set; }
        public string FrequencyDescription{ get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<PatientMedicationList> PatientMedicationLists { get; set; }  
    }
}
