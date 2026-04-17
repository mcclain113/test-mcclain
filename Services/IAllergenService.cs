using System.Collections.Generic;
using System.Threading.Tasks;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Services
{
    public interface IAllergenService
    {
        Task<IEnumerable<Allergen>> GetAllAllergensAsync();
        Task<List<PhysicianAssessmentAllergyDto>> GetPatientAllergiesAsync(string patientMrn);
        Task<List<PhysicianAssessmentAllergyDto>> GetAssessmentAllergiesAsync(long physicianAssessmentId);

    }
}