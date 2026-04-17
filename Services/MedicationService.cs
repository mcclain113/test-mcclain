using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace IS_Proj_HIT.Services
{
    class MedicationService : IMedicationService
    {
        private WCTCHealthSystemContext _context;

        public MedicationService(WCTCHealthSystemContext context) => _context = context;

        public async Task<List<PhysicianAssessmentAllergyDto>> GetPatientMedicationsAsync(string patientMrn)
        {
            // Load active PatientMedicationList rows for the patient including related Frequency and Medication properties
            var patientMedications = await _context.PatientMedicationLists
                .Where(m => m.IsActive == true && m.Mrn == patientMrn)
                .Include(m => m.MedicationFrequencies)
                .Include(m => m.Medications)
                    .ThenInclude(med => med.MedicationBrandName)
                .Include(m => m.Medications)
                    .ThenInclude(med => med.MedicationGenericName)
                .Include(m => m.Medications)
                    .ThenInclude(med => med.MedicationDosageForm)
                .Include(m => m.Medications)
                    .ThenInclude(med => med.MedicationDeliveryRoute)
                .ToListAsync();

            if (!patientMedications.Any()) return new List<PhysicianAssessmentAllergyDto>();

            // populate new
            var created = new List<PhysicianAssessmentAllergyDto>();
            int sort = 0;
            foreach (var pMedication in patientMedications)
            {
                // safe read using null-conditional, then test for null/whitespace
                string? brand = pMedication?.Medications?.MedicationBrandName?.BrandName;
                    string medicationBrandName = string.IsNullOrWhiteSpace(brand) ? "Brand Name Unknown" : brand!;
                string? generic = pMedication?.Medications?.MedicationGenericName?.GenericName;
                    string medicationGenericName = string.IsNullOrWhiteSpace(generic) ? "Generic Name Unknown" : generic!;
                string? strength = pMedication?.Medications?.ActiveStrength;
                    string medicationActiveStrength = string.IsNullOrWhiteSpace(strength) ? "Active Strength Unknown" : strength!;
                string? form = pMedication?.Medications?.MedicationDosageForm?.DosageForm;
                    string medicationDosageForm = string.IsNullOrWhiteSpace(form) ? "Dosage Form Unknown" : form!;
                string? route = pMedication?.Medications?.MedicationDeliveryRoute?.Description;
                    string medicationDeliveryRoute = string.IsNullOrWhiteSpace(route) ? "Delivery Route Unknown" : route!;
                string? frequency = pMedication?.MedicationFrequencies?.FrequencyDescription;
                    string medicationFrequency = string.IsNullOrWhiteSpace(frequency) ? "Frequency Unknown" : frequency!;

                string description;

                description = $"{medicationGenericName}|{medicationActiveStrength}|{medicationDosageForm}|{medicationBrandName}|{medicationDeliveryRoute}|{medicationFrequency}";
            
                var newRow = new PhysicianAssessmentAllergyDto
                    {
                        PhysicianAssessmentAllergyId = null,
                        PhysicianAssessmentId = null,
                        Description = description,
                        Type = "Medication",
                        SortOrder = sort++
                    };
                created.Add(newRow);
            }
            return created;
        }

        public async Task<List<PhysicianAssessmentAllergyDto>> GetAssessmentMedicationsAsync(long physicianAssessmentId)
        {
            return await _context.PhysicianAssessmentAllergies
                .AsNoTracking()
                .Where(a => a.PhysicianAssessmentId == physicianAssessmentId && a.Type == "Medication")
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