using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.AlertStructuredData
{
    public class AlertStructuredDataViewModel
    {
        public AlertType AlertType { get; set; }
        public AdvancedDirective AdvancedDirective { get; set; }
        public Allergen Allergen { get; set; }
        public ClinicalReminder ClinicalReminder { get; set; }
        public FallRisk FallRisk { get; set; }
        public Reaction Reaction { get; set; }
        public Restriction Restriction { get; set; }

        // constructors
        public AlertStructuredDataViewModel() { }

        public AlertStructuredDataViewModel(AlertType alertType)
        {
            this.AlertType = alertType;
        }

        public AlertStructuredDataViewModel(AdvancedDirective advancedDirective)
        {
            this.AdvancedDirective = advancedDirective;
        }

        public AlertStructuredDataViewModel(Allergen allergen)
        {
            this.Allergen = allergen;
        }

        public AlertStructuredDataViewModel(ClinicalReminder clinicalReminder)
        {
            this.ClinicalReminder = clinicalReminder;
        }

        public AlertStructuredDataViewModel(FallRisk fallRisk)
        {
            this.FallRisk = fallRisk;
        }

        public AlertStructuredDataViewModel(Reaction reaction)
        {
            this.Reaction = reaction;
        }

        public AlertStructuredDataViewModel(Restriction restriction)
        {
            this.Restriction = restriction;
        }
    }
}