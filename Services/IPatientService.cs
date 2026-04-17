using System.Collections.Generic;
using System.Threading.Tasks;
using IS_Proj_HIT.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.Services
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByMrnAsync(string mrn);
        Task<Patient> GetPatientBannerDataByMrnAsync(string mrn);
        
        // Fetch all Patient data except Allergies & Medications, Fetch all Person data
        Task<Patient> GetPatientAndPersonForPatientViewModelByMrnAsync(string mrn);
        Task<SelectList> GetPatientSelectListAsync(int facilityId, string selectedMrn = null);

        // Search patients by MRN, name or DOB (q can be MRN, name fragment, or date)
        Task<IEnumerable<PatientSearchResult>> SearchPatientsAsync(int facilityId, string q, int maxResults = 25);

    }
}