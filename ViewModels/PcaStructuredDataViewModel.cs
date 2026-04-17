using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels
{
    public class PcaStructuredDataViewModel
    {
        public BloodPressureRouteType BloodPressureRouteType {get; set;}
        public Bmimethod Bmimethod {get; set;}
        public CareSystemParameter CareSystemParameter {get; set;}
        public CareSystemType CareSystemType {get; set;}
        public O2deliveryType O2deliveryType {get; set;}
        public PainParameter PainParameter {get; set;}
        public PainRating PainRating {get; set;}
        public PainScaleType PainScaleType {get; set;}
        public PcacommentType PcacommentType {get; set;}
        public PulseRouteType PulseRouteType {get; set;}
        public TempRouteType TempRouteType {get; set;}

        public PcaStructuredDataViewModel(){}
        public PcaStructuredDataViewModel(BloodPressureRouteType bloodPressureRouteType)
        {
            this.BloodPressureRouteType = bloodPressureRouteType;
        }
        public PcaStructuredDataViewModel(Bmimethod bmimethod)
        {
            this.Bmimethod = bmimethod;
        }
        public PcaStructuredDataViewModel(CareSystemParameter careSystemParameter)
        {
            this.CareSystemParameter = careSystemParameter;
        }
        public PcaStructuredDataViewModel(CareSystemType careSystemType)
        {
            this.CareSystemType = careSystemType;
        }
        public PcaStructuredDataViewModel(O2deliveryType o2DeliveryType)
        {
            this.O2deliveryType = o2DeliveryType;
        }
        public PcaStructuredDataViewModel(PainParameter painParameter)
        {
            this.PainParameter = painParameter;
        }
        public PcaStructuredDataViewModel(PainRating painRating)
        {
            this.PainRating = painRating;
        }
        public PcaStructuredDataViewModel(PainScaleType painScaleType)
        {
            this.PainScaleType = painScaleType;
        }
        public PcaStructuredDataViewModel(PcacommentType pcacommentType)
        {
            this.PcacommentType = pcacommentType;
        }
        public PcaStructuredDataViewModel(PulseRouteType pulseRouteType)
        {
            this.PulseRouteType = pulseRouteType;
        }
        public PcaStructuredDataViewModel(TempRouteType tempRouteType)
        {
            this.TempRouteType = tempRouteType;
        }
    }
}