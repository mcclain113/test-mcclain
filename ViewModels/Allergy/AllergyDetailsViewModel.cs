using System;

namespace IS_Proj_HIT.ViewModels.Allergy
{
    public class AllergyDetailsViewModel
    {
        public long PatientAllergyId { get; set; }
        public string Mrn { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime StartDate { get; set; }

    #nullable enable
        public int? AllergenId { get; set; }

        public int? ReactionId { get; set; }
        
        public DateTime? EndDate { get; set; } 
        public string? Comments { get; set; } 
        public int? GenericMedicationId { get; set; }      
        public long? PatientAlertId { get; set; }
        public string? AllergenName { get; set; }
        public string? ReactionName { get; set; }
        public string? AllergenDisplay { get; set; }
    }
}