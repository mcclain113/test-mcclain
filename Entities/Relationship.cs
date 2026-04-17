using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Relationship
    {
        public Relationship()
        {
            PatientEmergencyContacts = new HashSet<PatientEmergencyContact>();
            PatientInsurances = new HashSet<PatientInsurance>();
        }

        public int RelationshipId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<PatientEmergencyContact> PatientEmergencyContacts { get; set; }

        public virtual ICollection<PatientInsurance> PatientInsurances { get; set; }
    }
}
