using System;

namespace IS_Proj_HIT.ViewModels.Allergy
{
    public class AllergyListItemViewModel
    {
        public long PatientAllergyId { get; set; }
        public string AllergenName { get; set; }
        public string Description { get; set; }  // concatenated description field
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
 
}