using System.Collections.Generic;

namespace IS_Proj_HIT.ViewModels.Allergy
{
    public class AllergyListViewModel
    {
        public string Mrn { get; set; }
        public List<AllergyListItemViewModel> ActiveAllergies { get; set; } = [];
        public List<AllergyListItemViewModel> InactiveAllergies { get; set; } = [];
    }
}