using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public class InsuranceProvider
    {
        public short InsuranceProviderId {get; set;}
        public string ProviderName {get; set;} = null!;
        public virtual ICollection<PatientInsurance> PatientInsurances {get; set;} = new List<PatientInsurance>();
    }
}