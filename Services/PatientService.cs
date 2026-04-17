using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IS_Proj_HIT.Services
{
    class PatientService : IPatientService
    {
        private WCTCHealthSystemContext _context;

        public PatientService(WCTCHealthSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Gets a patient for a given Mrn (PatientId)
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns></returns>
        public async Task<Patient> GetPatientByMrnAsync(string mrn) =>
            await _context.Patients
                .Where(x => x.Mrn == mrn)
                .FirstOrDefaultAsync();

        public async Task<Patient> GetPatientBannerDataByMrnAsync(string mrn)
        {
            if (string.IsNullOrWhiteSpace(mrn)) return null;
            
            return await _context.Patients
                .Include(p => p.Person)
                .Include(p => p.PatientAlerts)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Mrn == mrn);
        }

        /// <summary>
        ///     Fetch Patient data except for Documents, Encounters, PatientMedicationLists, PatientAllergies
        ///     Fetch Person data
        ///     Initially used in the PatientController, InitializePatientViewModel method but may have use elsewhere
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns></returns>
        public async Task<Patient> GetPatientAndPersonForPatientViewModelByMrnAsync(string mrn)
        {
            return await _context.Patients
            // patient-level collections that still live on Patient
            .Include(p => p.PatientAlerts)
            .Include(p => p.PatientEmergencyContacts)
            .Include(p => p.PatientInsurances)
            .Include(p => p.Facility)

            // include Person and navigations that moved from Patient to Person
            .Include(p => p.Person)
                .ThenInclude(person => person.PersonLanguages)
                    .ThenInclude(pl => pl.Language)
            .Include(p => p.Person)
                .ThenInclude(person => person.PersonRaces)
                    .ThenInclude(r => r.Race)
            .Include(p => p.Person).ThenInclude(person => person.EducationLevel)
            .Include(p => p.Person).ThenInclude(person => person.Employment)
            .Include(p => p.Person).ThenInclude(person => person.Ethnicity)
            .Include(p => p.Person).ThenInclude(person => person.Gender)
            .Include(p => p.Person).ThenInclude(person => person.GenderPronoun)
            .Include(p => p.Person).ThenInclude(person => person.MaritalStatus)
            .Include(p => p.Person).ThenInclude(person => person.Religion)
            .Include(p => p.Person).ThenInclude(person => person.Sex)
            .FirstOrDefaultAsync(p => p.Mrn == mrn);
        }

        /// <summary>
        /// Gets a SelectList of all patients, using MRN as the value
        /// and "MRN - FirstName LastName" as the text.
        /// </summary>
        /// <param name="selectedMrn">Optional MRN to mark as selected.</param>
        public async Task<SelectList> GetPatientSelectListAsync(int facilityId, string selectedMrn = null)
        {
            var items = await _context.Patients
                .Include(p => p.Person)
                .Where(p => p.FacilityId == facilityId)
                .OrderBy(p => p.Person.LastName)
                .ThenBy(p => p.Person.FirstName)
                .Select(p => new
                {
                    p.Mrn,
                    Display = $"{p.Mrn} - {p.Person.FirstName} {p.Person.LastName}"
                })
                .ToListAsync();

            return new SelectList(items, "Mrn", "Display", selectedMrn);
        }

        public async Task<IEnumerable<PatientSearchResult>> SearchPatientsAsync(int facilityId, string q, int maxResults = 25)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Array.Empty<PatientSearchResult>();

            q = q.Trim();

            // try parse date first (allow MM/dd/yyyy, yyyy-MM-dd, etc.)
            DateTime parsedDate;
            var isDate = DateTime.TryParse(q, out parsedDate);

            // normalize q for LIKE search
            var qLower = q.ToLower();

            var query = _context.Patients
                .Include(p => p.Person)
                .Where(p => p.FacilityId == facilityId);

            // build predicate: MRN equals or contains OR names contain OR DOB matches
            if (isDate)
            {
                var dt = parsedDate.Date;
                query = query.Where(p => p.Person.Dob.HasValue && p.Person.Dob.Value.Date == dt
                                         || EF.Functions.Like(p.Mrn.ToLower(), $"%{qLower}%")
                                         || EF.Functions.Like((p.Person.FirstName + " " + p.Person.LastName).ToLower(), $"%{qLower}%"));
            }
            else
            {
                query = query.Where(p =>
                    EF.Functions.Like(p.Mrn.ToLower(), $"%{qLower}%")
                    || EF.Functions.Like((p.Person.FirstName ?? "").ToLower(), $"%{qLower}%")
                    || EF.Functions.Like((p.Person.LastName ?? "").ToLower(), $"%{qLower}%")
                    || EF.Functions.Like((p.Person.FirstName + " " + p.Person.LastName).ToLower(), $"%{qLower}%")
                );
            }

            var matches = await query
                .OrderBy(p => p.Person.LastName)
                .ThenBy(p => p.Person.FirstName)
                .ThenBy(p => p.Mrn)
                .Select(p => new PatientSearchResult
                {
                    Mrn = p.Mrn,
                    FirstName = p.Person.FirstName,
                    LastName = p.Person.LastName,
                    Dob = p.Person.Dob
                })
                .Take(maxResults)
                .ToListAsync();

            return matches;
        }
    }
}