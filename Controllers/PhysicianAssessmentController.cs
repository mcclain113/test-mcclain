using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using IS_Proj_HIT.ViewModels.Encounters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IS_Proj_HIT.Controllers
 {
    [Authorize]
    [PermissionAuthorize("HistoryPhysicalAdd","HistoryPhysicalDelete","HistoryPhysicalEdit","HistoryPhysicalView")]
     public class PhysicianAssessmentController : BaseController
     {
        
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAllergenService _allergenService;
        private readonly IMedicationService _medicationService;
        
        public PhysicianAssessmentController(
                IWCTCHealthSystemRepository repository,
                UserManager<IdentityUser> userManager,
                IAllergenService allergenService,
                IMedicationService medicationService
            ) 
            {
                _repository = repository;
                _userManager = userManager;
                _allergenService = allergenService;
                _medicationService = medicationService;
            }

        /// <summary>
        ///    Display the form to enter a History and Physical Physician Assessment - Getter
        /// </summary>
        /// <param name="id">Id of unique encounter</param>
        [Authorize]
        [PermissionAuthorize("HistoryPhysicalView")]
        public async Task<IActionResult> ViewHistoryAndPhysical(long id)
        {
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            ViewData["UserPermissions"] = permissions;

            // Find any existing physician assessment for the encounter
            var historyPhysical = _repository.PhysicianAssessments
                .Include(hp => hp.Encounter)
                .Include(hp => hp.AuthoringProviderNavigation)
                .Include(hp => hp.CoSignatureNavigation)
                .Include(hp => hp.BodySystemAssessments)
                .ThenInclude(bst => bst.BodySystemType)
                .ThenInclude(ex => ex.ExamType)
                .FirstOrDefault(hp => hp.EncounterId == id);

            var encounter = _repository.Encounters
                .Include(e => e.EncounterPhysicians)
                .ThenInclude(e => e.Physician)
                .Include(e => e.PlaceOfService)
                .FirstOrDefault(e => e.EncounterId == id);

            if (encounter == null)
                return RedirectToAction("ViewEncounter", "Encounter", new { id });

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);

            if (patient == null)
                return RedirectToAction("ViewEncounter", "Encounter", new { id });

            // build allergy list
            List<PhysicianAssessmentAllergyDto> assessmentAllergies;
            if (historyPhysical == null)
            {
                assessmentAllergies = new List<PhysicianAssessmentAllergyDto>();
            }
            else
            {
                assessmentAllergies = await _allergenService.GetAssessmentAllergiesAsync(historyPhysical.PhysicianAssessmentId);
            }

            // build medications list
            List<PhysicianAssessmentAllergyDto> assessmentMedications;
            if (historyPhysical == null)
            {
                assessmentMedications = new List<PhysicianAssessmentAllergyDto>();
            }
            else
            {
                assessmentMedications = await _medicationService.GetAssessmentMedicationsAsync(historyPhysical.PhysicianAssessmentId);
            }


            // Build exam types and deterministic BodySystemAssessments list
            var systemExamTypes = _repository.ExamTypes
                .Where(et => et.ExamTypeCode < 100)
                .Include(et => et.BodySystemTypes)
                .ToList();

            var bsaList = new List<BodySystemAssessment>();

            // If an existing assessment is present, use its BSA values. Otherwise preserve empty placeholders.
            foreach (var et in systemExamTypes)
            {
                foreach (var bst in et.BodySystemTypes)
                {
                    BodySystemAssessment existing = null;
                    if (historyPhysical != null && historyPhysical.BodySystemAssessments != null)
                    {
                        existing = historyPhysical.BodySystemAssessments
                            .FirstOrDefault(x => x.BodySystemTypeId == bst.BodySystemTypeId);
                    }

                    if (existing != null)
                    {
                        if (existing.Comment == null) existing.Comment = string.Empty;
                        existing.BodySystemTypeId = bst.BodySystemTypeId;
                        bsaList.Add(existing);
                    }
                    else
                    {
                        bsaList.Add(new BodySystemAssessment
                        {
                            BodySystemTypeId = bst.BodySystemTypeId,
                            IsWithinNormalLimits = null,
                            Comment = string.Empty
                        });
                    }
                }
            }

            // Build view model (use constructor that accepts assessment and encounter)
            PhysicianAssessmentViewModel model;
            if (historyPhysical != null)
            {
                model = new PhysicianAssessmentViewModel(historyPhysical, encounter);
            }
            else
            {
                model = new PhysicianAssessmentViewModel(encounter);
            }

            model.SystemExamTypes = systemExamTypes;
            model.BodySystemAssessments = bsaList;
            model.AssessmentAllergies = assessmentAllergies;
            model.AssessmentMedications = assessmentMedications;
            model.PatientMrn = encounter.Mrn;

            ViewBag.Encounter = encounter;
            ViewBag.Patient = patient;

            return View(model);
        }

        /// <summary>
        ///     Display the form to update an existing History and Physical - Getter
        /// </summary>
        /// <param name = "assessmentId" > PhysicianAssessment Id for Db Lookup</param>
        ///  <param name = "patientMrn" > Unique Identifier of patient</param>
        ///   <param name = "encounterId" > Unique Identifier of patient</param>
        [Authorize]
        [PermissionAuthorize("HistoryPhysicalAdd","HistoryPhysicalEdit")]
        public async Task<IActionResult> UpdateHistoryAndPhysicalAsync(long assessmentId, string patientMRN, long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var physAssessment = _repository.PhysicianAssessments
                .Where(pa => pa.EncounterId == encounterId)
                .Include(pa => pa.BodySystemAssessments)
                .ThenInclude(bst => bst.BodySystemType)
                .ThenInclude(et => et.ExamType)
                .FirstOrDefault(pa => pa.PhysicianAssessmentId == assessmentId);

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == patientMRN);

            var encounter = _repository.Encounters
                .Include(e => e.EncounterPhysicians)
                .ThenInclude(e => e.Physician)
                .Include(e => e.PlaceOfService)
                .FirstOrDefault(e => e.EncounterId == encounterId);

            var patientAlerts = _repository.PatientAlerts
                .Where(b => b.Mrn == patientMRN).Count();

            var queryPhysician = _repository.Physicians
                .Include(n => n.Specialty)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                .ToList();

            var queryCosigner = _repository.Physicians
                .Where(p => p.ProviderStatusId == 1)
                .Include(p => p.Specialty)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                .ToList();

            if (encounter is null || patient is null)
                return RedirectToAction("ViewEncounter", "Encounter",
                    new { encounterId = encounterId, isPatientEncounter = false });

            var systemExamTypes = _repository.ExamTypes
                .Include(bs => bs.BodySystemTypes)
                .ToList();

            var bodySystemAssessments = new List<BodySystemAssessment>();

            ViewBag.Encounter = encounter;
            ViewBag.Patient = patient;
            ViewBag.PhysicianAssessment = physAssessment;
            ViewBag.Providers = new SelectList(queryPhysician, "PhysicianId", "FullName", selectedValue:null);
            ViewBag.Cosigners = new SelectList(queryCosigner,  "PhysicianId", "FullName",  selectedValue:null);

            // check if new assessment
            if ( assessmentId == 0 )
            {
                ViewBag.checkForNull = true;
                var newModelHistoryPhysical = PrepareHistoryPhysicalViewModel(encounter);
                return View(newModelHistoryPhysical);
            }

            ViewBag.checkForNull = false;
            var bsaID = physAssessment.BodySystemAssessments.FirstOrDefault();

            foreach (var set in systemExamTypes)
            {
                foreach (var bst in set.BodySystemTypes)
                {
                    // Try to find an existing BodySystemAssessment for this BodySystemType
                    var existing = physAssessment?
                        .BodySystemAssessments
                        .FirstOrDefault(bsa => bsa.BodySystemTypeId == bst.BodySystemTypeId &&
                                            bst.ExamTypeCode == set.ExamTypeCode);

                    if (existing != null)
                    {
                        // Ensure Comment is not null to simplify client logic
                        if (existing.Comment == null) existing.Comment = string.Empty;

                        // Make sure BodySystemTypeId is correct (defensive)
                        existing.BodySystemTypeId = bst.BodySystemTypeId;

                        bodySystemAssessments.Add(existing);
                    }
                    else
                    {
                        // Add a placeholder entry so the view always renders an item in the same order
                        bodySystemAssessments.Add(new BodySystemAssessment
                        {
                            BodySystemTypeId = bst.BodySystemTypeId,
                            IsWithinNormalLimits = null,
                            Comment = string.Empty
                        });
                    }
                }
            }

            List<PhysicianAssessmentAllergyDto> assessmentAllergies;
            if (physAssessment == null)
            {
                assessmentAllergies = new List<PhysicianAssessmentAllergyDto>();
            }
            else
            {
                assessmentAllergies = await _allergenService.GetAssessmentAllergiesAsync(physAssessment.PhysicianAssessmentId);
            }

            // set a flag to control which buttons are visible
            if(assessmentAllergies.Any())
            {
                ViewBag.allergiesExist = true;
            }
            else
            {
                ViewBag.allergiesExist = false;
            }

            
            List<PhysicianAssessmentAllergyDto> assessmentMedications;
            if (physAssessment == null)
            {
                assessmentMedications = new List<PhysicianAssessmentAllergyDto>();
            }
            else
            {
                assessmentMedications = await _medicationService.GetAssessmentMedicationsAsync(physAssessment.PhysicianAssessmentId);
            }
            // set a flag to control which buttons are visible
            if(assessmentMedications.Any())
            {
                ViewBag.medicationsExist = true;
            }
            else
            {
                ViewBag.medicationsExist = false;
            }

            var updateHistoryPhysical = new PhysicianAssessmentViewModel
            (
                physAssessment, encounter, systemExamTypes, bodySystemAssessments, assessmentAllergies, assessmentMedications
            );

            return View(updateHistoryPhysical);
        }

        /// <summary>
        /// Post form for updating existing History and Physical. Check for errors. Trigger SaveAssessment if none.
        /// </summary>
        /// <param name="model">view model for History and Physical</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("HistoryPhysicalAdd","HistoryPhysicalEdit")]
        public IActionResult UpdateHistoryAndPhysical(PhysicianAssessmentViewModel model)
        {
            // Defensive: ensure model and nested objects exist
            if (model == null) return BadRequest();

            // Treat AuthoringProvider == 0 as "not selected" (client posts empty option => model binder gives 0)
            var authoringProviderSelected = model.PhysicianAssessment?.AuthoringProvider != 0;

            if (!ModelState.IsValid || !authoringProviderSelected)
            {
                // Recreate encounter and patient for the view
                var encounter = _repository.Encounters
                    .Include(e => e.PlaceOfService)
                    .FirstOrDefault(e => e.EncounterId == model.PhysicianAssessment.EncounterId);

                var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == model.PatientMrn);

                // Re-query exam types and body system types to rebuild deterministic list order
                var systemExamTypes = _repository.ExamTypes
                    .Include(bs => bs.BodySystemTypes)
                    .ToList();

                // Rebuild BodySystemAssessments as a List in the same order the view renders
                var bsaList = new List<BodySystemAssessment>();
                foreach (var set in systemExamTypes)
                {
                    foreach (var bst in set.BodySystemTypes)
                    {
                        // Try to find the posted entry in the incoming model by BodySystemTypeId
                        var posted = model.BodySystemAssessments?
                            .FirstOrDefault(x => x.BodySystemTypeId == bst.BodySystemTypeId);

                        if (posted != null)
                        {
                            // Ensure non-null fields for client-side logic
                            if (posted.Comment == null) posted.Comment = string.Empty;
                            bsaList.Add(posted);
                        }
                        else
                        {
                            bsaList.Add(new BodySystemAssessment
                            {
                                BodySystemTypeId = bst.BodySystemTypeId,
                                IsWithinNormalLimits = null,
                                Comment = string.Empty
                            });
                        }
                    }
                }

                // Recreate select lists and preserve the user's selected values so Select2 shows correctly
                var queryPhysician = _repository.Physicians
                    .Include(n => n.Specialty)
                    .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                    .Select(p => new { p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})" })
                    .ToList();

                var queryCosigner = _repository.Physicians
                    .Where(p => p.ProviderStatusId == 1)
                    .Include(p => p.Specialty)
                    .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                    .Select(p => new { p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})" })
                    .ToList();

                // Use selectedValue: null for initial empty, or the posted numeric id to show selection
                ViewBag.Providers = new SelectList(queryPhysician, "PhysicianId", "FullName", model.PhysicianAssessment?.AuthoringProvider == 0 ? null : (object)model.PhysicianAssessment.AuthoringProvider);
                ViewBag.Cosigners = new SelectList(queryCosigner, "PhysicianId", "FullName", model.PhysicianAssessment?.CoSignature == 0 ? null : (object)model.PhysicianAssessment.CoSignature);

                ViewBag.Encounter = encounter;
                ViewBag.Patient = patient;
                ViewBag.checkForNull = false;

                // Assign the rebuilt list back into the model and return it so posted inputs remain intact
                model.BodySystemAssessments = bsaList;
                model.SystemExamTypes = systemExamTypes;

                return View(model);
            }

            // defensive clearing of cosigner info when cosigner is not required
            var authorProviderId = model.PhysicianAssessment?.AuthoringProvider;
            int? statusNumber = null;

            if (authorProviderId != 0)
            {
                var physician = _repository.Physicians.FirstOrDefault(p => p.PhysicianId == authorProviderId);
                statusNumber = physician?.ProviderStatusId;
            }

            // If provider does not require cosign (null or 1), clear cosign fields
            if (statusNumber == null || statusNumber == 1)
            {
                model.PhysicianAssessment.CoSignature = null;
                model.PhysicianAssessment.CoSigningProviderSignature = null;
                model.PhysicianAssessment.CoSigningProviderSignedDate = null;
            }

            // Valid -> proceed to save
            return SaveAssessment(model);
        }

        /// <summary>
        /// Save the information added or updated in the History and Physical form. 
        /// </summary>
        /// <param name="model">view model for History and Physical</param>
        /// <returns></returns>
        private IActionResult SaveAssessment(PhysicianAssessmentViewModel model)
        {
            
            var editedBy = _repository.UserTables
                .Where(x => x.AspNetUsersId == _userManager.GetUserId(User))
                .Select(x => x.UserId)
                .FirstOrDefault();
            
            // for new physician assessments
            if (model.PhysicianAssessment.PhysicianAssessmentId == 0)
            {
                using (var tran = new TransactionScope())
                {
                    var pa = new PhysicianAssessment
                    {
                        ChiefComplaint = model.PhysicianAssessment.ChiefComplaint?.Trim(),
                        HistoryOfPresentIllness = model.PhysicianAssessment.HistoryOfPresentIllness?.Trim(),
                        PastMedicalSurgicalHistory = model.PhysicianAssessment.PastMedicalSurgicalHistory?.Trim(),
                        SocialHistory = model.PhysicianAssessment.SocialHistory?.Trim(),
                        FamilyHistory = model.PhysicianAssessment.FamilyHistory?.Trim(),
                        SignificantDiagnosticTests = model.PhysicianAssessment.SignificantDiagnosticTests?.Trim(),
                        Assessment = model.PhysicianAssessment.Assessment?.Trim(),
                        Plan = model.PhysicianAssessment.Plan?.Trim(),
                        PhysicianAssessmentDate = model.PhysicianAssessment.PhysicianAssessmentDate,
                        ReferringProvider = model.PhysicianAssessment.ReferringProvider,
                        AuthoringProvider = model.PhysicianAssessment.AuthoringProvider,
                        AuthoringProviderSignature = string.IsNullOrWhiteSpace(model.PhysicianAssessment.AuthoringProviderSignature)
                            ? null
                            : model.PhysicianAssessment.AuthoringProviderSignature.Trim(),
                        AuthoringProviderSignedDate = model.PhysicianAssessment.AuthoringProviderSignedDate,
                        CoSignature = model.PhysicianAssessment.CoSignature == 0 ? (int?)null : model.PhysicianAssessment.CoSignature,
                        CoSigningProviderSignature = string.IsNullOrWhiteSpace(model.PhysicianAssessment.CoSigningProviderSignature)
                            ? null
                            : model.PhysicianAssessment.CoSigningProviderSignature.Trim(),
                        CoSigningProviderSignedDate = model.PhysicianAssessment.CoSigningProviderSignedDate,
                        WrittenDateTime = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        EditedBy = editedBy,
                        EncounterId = model.PhysicianAssessment.EncounterId,
                        PhysicianAssessmentTypeId = 1
                    };

                    _repository.AddPhysicianAssessment(pa);

                    if (model.BodySystemAssessments != null)
                    {
                        foreach (var bsa in model.BodySystemAssessments.Where(x => x.IsWithinNormalLimits != null))
                        {
                            // Defensive: ensure BodySystemTypeId is set
                            if (bsa.BodySystemTypeId == 0) continue;

                            bsa.PhysicianAssessmentId = pa.PhysicianAssessmentId;
                            bsa.LastModified = DateTime.Now;
                            bsa.Comment = bsa.Comment?.Trim();
                            _repository.AddBodySystemAssessment(bsa);
                        }
                    }

                    if (model.AssessmentAllergies != null && model.AssessmentAllergies.Any())
                    {
                        foreach (var dto in model.AssessmentAllergies)
                        {
                            var paAllergy = new PhysicianAssessmentAllergy
                            {
                                PhysicianAssessmentId = pa.PhysicianAssessmentId,
                                Description = dto.Description?.Trim(),
                                SortOrder = dto.SortOrder,
                                Type = dto.Type
                            };
                            _repository.AddPhysicianAssessmentAllergy(paAllergy);
                        }
                        
                    }

                    if (model.AssessmentMedications != null && model.AssessmentMedications.Any())
                    {
                        foreach (var dto in model.AssessmentMedications)
                        {
                            var paMed = new PhysicianAssessmentAllergy
                            {
                                PhysicianAssessmentId = pa.PhysicianAssessmentId,
                                Description = dto.Description?.Trim(),
                                SortOrder = dto.SortOrder,
                                Type = dto.Type
                            };
                            _repository.AddPhysicianAssessmentAllergy(paMed);
                        }
                        
                    }

                    tran.Complete();
                }

            }   // if physician assessment exists, update it accordingly
            else
            {
                using (var tran = new TransactionScope())
                {
                    var existingHistoryPhysical = _repository.PhysicianAssessments
                        .Include(pa => pa.BodySystemAssessments)
                        .FirstOrDefault(pcr => pcr.PhysicianAssessmentId == model.PhysicianAssessment.PhysicianAssessmentId);

                    if (existingHistoryPhysical == null)
                    {
                        // Nothing to update; fail fast or handle as needed
                        return BadRequest();
                    }

                    // Update scalar fields (trim strings, treat empty signatures as null)
                    existingHistoryPhysical.ChiefComplaint = model.PhysicianAssessment.ChiefComplaint?.Trim();
                    existingHistoryPhysical.HistoryOfPresentIllness = model.PhysicianAssessment.HistoryOfPresentIllness?.Trim();
                    existingHistoryPhysical.PastMedicalSurgicalHistory = model.PhysicianAssessment.PastMedicalSurgicalHistory?.Trim();
                    existingHistoryPhysical.SocialHistory = model.PhysicianAssessment.SocialHistory?.Trim();
                    existingHistoryPhysical.FamilyHistory = model.PhysicianAssessment.FamilyHistory?.Trim();
                    existingHistoryPhysical.SignificantDiagnosticTests = model.PhysicianAssessment.SignificantDiagnosticTests?.Trim();
                    existingHistoryPhysical.Assessment = model.PhysicianAssessment.Assessment?.Trim();
                    existingHistoryPhysical.Plan = model.PhysicianAssessment.Plan?.Trim();
                    existingHistoryPhysical.ReferringProvider = model.PhysicianAssessment.ReferringProvider;
                    existingHistoryPhysical.AuthoringProvider = model.PhysicianAssessment.AuthoringProvider;
                    existingHistoryPhysical.AuthoringProviderSignature = string.IsNullOrWhiteSpace(model.PhysicianAssessment.AuthoringProviderSignature)
                        ? null
                        : model.PhysicianAssessment.AuthoringProviderSignature.Trim();
                    existingHistoryPhysical.AuthoringProviderSignedDate = model.PhysicianAssessment.AuthoringProviderSignedDate;
                    existingHistoryPhysical.CoSignature = model.PhysicianAssessment.CoSignature == 0
                        ? (int?)null
                        : model.PhysicianAssessment.CoSignature;
                    existingHistoryPhysical.CoSigningProviderSignature = string.IsNullOrWhiteSpace(model.PhysicianAssessment.CoSigningProviderSignature)
                        ? null
                        : model.PhysicianAssessment.CoSigningProviderSignature.Trim();
                    existingHistoryPhysical.CoSigningProviderSignedDate = model.PhysicianAssessment.CoSigningProviderSignedDate;
                    existingHistoryPhysical.PhysicianAssessmentDate = model.PhysicianAssessment.PhysicianAssessmentDate;
                    existingHistoryPhysical.WrittenDateTime = model.PhysicianAssessment.WrittenDateTime;
                    existingHistoryPhysical.LastUpdated = DateTime.Now;
                    existingHistoryPhysical.EditedBy = editedBy;
                    existingHistoryPhysical.EncounterId = model.PhysicianAssessment.EncounterId;

                    // Ensure posted BodySystemAssessments exists
                    var postedList = model.BodySystemAssessments ?? new List<BodySystemAssessment>();

                    foreach (var postedBsa in postedList)
                    {
                        // Defensive: skip entries without a valid BodySystemTypeId
                        if (postedBsa.BodySystemTypeId == 0) continue;

                        var existBsa = existingHistoryPhysical.BodySystemAssessments
                            .FirstOrDefault(a => a.BodySystemTypeId == postedBsa.BodySystemTypeId);

                        if (existBsa == null)
                        {
                            // create new only if user selected a value (not null)
                            if (postedBsa.IsWithinNormalLimits != null)
                            {
                                postedBsa.PhysicianAssessmentId = existingHistoryPhysical.PhysicianAssessmentId;
                                postedBsa.LastModified = DateTime.Now;
                                postedBsa.Comment = postedBsa.Comment?.Trim();
                                existingHistoryPhysical.BodySystemAssessments.Add(postedBsa);
                            }
                        }
                        else
                        {
                            // update existing record
                            existBsa.Comment = postedBsa.Comment?.Trim();
                            existBsa.IsWithinNormalLimits = postedBsa.IsWithinNormalLimits;
                            existBsa.LastModified = DateTime.Now;
                        }
                    }

                    _repository.EditPhysicianAssessment(existingHistoryPhysical);

                    // handle allergies
                    if (model.AssessmentAllergies != null && model.AssessmentAllergies.Any())
                    {
                        // gather existing PhysicianAssessmentAllergies
                        var existing = _repository.PhysicianAssessmentAllergies.Where(x => x.Type == "Allergy").ToList();

                        foreach (var dto in model.AssessmentAllergies)
                        {
                            // check against existing;  if there, update it
                            var existToUpdate = existing.FirstOrDefault(x => x.PhysicianAssessmentAllergyId == dto.PhysicianAssessmentAllergyId);
                            if(existToUpdate != null)
                            {
                                existToUpdate.Description = dto.Description;
                                existToUpdate.SortOrder = dto.SortOrder;
                                existToUpdate.Type = dto.Type;

                                _repository.EditPhysicianAssessmentAllergy(existToUpdate);
                            }
                            else //doesn't exist so create and add
                            {
                                var paAllergy = new PhysicianAssessmentAllergy
                                {
                                    PhysicianAssessmentId = existingHistoryPhysical.PhysicianAssessmentId,
                                    Description = dto.Description?.Trim(),
                                    SortOrder = dto.SortOrder,
                                    Type = dto.Type
                                };
                                _repository.AddPhysicianAssessmentAllergy(paAllergy);
                            }
                        }
                    }
                    else
                    {
                        var existingToDelete = _repository.PhysicianAssessmentAllergies.Where(x => x.Type == "Allergy").ToList();
                        if(existingToDelete != null)
                        {
                            _repository.DeletePhysicianAssessmentAllergy(existingToDelete);
                        }
                    }

                    // handle medications
                    if (model.AssessmentMedications != null && model.AssessmentMedications.Any())
                    {
                        // gather existing PhysicianAssessmentAllergies
                        var existing = _repository.PhysicianAssessmentAllergies.Where(x => x.Type == "Medication").ToList();

                        foreach (var dto in model.AssessmentMedications)
                        {
                            // check against existing;  if there, update it
                            var existToUpdate = existing.FirstOrDefault(x => x.PhysicianAssessmentAllergyId == dto.PhysicianAssessmentAllergyId);
                            if(existToUpdate != null)
                            {
                                existToUpdate.Description = dto.Description;
                                existToUpdate.SortOrder = dto.SortOrder;
                                existToUpdate.Type = dto.Type;

                                _repository.EditPhysicianAssessmentAllergy(existToUpdate);
                            }
                            else //doesn't exist so create and add
                            {
                                var paMed = new PhysicianAssessmentAllergy
                                {
                                    PhysicianAssessmentId = existingHistoryPhysical.PhysicianAssessmentId,
                                    Description = dto.Description?.Trim(),
                                    SortOrder = dto.SortOrder,
                                    Type = dto.Type
                                };
                            _repository.AddPhysicianAssessmentAllergy(paMed);
                            }
                        }
                    }
                    else
                    {
                        var existingToDelete = _repository.PhysicianAssessmentAllergies.Where(x => x.Type == "Medication").ToList();
                        if(existingToDelete != null)
                        {
                            _repository.DeletePhysicianAssessmentAllergy(existingToDelete);
                        }
                        
                    }

                    tran.Complete();
                }
            }
            
            // return to assessment view
            return RedirectToAction("ViewHistoryAndPhysical", new { id = model.PhysicianAssessment.EncounterId });
        }

        /// <summary>
        /// Preprocessing to the PhysicianAssessment view model
        /// Also load systems exam types /systems review info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private PhysicianAssessmentViewModel PrepareHistoryPhysicalViewModel(Encounter encounter, PhysicianAssessmentViewModel model = null)
        {
            if (model is null)
                model = new PhysicianAssessmentViewModel(encounter);

            // Checks for less than 100 to avoid junk/testing types
            model.SystemExamTypes = _repository.ExamTypes
                .Where(et => et.ExamTypeCode < 100)
                .Include(et => et.BodySystemTypes)
                .ToList();

            // Ensure BodySystemAssessments is a List and is initialized
            if (model.BodySystemAssessments == null)
                model.BodySystemAssessments = new List<BodySystemAssessment>();

            // Build a deterministic list in the same sequence the view will render.
            var bsaList = new List<BodySystemAssessment>();

            foreach (var et in model.SystemExamTypes)
            {
                foreach (var bst in et.BodySystemTypes)
                {
                    // Try to find an existing entry in the supplied model (preserve posted values)
                    var existing = model.BodySystemAssessments.FirstOrDefault(x => x.BodySystemTypeId == bst.BodySystemTypeId);

                    if (existing != null)
                    {
                        // Ensure comment not null to simplify client-side logic
                        if (existing.Comment == null) existing.Comment = string.Empty;

                        // Defensive: make sure BodySystemTypeId is correct
                        existing.BodySystemTypeId = bst.BodySystemTypeId;

                        bsaList.Add(existing);
                    }
                    else
                    {
                        // Add placeholder so index/order is consistent for the view
                        bsaList.Add(new BodySystemAssessment
                        {
                            BodySystemTypeId = bst.BodySystemTypeId,
                            IsWithinNormalLimits = null,
                            Comment = string.Empty
                        });
                    }
                }
            }

            // Replace model list with the rebuilt ordered list
            model.BodySystemAssessments = bsaList;

            // initialize the assessmentAllergies and assessmentMedications and set the flags for the view
            var assessmentAllergies = new List<PhysicianAssessmentAllergyDto>();
            ViewBag.allergiesExist = false;

            var assessmentMedications =  new List<PhysicianAssessmentAllergyDto>();
            ViewBag.medicationsExist = false;

            model.AssessmentAllergies = assessmentAllergies;
            model.AssessmentMedications = assessmentMedications;
            
            return model;
        }

        /// <summary>
        ///     AJAX call from buttons in UpdateHistoryAndPhysical to display PatientAllergies snapshot in the H & P
        /// </summary>
        /// <param name="paId"></param>
        /// <param name="mrn"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FetchAssessmentAllergies(int paId, string mrn, string action = null)
        {
            // paId may be 0 for new assessment
            List<PhysicianAssessmentAllergyDto> dtoList;

            if (paId > 0)
            {
                // assessment exists but want to refresh the snapshot;  delete existing first then fetch new
                if (string.Equals(action, "repopulate", StringComparison.OrdinalIgnoreCase))
                {
                    await _repository.PhysicianAssessmentAllergies
                        .Where(x => x.PhysicianAssessmentId == paId && x.Type == "Allergy")
                        .ExecuteDeleteAsync();
                }

                // now populate snapshot, either because it was an existing assessment but never populated or for snapshot refresh
                dtoList = await _allergenService.GetPatientAllergiesAsync(mrn);

            }
            else
            {
                // New assessment: fetch patient allergies
                dtoList = await _allergenService.GetPatientAllergiesAsync(mrn);
            }

            // Return partial that renders visible list + hidden inputs for form binding
            return PartialView("_AssessmentAllergiesListPartial", dtoList);
        }

        /// <summary>
        ///     AJAX call from buttons in UpdateHistoryAndPhysical to display PatientMedicationList snapshot in the H & P
        /// </summary>
        /// <param name="paId"></param>
        /// <param name="mrn"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FetchAssessmentMedications(int paId, string mrn, string action)
        {
            List<PhysicianAssessmentAllergyDto> dtoList;

            if (paId > 0)
            {
                // assessment exists but want to refresh the snapshot;  delete existing first then fetch new
                if (string.Equals(action, "repopulateMeds", StringComparison.OrdinalIgnoreCase))
                {
                    await _repository.PhysicianAssessmentAllergies
                        .Where(x => x.PhysicianAssessmentId == paId && x.Type == "Medication")
                        .ExecuteDeleteAsync();
                }

                // now populate snapshot, either because it was an existing assessment but never populated or for snapshot refresh
                dtoList = await _medicationService.GetPatientMedicationsAsync(mrn);

            }
            else
            {
                // New assessment: fetch patient medications
                dtoList = await _medicationService.GetPatientMedicationsAsync(mrn);
            }

            return PartialView("_AssessmentMedicationsListPartial", dtoList);
        }
 
    }
}
