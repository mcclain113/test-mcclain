using System.Collections.Generic;
using System.Threading.Tasks;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Services
{
    public interface IMedicationService
    {
        Task<List<PhysicianAssessmentAllergyDto>> GetPatientMedicationsAsync(string patientMrn);
        Task<List<PhysicianAssessmentAllergyDto>> GetAssessmentMedicationsAsync(long physicianAssessmentId);
    }
}