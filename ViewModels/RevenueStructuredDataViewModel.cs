using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels
{
    public class RevenueStructuredDataViewModel
    {
        public RevenueCode RevenueCode {get; set;}
        public Department Department {get; set;}
        public ChargeDefinition ChargeDefinition {get; set;}
        public DisclosureFeeType DisclosureFeeType {get; set;}
        public PaymentType PaymentType {get; set;}

        // This property will serve as the deleted sort order value for DisclosureFeeType
        public int DeletedSortOrder { get; set; }

        public RevenueStructuredDataViewModel() { }

        public RevenueStructuredDataViewModel(RevenueCode revenueCode)
        {
            this.RevenueCode = revenueCode;
        }

        public RevenueStructuredDataViewModel(ChargeDefinition chargeDefinition)
        {
            this.ChargeDefinition = chargeDefinition;
        }

        public RevenueStructuredDataViewModel(DisclosureFeeType disclosureFeeType)
        {
            this.DisclosureFeeType = disclosureFeeType;
        }

        public RevenueStructuredDataViewModel(PaymentType paymentType)
        {
            this.PaymentType = paymentType;
        }

        public RevenueStructuredDataViewModel(ChargeDefinition chargeDefinition, Department department, RevenueCode revenueCode)
        {
            this.ChargeDefinition = chargeDefinition;
            this.Department = department;
            this.RevenueCode = revenueCode;
        }
    }

}