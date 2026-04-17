using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace IS_Proj_HIT.Services
{
    class AllergenService : IAllergenService
    {
        private WCTCHealthSystemContext _context;

        public AllergenService(WCTCHealthSystemContext context) => _context = context;
    
        /// <summary>
        ///     Gets a list of all possible allergens
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Allergen>> GetAllAllergensAsync() =>
            await _context.Allergens
                .ToListAsync();
   
        public async Task<List<PhysicianAssessmentAllergyDto>> GetPatientAllergiesAsync(string patientMrn)
        {
            var patientAllergies = await _context.PatientAllergies
                .AsNoTracking()
                .Where(a => a.IsActive == true && a.Mrn == patientMrn)
                .Include(a => a.Allergen)
                .Include(a => a.Reaction)
                .Include(a => a.MedicationGenericName)
                .ToListAsync();

            if (!patientAllergies.Any()) return new List<PhysicianAssessmentAllergyDto>();

            // populate new
            var created = new List<PhysicianAssessmentAllergyDto>();
            int sort = 0;
            foreach (var pAllergy in patientAllergies)
            {
                // Build Description: "AllergenName: ReactionName"
                // fall back to sensible alternatives if navigation is null
                string allergenName = null;

                var allergenFromObject = pAllergy.Allergen?.AllergenName;
                var genericFromObject = pAllergy.MedicationGenericName?.GenericName;

                if (string.Equals(allergenFromObject, "Medication", StringComparison.OrdinalIgnoreCase))
                {
                    // Only use generic if we actually have one
                    allergenName = !string.IsNullOrEmpty(genericFromObject) ? genericFromObject : null;
                }
                else
                {
                    allergenName = allergenFromObject ?? (pAllergy.GenericMedicationId.HasValue ? genericFromObject : null);
                }

                var reactionName = pAllergy.Reaction?.Name;

                // If both are missing, optionally use Comments or skip the row
                string description;
                if (!string.IsNullOrWhiteSpace(allergenName) && !string.IsNullOrWhiteSpace(reactionName))
                {
                    description = $"{allergenName}: {reactionName}";
                }
                else if (!string.IsNullOrWhiteSpace(allergenName))
                {
                    description = allergenName;
                }
                else if (!string.IsNullOrWhiteSpace(reactionName))
                {
                    description = reactionName;
                }
                else if (!string.IsNullOrWhiteSpace(pAllergy.Comments))
                {
                    description = pAllergy.Comments;
                }
                else
                {
                    // nothing useful to show; skip this PatientAllergy
                    continue;
                }

                var newRow = new PhysicianAssessmentAllergyDto
                {
                    PhysicianAssessmentAllergyId = null,
                    PhysicianAssessmentId = null,
                    Description = description,
                    Type = "Allergy",
                    SortOrder = sort++
                };
                created.Add(newRow);
            }
            return created;
        }

        
        public async Task<List<PhysicianAssessmentAllergyDto>> GetAssessmentAllergiesAsync(long physicianAssessmentId)
        {
            return await _context.PhysicianAssessmentAllergies
                .AsNoTracking()
                .Where(a => a.PhysicianAssessmentId == physicianAssessmentId && a.Type == "Allergy")
                .OrderBy(a => a.SortOrder)
                .Select(a => new PhysicianAssessmentAllergyDto
                {
                    PhysicianAssessmentAllergyId = a.PhysicianAssessmentAllergyId,
                    PhysicianAssessmentId = a.PhysicianAssessmentId,
                    Description = a.Description,
                    SortOrder = a.SortOrder,
                    Type = a.Type
                })
                .ToListAsync();
        }
    
    }
}