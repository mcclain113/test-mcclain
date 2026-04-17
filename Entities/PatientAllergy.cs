using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class PatientAllergy
    {
        public long PatientAllergyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsActive { get; set; }
#nullable enable
        public int? AllergenId { get; set; }
        public int? ReactionId { get; set; }
        public DateTime? EndDate { get; set; }
        public long? PatientAlertId { get; set; }   // if have PatientAlertId, do not need Mrn
        public string? Mrn { get; set; }    // For PatientAllergy which does not require a PatientAlert, must have Mrn
        public int? GenericMedicationId { get; set; }
        public string? Comments { get; set; }

#nullable disable

        public virtual Allergen Allergen { get; set; }
        public virtual MedicationGenericName MedicationGenericName { get; set; }
        public virtual PatientAlert PatientAlert { get; set; }
        public virtual Reaction Reaction { get; set; }
    }
}
