using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class EncounterHistory
    {
        public string Mrn { get; set; }
        public long EncounterId { get; set; }
        public string ChiefComplaint { get; set; }
        public string Description { get; set; }
        public string Explaination { get; set; }
        public int? AdmitDate { get; set; }
        public string FacilityName { get; set; }
        public string DepartmentName { get; set; }
        public string DischargeDate { get; set; }
    }
}
