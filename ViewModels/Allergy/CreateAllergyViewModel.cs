using IS_Proj_HIT.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IS_Proj_HIT.ViewModels.Allergy
{
    public class CreateAllergyViewModel : IValidatableObject
    {
        [Required]
        public string Mrn { get; set; }

        [Display(Name = "Allergen")]
        [Required(ErrorMessage = "Please select an Allergen.")]
        public int AllergenId { get; set; }

        [Display(Name = "Reaction")]
        [Required(ErrorMessage = "Please select a Reaction.")]
        public int? ReactionId { get; set; }

        [Display(Name = "Medication Name")]
        public int? GenericMedicationId { get; set; }

        // Collections for populating selects
        public IEnumerable<Allergen> Allergens { get; set; } = Enumerable.Empty<Allergen>();
        public IEnumerable<Reaction> Reactions { get; set; } = Enumerable.Empty<Reaction>();
        public IEnumerable<MedicationGenericName> MedicationGenericNames { get; set; } = Enumerable.Empty<MedicationGenericName>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Ensure Reaction selected
            if (!ReactionId.HasValue)
            {
                yield return new ValidationResult("Please select a Reaction.", new[] { nameof(ReactionId) });
            }

            // If user selected the allergen that means "Medication" then require GenericMedicationId
            // This logic relies on the Allergens collection being populated (same as in GET)
            var selectedAllergen = Allergens?.FirstOrDefault(a => a.AllergenId == AllergenId);
            if (selectedAllergen != null && string.Equals(selectedAllergen.AllergenName, "Medication", System.StringComparison.OrdinalIgnoreCase))
            {
                if (!GenericMedicationId.HasValue)
                {
                    yield return new ValidationResult("Please select a Medication.", new[] { nameof(GenericMedicationId) });
                }
            }
        }
    }
}