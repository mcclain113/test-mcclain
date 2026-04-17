using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientInsurance
    {
        
        public PatientInsurance()
        {

        }

        public int PatientInsuranceId { get; set; }
        public string MRN { get; set; }
        public byte InsuranceOrder { get; set; }
        public string Guarantor { get; set; }
        public short? InsuranceProviderId {get; set;}
        public string Subscriber { get; set; }
        public int? SubscriberRelationshipID { get; set; }
        public string SubscriberNumber{ get; set; }
        public string GroupNumber { get; set; }
        public string PlanName { get; set; }
        public string PlanNumber { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Notes { get; set; }
        
       public virtual Patient Patient { get; set; }

       public virtual Relationship Relationship { get; set; }
       public virtual InsuranceProvider InsuranceProvider {get; set;}
        
    }
}
