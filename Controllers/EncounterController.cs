using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Transactions;
using System.Threading.Tasks;   // needed for async Task<> methods
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ProviderType = IS_Proj_HIT.Entities.Enum.ProviderType;
using NuGet.Protocol;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("EncounterAdd","EncounterDelete","EncounterEdit","EncounterView", "EncounterDischarge","ProgressNotesAdd","ProgressNotesEdit","ProgressNotesDelete","ProgressNotesView","DischargeSummaryAdd","DischargeSummaryEdit","DischargeSummaryDelete","DischargeSummaryView","PCAView")]
    public class EncounterController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;
        
        public int PageSize = 8;
        public EncounterController(IWCTCHealthSystemRepository repo, UserManager<IdentityUser> userManager) 
        {
            _repository = repo;
            _userManager = userManager;
        } 

        /// <summary>
        /// Loads checked in encounters screen, filters by facility
        /// </summary>
        // Used in: Navbar (_Layout) and Home Page, ViewDischarge, ViewEncounter (if patient is still checked in?)
        [Authorize]
        [PermissionAuthorize("EncounterView")]
        public ViewResult CheckedIn()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var currentUser = _repository.UserTables.FirstOrDefault(u => u.Email == User.Identity.Name);

                // Retrieve FacilityId from the session
                var facilityIdString = HttpContext.Session.GetString("Facility");
                if (string.IsNullOrEmpty(facilityIdString))
                {
                    // Handle the case where Facility is not set in the session
                    ViewData["ErrorMessage"] = "From EncounterController CheckedIn Method:  You do not currently have an assigned facility.";
                    return View("~/Views/Home/Index.cshtml");
                }
                int facilityId = int.Parse(facilityIdString);
                
            var currentUserFacility = _repository.Facilities.FirstOrDefault(f => f.FacilityId == facilityId);
            var facilities = _repository.Facilities;
            
            var model = _repository.Encounters
                .Where(e => e.DischargeDateTime == null)
                .OrderByDescending(e => e.AdmitDateTime)
                .Join(_repository.Patients,
                    e => e.Mrn,
                    p => p.Mrn,
                    (e, p) =>
                        new EncounterPatientViewModel(e.Mrn,
                            e.EncounterId,
                            e.AdmitDateTime,
                            p.FirstName,
                            p.LastName,
                            e.Facility.Name,
                            e.DischargeDateTime.ToString(),
                            e.RoomNumber));
 
            if(!permissions.Contains("SecurityAllFacilities") && currentUserFacility == null)
            {
                model = _repository.Encounters
                .Where(e => e.DischargeDateTime == null && (e.FacilityId == 0))
                .OrderByDescending(e => e.AdmitDateTime)
                .Join(_repository.Patients,
                    e => e.Mrn,
                    p => p.Mrn,
                    (e, p) =>
                        new EncounterPatientViewModel(e.Mrn,
                            e.EncounterId,
                            e.AdmitDateTime,
                            p.FirstName,
                            p.LastName,
                            e.Facility.Name,
                            e.DischargeDateTime.ToString(),
                            e.RoomNumber));

                ViewData["ErrorMessage"] = "From EncounterController CheckedIn Method:  You do not currently have an assigned facility.";
                return View("~/Views/Home/Index.cshtml");
            }

            if(!permissions.Contains("SecurityAllFacilities") && currentUserFacility != null)
            {
                model = _repository.Encounters
                .Where(e => e.DischargeDateTime == null && e.FacilityId == currentUserFacility.FacilityId)
                .OrderByDescending(e => e.AdmitDateTime)
                .Join(_repository.Patients,
                    e => e.Mrn,
                    p => p.Mrn,
                    (e, p) =>
                        new EncounterPatientViewModel(e.Mrn,
                            e.EncounterId,
                            e.AdmitDateTime,
                            p.FirstName,
                            p.LastName,
                            e.Facility.Name,
                            e.DischargeDateTime.ToString(),
                            e.RoomNumber));
            }

            return View(model);
        }

        #region Progress Notes
        /// <summary>
        /// View ProgressNotes page
        /// </summary>
        /// <param name="id">Id of unique encounter</param>
        // Used in: EncounterMenu
        [Authorize]
        [PermissionAuthorize("ProgressNotesView")]
        public IActionResult ProgressNotes(long id, string sortOrder)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var desiredEncounter = _repository.Encounters.FirstOrDefault(u => u.EncounterId == id);

            var desiredPatient = _repository.Patients.FirstOrDefault(u => u.Mrn == desiredEncounter.Mrn);
            
            ViewBag.EncounterId = id;
            // This is how alerts connects
            ViewBag.Patient = _repository.Patients
            .Include(p => p.PatientAlerts)
            .FirstOrDefault(b => b.Mrn == desiredEncounter.Mrn);

            //sortable List
            ViewBag.PhysicianSortParm = sortOrder == "byPhysician" ? "byPhysicianDesc" : "byPhysician";
            ViewBag.CoPhysicianSortParm = sortOrder == "byCoPhysician" ? "byCoPhysicianDesc" : "byCoPhysician";
            ViewBag.DateSortParm = sortOrder == "byDate" ? "byDateDesc" : "byDate";
            ViewBag.NoteTypeSortParm = sortOrder == "byNoteType" ? "byNoteTypeDesc"   : "byNoteType";

            if (sortOrder == "byNoteType" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by Note Type Ascending";
            }
            else if (sortOrder == "byNoteTypeDesc" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by Note Type Descending";
            }
            else if (sortOrder == "byDate" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by Date Ascending";
            }

            else if (sortOrder == "byDateDesc" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by Date Descending";
            }
            else if (sortOrder == "byPhysician" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by Physician Ascending";
            }

            else if (sortOrder == "byPhysicianDesc" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by Physician Descending";
            }
            else if (sortOrder == "byCoPhysician" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by CoPhysician Ascending";
            }

            else if (sortOrder == "byCoPhysicianDesc" && _repository.ProgressNotes.Where(b => b.EncounterId == id).Count() > 0)
            {
                TempData["msg"] = "Sort order is by CoPhysician Descending";
            }

            else
            {
                TempData["msg"] = "";
            }
            //takes the sort order and plugs in the correct sort into the model
            if(sortOrder == "byDate"){
                sortOrder= "";
                var desiredProgressNotes = _repository.ProgressNotes
                    .Include(e => e.NoteType)
                    .Include(e => e.Physician)
                    .Include(e => e.CoPhysician)
                    .Where(u => u.EncounterId == id)
                    .OrderBy(p => p.LastUpdated);
                var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            }
            if(sortOrder == "byDateDesc"){
                sortOrder= "";
                var desiredProgressNotes = _repository.ProgressNotes
                    .Include(e => e.NoteType)
                    .Include(e => e.Physician)
                    .Include(e => e.CoPhysician)
                    .Where(u => u.EncounterId == id)
                    .OrderByDescending(p => p.LastUpdated);
                var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            }

            if(sortOrder == "byPhysician"){
                sortOrder= "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderBy(p => p.Physician.LastName);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            }
            if(sortOrder == "byPhysicianDesc"){
                sortOrder= "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderByDescending(p => p.Physician.LastName);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            }

            if(sortOrder == "byCoPhysician"){
                sortOrder= "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderBy(p => p.CoPhysician.LastName);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            }
            if(sortOrder == "byCoPhysicianDesc"){
                sortOrder= "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderByDescending(p => p.CoPhysician.LastName);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            }
            
            if(sortOrder == "byNoteType"){
                sortOrder = "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderBy(p => p.NoteType);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            } 

            if(sortOrder == "byNoteTypeDesc"){
                sortOrder = "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderByDescending(p => p.NoteType);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model);
            } else{
                    sortOrder= "";
                    var desiredProgressNotes = _repository.ProgressNotes
                        .Include(e => e.NoteType)
                        .Include(e => e.Physician)
                        .Include(e => e.CoPhysician)
                        .Where(u => u.EncounterId == id)
                        .OrderByDescending(p => p.LastUpdated);
                    var model = new ViewEncounterPageModel(desiredEncounter, desiredPatient, desiredProgressNotes);
                return View(model); 
            }
            
        }

        /// <summary>
        /// View a specific ProgressNote page
        ///</summary>
        /// <param name="id">Id of unique encounter</param>
        // Used in: ProgressNote
        [Authorize]
        [PermissionAuthorize("ProgressNotesView")] 
        public IActionResult ViewProgressNote(long id, long pId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var desiredEncounter = _repository.Encounters.FirstOrDefault(u => u.EncounterId == id);

            var desiredPatient = _repository.Patients.Include(p => p.PatientAlerts).FirstOrDefault(u => u.Mrn == desiredEncounter.Mrn);

            ViewBag.StartDate = _repository.ProgressNotes.FirstOrDefault(p => p.ProgressNoteId == pId).WrittenDate;

            var desiredProgressNote = _repository.ProgressNotes
                .Include(e => e.NoteType)
                .Include(e => e.Physician)
                .Include(e => e.CoPhysician)
                .Where(u => u.EncounterId == id)
                .FirstOrDefault(u => u.ProgressNoteId == pId);

            ViewBag.Encounter = desiredEncounter;
            ViewBag.Patient = desiredPatient;

            var model = new ProgressNotesViewModel(desiredEncounter, desiredPatient, desiredProgressNote);

            return View(model);
        }

        /// <summary>
        /// View AddProgressNotes page
        /// </summary>
        /// <param name="id">Id of unique encounter</param>
        // Used in: ProgressNotes
        [Authorize]
        [PermissionAuthorize("ProgressNotesAdd")]
        public IActionResult AddProgressNotes(long id){
            
            ViewBag.EncounterId = id;

            var desiredEncounter = _repository.Encounters.FirstOrDefault(u => u.EncounterId == id);

            var desiredPatient = _repository.Patients.Include(p => p.PatientAlerts).FirstOrDefault(u => u.Mrn == desiredEncounter.Mrn);

            ViewBag.Patient = desiredPatient;
            ViewBag.Encounter = desiredEncounter;

            var queryProvider = _repository.Physicians
                    .Include(n => n.Specialty)
                    .OrderBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                    .ToList();

            ViewBag.Providers = new SelectList(queryProvider,  "PhysicianId", "FullName",  0);

            var queryCosigner = _repository.Physicians
                    .Where(p => p.ProviderStatusId == 1)
                    .Include(p => p.Specialty)
                    .OrderBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                    .ToList();

            ViewBag.Cosigners = new SelectList(queryCosigner,  "PhysicianId", "FullName",  0);
            
            var queryNoteType = _repository.NoteTypes
                    .OrderBy(n => n.NoteTypeId)
                    .Select(n => new {n.NoteTypeId, n.NoteType1})
                    .ToList();

            ViewBag.NoteTypes = new SelectList(queryNoteType, "NoteTypeId", "NoteType1", 0);

            var model = new ProgressNotesViewModel();

            model.Encounter = desiredEncounter;
            model.Patient = desiredPatient;
            
            return View(model);
        }

        /// <summary>
        /// Create new progress note
        /// </summary>
        /// <param name="model">ProgressNotes model to be added to database</param>
        // Used in: AddProgressNotes
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("ProgressNotesAdd")]
        public IActionResult AddProgressNotes(ProgressNote model)
        {
            if (_repository.ProgressNotes.Any(p => p.ProgressNoteId == model.ProgressNoteId))
            {
                ModelState.AddModelError("", "Progress Note Id must be unique");
            }
            
            if(ModelState.IsValid) {
                var editedBy = UserHelper.GetEditedBy(_repository, _userManager, User);
                
                model.LastUpdated = DateTime.Now;
                model.EditedBy = editedBy;
                
                if(model.CoPhysicianId == 0)
                {
                    model.CoPhysicianId = null;
                }
            
                _repository.AddProgressNote(model);
                
                return RedirectToAction("ProgressNotes", new { id = model.EncounterId});
            }
            
            return View(model);
        }

         /// <summary>
        /// Deletes a progress Note, redirects to patients progress notes
        /// </summary>
        /// <param name="pId">Id of unique Progress note </param>
        /// <param name="id">Id of unique Encounter</param>
        // Used in: ProgressNotes
        [Authorize]
        [PermissionAuthorize("ProgressNotesDelete")]
        public IActionResult DeleteProgressNote(long id, long pId)
        {
            bool exists = _repository.ProgressNotes.Any(p => p.ProgressNoteId == pId);

            if(exists)
            {
                var progressnote = _repository.ProgressNotes.FirstOrDefault(p => p.ProgressNoteId == pId);
                _repository.DeleteProgressNote(progressnote);
            }
            return RedirectToAction("ProgressNotes", new { id = id});
        }    

        /// <summary>
        /// Shows the edit Progress Note page
        /// </summary>
        /// <param name="pId">Id of unique Progress note </param>
        /// <param name="id">Id of unique Encounter</param>
        [Authorize]
        [PermissionAuthorize("ProgressNotesEdit")]
        public IActionResult EditProgressNote(long id, long pId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            ViewBag.EncounterId = id;

            //passing the current progress note ID to the editprogressnote page
            ViewBag.ProgressNoteId = pId;

            var desiredEncounter = _repository.Encounters.FirstOrDefault(u => u.EncounterId == id);

            var desiredPatient = _repository.Patients.Include(p => p.PatientAlerts).FirstOrDefault(u => u.Mrn == desiredEncounter.Mrn);

            ViewBag.Patient = desiredPatient;
            ViewBag.Encounter = desiredEncounter;

            // grabbing the original written date and saving it
            var desiredWrittenDate = _repository.ProgressNotes.Where(u => u.EncounterId == id).FirstOrDefault(u => u.ProgressNoteId == pId).WrittenDate;
            
            
            ViewBag.WrittenDate = desiredWrittenDate;

            var desiredProgressNote = _repository.ProgressNotes
                .Include(e => e.NoteType)
                .Include(e => e.Physician)
                .Include(e => e.CoPhysician)
                .Where(u => u.EncounterId == id)
                .FirstOrDefault(u => u.ProgressNoteId == pId);

            ViewBag.ProgressNotes = desiredProgressNote;
            
            //Queries for the select lists
            var queryProvider = _repository.Physicians
                    .Include(n => n.Specialty)
                    .OrderBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                    .ToList();

            ViewBag.Providers = new SelectList(queryProvider,  "PhysicianId", "FullName",  0);

            var queryCosigner = _repository.Physicians
                    .Where(p => p.ProviderStatusId == 1)
                    .Include(p => p.Specialty)
                    .OrderBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                    .ToList();

            ViewBag.Cosigners = new SelectList(queryCosigner,  "PhysicianId", "FullName",  0);

            var queryNoteType = _repository.NoteTypes
                    .OrderBy(n => n.NoteTypeId)
                    .Select(n => new {n.NoteTypeId, n.NoteType1})
                    .ToList();

            ViewBag.NoteTypes = new SelectList(queryNoteType, "NoteTypeId", "NoteType1", 0);

            var model = new ProgressNotesViewModel(desiredEncounter, desiredPatient, desiredProgressNote);
            
            return View(model);
        }

        /// <summary>
        /// Edit a progress note, redirects to patients progress notes
        /// </summary>
        /// <param name="pId">Id of unique Progress note </param>
        /// <param name="id">Id of unique Encounter</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("ProgressNotesEdit")]
        public IActionResult EditProgressNotes(ProgressNote model)
        {
            var user = UserHelper.GetEditedBy(_repository, _userManager, User);

                if(model.CoPhysicianId == 0)
                {
                    model.CoPhysicianId = null;
                }

            model.EditedBy = user;
            
            if(!ModelState.IsValid) {

                return View(model);
            }
            
            _repository.EditProgressNote(model);

            return RedirectToAction("ProgressNotes", new { id = model.EncounterId});
        }
        #endregion // end of Progress Notes

        #region Encounters
        /// <summary>
        ///     View Discharge page displayed after an Encounter has been Discharged.
        ///         Is NOT the same view as the Encounter Discharge Information tab
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        [Authorize]
        [PermissionAuthorize("EncounterView", "EncounterDischarge")]
        public IActionResult ViewDischarge(long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;
            
            ViewData["ErrorMessage"] = "";

            var encounter = _repository.Encounters
                .Include(e => e.Facility)
                .Include(e => e.Department)
                .Include(e => e.AdmitType)
                .Include(e => e.EncounterType)
                .Include(e => e.PlaceOfService)
                .Include(e => e.PointOfOrigin)
                .Include(e => e.DischargeDispositionNavigation)
                .Include(e => e.Pcarecords)
                .FirstOrDefault(b => b.EncounterId == encounterId);
            if (encounter is null)
                return RedirectToAction("CheckedIn");

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);

            return View(new ViewEncounterPageModel
            {
                Encounter = encounter,
                Patient = patient
            });
        }

        /// <summary>
        /// View page of a specific encounter
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        // Used in: PCAController, CheckedIn, EditEncounter (to return to view), HistoryAndPhysical (currently unused), PatientDetails, View/Create/UpdatePCAAssessment
        [Authorize]
        [PermissionAuthorize("EncounterView", "EncounterDischarge")]
        public IActionResult ViewEncounter(long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;
            
            ViewData["ErrorMessage"] = "";

                // set a toggle for whether the encounter has been discharged
            ViewBag.IsEncounterDischarged = false;

            var id = User.Identity.Name;

            var encounter = _repository.Encounters
                .Include(e => e.Facility)
                .Include(e => e.Department)
                .Include(e => e.AdmitType)
                .Include(e => e.EncounterPhysicians)
                .ThenInclude(e => e.Physician)
                .Include(e => e.EncounterType)
                .Include(e => e.PlaceOfService)
                .Include(e => e.PointOfOrigin)
                .Include(e => e.DischargeDispositionNavigation)
                .Include(e => e.Pcarecords)
                .FirstOrDefault(b => b.EncounterId == encounterId);


            if (encounter is null)
                return RedirectToAction("CheckedIn");

            if (encounter.DischargeDateTime != null)
            {
                ViewBag.IsEncounterDischarged = true;
            }

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);

            return View(new ViewEncounterPageModel(encounter, patient));
        }

        /// <summary>
        /// View AddEncounter page
        /// </summary>
        /// <param name="id">Mrn of patient?</param>
        // Used in: PatientDetails
        [Authorize]
        [PermissionAuthorize("EncounterAdd")]
        public IActionResult AddEncounter(string id)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;
            
            //This exists to allow the Patient to be easily checked. 
            Patient patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .Include(p => p.Facility)
                .FirstOrDefault(b => b.Mrn == id);
            bool OpenEncounter = _repository.Encounters.Where(e => e.DischargeDateTime == null && e.Mrn == patient.Mrn).Any();

            ViewBag.Patient = patient;
            if(patient.DeceasedLiving == true) {
                return RedirectToAction("AddEncounterDeceasedError");

            }

            else if(OpenEncounter == true) {
                return RedirectToAction("AddEncounterOpenError");
            }

            AddDropdowns();
            EncounterFormViewModel model = new EncounterFormViewModel();
            model.patient = patient;
            model.FacilityId = patient.FacilityId;

            return View(model);
        }

        /// <summary>
        ///     Notify User the patient is deceased and a new encounter cannot be created
        ///     The 'Create New Encounter' button has conditionals affecting visibility when the patient is deceased but a direct url for AddEncounter would still need this code.  Future code could also perhaps add a new route to the 'AddEncounter' method which would also need this code.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("EncounterAdd")]
        public IActionResult AddEncounterDeceasedError() 
        { 
            return View(); 
        }
        
        /// <summary>
        ///     Notify User the patient has an open encounter and a new encounter cannot be created.
        ///     The 'Create New Encounter' button has conditionals affecting visibility when an open encounter exists but a direct url for AddEncounter would still need this code.  Future code could also perhaps add a new route to the 'AddEncounter' method which would also need this code.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("EncounterAdd")]
        public IActionResult AddEncounterOpenError() 
        { 
            return View(); 
        }

        /// <summary>
        ///     Delete Encounter:  Setter
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        // Used in: CheckedIn, ViewEncounter
        [Authorize]
        [PermissionAuthorize("EncounterDelete")]
        public IActionResult DeleteEncounter(long encounterId)
        {           
            using var tran = new TransactionScope();
            var encounter = _repository.Encounters.FirstOrDefault(b => b.EncounterId == encounterId);
            var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == encounter.Mrn);
            var documentsToDelete = _repository.Documents.Where(d => d.EncounterId == encounterId).ToList();
            var pa = _repository.PhysicianAssessments.FirstOrDefault(x => x.EncounterId == encounterId);
            var paAllergiesToDelete = _repository.PhysicianAssessmentAllergies.Where(y => y.PhysicianAssessmentId == pa.PhysicianAssessmentId).ToList();
            if (encounter == null)
            {
                return NotFound();
            }

            try
            {
                // Delete any/all Encounter Documents first
                foreach (var doc in documentsToDelete)
                {
                    _repository.DeleteDocument(doc);
                }

                // Delete any PhysicianAssessmentAllergies records
                _repository.DeletePhysicianAssessmentAllergy(paAllergiesToDelete);
                
                // Now delete the Encounter
                _repository.DeleteEncounter(encounter);

                // Commit transaction
                tran.Complete();
                TempData["Success"] = "Encounter successfully deleted.";
                return RedirectToAction("ViewPatientDetails", "Patient", new {id = patient.Mrn, isModal = false});
            }
            catch (Exception ex)
            {
                // Roll back transaction in case of errors
                tran.Dispose();
                TempData["Error"] = $"An exception occurred:  {ex.Message}";
                return RedirectToAction("ViewPatientDetails", "Patient", new {id = patient.Mrn, isModal = false});
            }
        }

        /// <summary>
        /// View EditEncounter page
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        // Used in: CheckedIn, ViewDischarge, ViewEncounter
        [Authorize]
        [PermissionAuthorize("EncounterEdit")]
        public IActionResult EditEncounter(long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

                // set a toggle for whether the encounter has been discharged
            ViewBag.IsEncounterDischarged = false;
            
            var encounter = _repository.Encounters
                .FirstOrDefault(b => b.EncounterId == encounterId);
            ViewBag.Patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .Include(p => p.Facility)
                .FirstOrDefault(b => b.Mrn == encounter.Mrn);
            
                // set the toggle, used to control display of buttons in the view
            if (encounter.DischargeDateTime != null)
            {
                ViewBag.IsEncounterDischarged = true;
            }

            AddDropdowns();

            EncounterFormViewModel model = new EncounterFormViewModel();
            model.encounter = encounter;
            model.EncounterId = encounterId;
            model.FacilityRegistryOptInOut = encounter.FacilityRegistryOptInOut;
            model.ChiefComplaint = encounter.ChiefComplaint;
            model.Mrn = encounter.Mrn;
            model.PatientCurrentAge = encounter.PatientCurrentAge;
            model.FacilityId = encounter.FacilityId;
            model.DepartmentId = encounter.DepartmentId;
            model.RoomNumber = encounter.RoomNumber;
            model.PointOfOriginId = encounter.PointOfOriginId;
            model.PlaceOfServiceId = encounter.PlaceOfServiceId;
            model.AdmitDateTime = encounter.AdmitDateTime;
            model.AdmitTypeId = encounter.AdmitTypeId;
            model.EncounterTypeId = encounter.EncounterTypeId;

            model.DischargeDateTime = encounter.DischargeDateTime;
            model.DischargeDisposition = encounter.DischargeDisposition;
            model.DischargeComment = encounter.DischargeComment;

            EncounterPhysician admittingProvider = _repository.EncounterPhysicians.FirstOrDefault(p => p.EncounterId == encounterId && p.PhysicianRoleId == 3);
            if(admittingProvider != null){
                model.admittingProviderId = admittingProvider.PhysicianId;
            }
            EncounterPhysician emergencyProvider = _repository.EncounterPhysicians.FirstOrDefault(p => p.EncounterId == encounterId && p.PhysicianRoleId == 4);
            if(emergencyProvider != null){
                model.emergencyProviderId = emergencyProvider.PhysicianId;
            }
            EncounterPhysician attendingProvider = _repository.EncounterPhysicians.FirstOrDefault(p => p.EncounterId == encounterId && p.PhysicianRoleId == 1);
            if(attendingProvider != null){
                model.attendingProviderId = attendingProvider.PhysicianId;
            }
            EncounterPhysician primaryProvider = _repository.EncounterPhysicians.FirstOrDefault(p => p.EncounterId == encounterId && p.PhysicianRoleId == 12);
            if(primaryProvider != null){
                model.primaryProviderId = primaryProvider.PhysicianId;
            }
            EncounterPhysician refferingProvider = _repository.EncounterPhysicians.FirstOrDefault(p => p.EncounterId == encounterId && p.PhysicianRoleId == 2);
            if(refferingProvider != null){
                model.referringProviderId = refferingProvider.PhysicianId;
            }

            return View(model);
        }

        [Authorize]
        [PermissionAuthorize("EncounterDischarge")]
        public IActionResult DischargeEncounter(long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;
            
            var encounter = _repository.Encounters
                .FirstOrDefault(b => b.EncounterId == encounterId);
            ViewBag.Patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(b => b.Mrn == encounter.Mrn);

                AddDropdowns();
            
            return View(encounter);
        }

        /// <summary>
        /// Create new encounter
        /// </summary>
        /// <param name="model">Encounter model to be added to database</param>
        // Used in: AddEncounter
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("EncounterAdd")]
        public IActionResult AddEncounter(EncounterFormViewModel model)
        {
            Encounter encounter = new Encounter();
            if (_repository.Encounters.Any(p => p.EncounterId == encounter.EncounterId))
            {
                ModelState.AddModelError("", "Encounter Id must be unique");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Patient = _repository.Patients
                    .Include(p => p.PatientAlerts)
                    .Include(p => p.Facility)
                    .FirstOrDefault(b => b.Mrn == model.Mrn);
                AddDropdowns();

                return View(model);
            }

            var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == model.Mrn);

            EncounterPhysician emergencyProvider = new EncounterPhysician();
            EncounterPhysician admittingProvider = new EncounterPhysician();
            EncounterPhysician attendingProvider = new EncounterPhysician();
            EncounterPhysician primaryProvider = new EncounterPhysician();
            EncounterPhysician referringProvider = new EncounterPhysician();

            encounter.FacilityRegistryOptInOut = model.FacilityRegistryOptInOut;
            encounter.ChiefComplaint = model.ChiefComplaint;
            encounter.Mrn = model.Mrn;
            encounter.PatientCurrentAge = model.PatientCurrentAge;
            encounter.FacilityId = patient.FacilityId;
            encounter.DepartmentId = model.DepartmentId;
            encounter.RoomNumber = model.RoomNumber;
            encounter.PointOfOriginId = model.PointOfOriginId;
            encounter.PlaceOfServiceId = model.PlaceOfServiceId;
            encounter.AdmitTypeId = model.AdmitTypeId;
            encounter.EncounterTypeId = model.EncounterTypeId;
            
            //TODO: use the physician role enum here
            if(model.emergencyProviderId!= null){
                emergencyProvider.PhysicianRoleId = 4;
                emergencyProvider.PhysicianId = (int) model.emergencyProviderId;
                emergencyProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(emergencyProvider);
            }

            if(model.admittingProviderId != null){
                admittingProvider.PhysicianRoleId = 3;
                admittingProvider.PhysicianId = (int) model.admittingProviderId;
                admittingProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(admittingProvider);
            }

            if(model.attendingProviderId != null){
                attendingProvider.PhysicianRoleId = 1;
                attendingProvider.PhysicianId = (int) model.attendingProviderId;
                attendingProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(attendingProvider);
            }

            if(model.primaryProviderId != null){
                primaryProvider.PhysicianRoleId = 12;
                primaryProvider.PhysicianId = (int) model.primaryProviderId;
                primaryProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(primaryProvider);
            }

            if(model.referringProviderId != null){
                referringProvider.PhysicianRoleId = 2;
                referringProvider.PhysicianId = (int) model.referringProviderId;
                referringProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(referringProvider);
            }

            encounter.AdmitDateTime = DateTime.Now;
            encounter.LastModified = DateTime.Now;
            
            _repository.AddEncounter(encounter);

            return RedirectToAction("ViewEncounter", "Encounter", new {encounterId = encounter.EncounterId});
        }

        /// <summary>
        /// Save edits to patient record from Edit Patients page
        /// </summary>
        /// <param name="model">Encounter model to be edited</param>
        // Used in: EditEncounter
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("EncounterEdit")]
        public IActionResult EditEncounter(EncounterFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patient = _repository.Patients
                    .Include(p => p.PatientAlerts)
                    .Include(p => p.Facility)
                    .FirstOrDefault(b => b.Mrn == model.Mrn);

                AddDropdowns();

                return View(model);
            }

            var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == model.Mrn);

            Encounter encounter = _repository.Encounters.FirstOrDefault(e => e.EncounterId == model.EncounterId);
            //Add encounter physicians here
            EncounterPhysician emergencyProvider = _repository.EncounterPhysicians.FirstOrDefault(e => e.EncounterPhysiciansId == model.emergencyProviderId);
            EncounterPhysician admittingProvider = _repository.EncounterPhysicians.FirstOrDefault(e => e.EncounterPhysiciansId == model.admittingProviderId);
            EncounterPhysician attendingProvider = _repository.EncounterPhysicians.FirstOrDefault(e => e.EncounterPhysiciansId == model.attendingProviderId);
            EncounterPhysician primaryProvider = _repository.EncounterPhysicians.FirstOrDefault(e => e.EncounterPhysiciansId == model.primaryProviderId);
            EncounterPhysician referringProvider = _repository.EncounterPhysicians.FirstOrDefault(e => e.EncounterPhysiciansId == model.referringProviderId);

            encounter.FacilityRegistryOptInOut = model.FacilityRegistryOptInOut;
            encounter.ChiefComplaint = model.ChiefComplaint;
            encounter.Mrn = model.Mrn;
            encounter.PatientCurrentAge = model.PatientCurrentAge;
            encounter.FacilityId = patient.FacilityId;
            encounter.DepartmentId = model.DepartmentId;
            encounter.RoomNumber = model.RoomNumber;
            encounter.PointOfOriginId = model.PointOfOriginId;
            encounter.PlaceOfServiceId = model.PlaceOfServiceId;
            encounter.AdmitDateTime = model.AdmitDateTime;
            encounter.AdmitTypeId = model.AdmitTypeId;
            encounter.EncounterTypeId = model.EncounterTypeId;
            encounter.DischargeDateTime = model.DischargeDateTime;
            encounter.DischargeDisposition = model.DischargeDisposition;
            encounter.DischargeComment = model.DischargeComment;
            
            if(model.emergencyProviderId!= null){
                if(emergencyProvider== null){
                    emergencyProvider = new EncounterPhysician();
                }
                emergencyProvider.PhysicianRoleId = 4;
                emergencyProvider.PhysicianId = (int) model.emergencyProviderId;
                emergencyProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(emergencyProvider);
            }

            if(model.admittingProviderId != null){
                if(admittingProvider== null){
                    admittingProvider = new EncounterPhysician();
                }
                admittingProvider.PhysicianRoleId = 3;
                admittingProvider.PhysicianId = (int) model.admittingProviderId;
                admittingProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(admittingProvider);
            }

            if(model.attendingProviderId != null){
                if(attendingProvider== null){
                    attendingProvider = new EncounterPhysician();
                }
                attendingProvider.PhysicianRoleId = 1;
                attendingProvider.PhysicianId = (int) model.attendingProviderId;
                attendingProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(attendingProvider);
            }

            if(model.primaryProviderId != null){
                if(primaryProvider== null){
                    primaryProvider = new EncounterPhysician();
                }
                primaryProvider.PhysicianRoleId = 12;
                primaryProvider.PhysicianId = (int) model.primaryProviderId;
                primaryProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(primaryProvider);
            }

            if(model.referringProviderId != null){
                if(referringProvider== null){
                    referringProvider = new EncounterPhysician();
                }
                referringProvider.PhysicianRoleId = 2;
                referringProvider.PhysicianId = (int) model.referringProviderId;
                referringProvider.EncounterId = encounter.EncounterId;
                encounter.EncounterPhysicians.Add(referringProvider);
            }

            encounter.LastModified = DateTime.Now;
            _repository.EditEncounter(encounter);
            return RedirectToAction("ViewEncounter",
                new {encounterId = model.EncounterId, allowCheckedInRedirect = true});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("EncounterDischarge")]
        public IActionResult DischargeEncounter(Encounter model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patient = _repository.Patients
                    .Include(p => p.PatientAlerts)
                    .FirstOrDefault(b => b.Mrn == model.Mrn);

                AddDropdowns();

                return View(model);
            }

            Encounter tempEncounter = _repository.Encounters.FirstOrDefault(e=> e.EncounterId == model.EncounterId);
            tempEncounter.DischargeDateTime=model.DischargeDateTime;
            tempEncounter.DischargeComment= model.DischargeComment;
            tempEncounter.DischargeDisposition = model.DischargeDisposition;

            tempEncounter.LastModified = DateTime.Now;
            _repository.EditEncounter(tempEncounter);
            return RedirectToAction("ViewDischarge",
                new {encounterId = tempEncounter.EncounterId, allowCheckedInRedirect = true});
        }
        #endregion // end of Encounter section

        #region Dropdowns
        /// <summary>
        /// Add dropdowns to encounter views, only user's facility(ies) can be selected. Controller method to display dropdowns
        /// </summary>
        private void AddDropdowns()
        {
            var queryAdmitTypes = _repository.AdmitTypes
                .OrderBy(n => n.Description)
                .Select(n => new {n.AdmitTypeId, n.Description})
                .ToList();
            ViewBag.AdmitTypes = new SelectList(queryAdmitTypes, "AdmitTypeId", "Description", 0);

            var queryDepartments = _repository.Departments
                .OrderBy(n => n.Name)
                .Select(dep => new {dep.DepartmentId, dep.Name})
                .ToList();
            ViewBag.Departments = new SelectList(queryDepartments, "DepartmentId", "Name", 0);
            
            var queryDischarges = _repository.Discharges
                .OrderBy(n => n.Name)
                .Select(dis => new {dis.DischargeId, dis.Name})
                .ToList();
            ViewBag.Discharges = new SelectList(queryDischarges, "DischargeId", "Name", 0);

            var queryEncounterTypes = _repository.EncounterTypes
                .OrderBy(n => n.Name)
                .Select(ent => new {ent.EncounterTypeId, ent.Name})
                .ToList();
            ViewBag.EncounterTypes = new SelectList(queryEncounterTypes, "EncounterTypeId", "Name", 0);

            var queryPlacesOfService = _repository.PlaceOfService
                .OrderBy(n => n.Description)
                .Select(pos => new {pos.PlaceOfServiceId, pos.Description})
                .ToList();
            ViewBag.PlacesOfService = new SelectList(queryPlacesOfService, "PlaceOfServiceId", "Description", 0);

            var queryPointsOfOrigin = _repository.PointOfOrigin
                .OrderBy(n => n.Description)
                .Select(poo => new {poo.PointOfOriginId, poo.Description})
                .ToList();
            ViewBag.PointsOfOrigin = new SelectList(queryPointsOfOrigin, "PointOfOriginId", "Description", 0);

            var currentUser = _repository.UserTables.FirstOrDefault(u => u.Email == User.Identity.Name);
            var currentUserFacility = _repository.UserFacilities.FirstOrDefault(e => e.UserId == currentUser.UserId);

             // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            var queryFacility = _repository.Facilities
                    .OrderBy(n => n.Name)
                    .Select(fac => new {fac.FacilityId, fac.Name})
                    .ToList();
            var facilities = _repository.Facilities;

            if (!permissions.Contains("SecurityAllFacilities") && currentUserFacility == null) {
                queryFacility = _repository.Facilities
                    .Where(e => e.FacilityId == 0)
                    .OrderBy(n => n.Name)
                    .Select(fac => new {fac.FacilityId, fac.Name})
                    .ToList();

                ViewBag.ErrorMessage = "You do not currently have an assigned facility.";
            }

            if (!permissions.Contains("SecurityAllFacilities") && currentUserFacility != null) {
                queryFacility = _repository.Facilities
                    .Where(e => e.FacilityId == currentUserFacility.FacilityId)
                    .OrderBy(n => n.Name)
                    .Select(fac => new {fac.FacilityId, fac.Name})
                    .ToList();
            }

            ViewBag.Facility = new SelectList(queryFacility, "FacilityId", "Name", 0);

            var queryPhysicians = _repository.Physicians
                .Include(n => n.Specialty)
                .OrderBy(n => n.LastName)
                .ThenBy(n=> n.FirstName)
                .Select(p => new {p.PhysicianId, Name = $"{p.LastName}, {p.FirstName} ({p.Specialty.Name})"})
                .ToList();
            ViewBag.Physician = new SelectList(queryPhysicians, "PhysicianId", "Name", 0);

        }

        /// <summary>
        ///     JsonResult method which dynamically populates the select list for PlaceOfService depending upon the User selection for EncounterType
        ///         Used in AddEncounter.cshtml and EditEncounter.cshtml, called via <script> 'fetch'
        /// </summary>
        /// <param name="encounterTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("EncounterAdd", "EncounterEdit")]
        public JsonResult GetPlacesOfService(int encounterTypeId)
        {
            var query = _repository.PlaceOfService
                .OrderBy(n => n.Description)
                .Select(pos => new { pos.PlaceOfServiceId, pos.Description });

            if (encounterTypeId == 1) // INPATIENT
            {
                query = query.Where(p => p.PlaceOfServiceId == 1002);
            }
            else if (encounterTypeId == 2) // OUTPATIENT
            {
                query = query.Where(p => p.PlaceOfServiceId != 1002);
            }

            return Json(query.ToList());
        }

        /// <summary>
        ///     AJAX call from the AddEncounter.cshtml and EditEncounter.cshtml which verifies the Room Number entered is not used in any active Encounter
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("EncounterAdd", "EncounterEdit")]
        public async Task<IActionResult> ValidateRoomNumber(string roomNumber, int? encounterId)
        {
            // Retrieve FacilityId from the session
            var facilityIdString = HttpContext.Session.GetString("Facility");
            if (string.IsNullOrEmpty(facilityIdString))
            {
                // Handle the case where Facility is not set in the session
                return Unauthorized("Facility not set for the session.");
            }
            int facilityId = int.Parse(facilityIdString);

            // check against Room Numbers in the Users Facility and only active Encounters (discharged Encounter Room numbers may be re-used)
            bool inUse = await _repository.Encounters
                .AnyAsync(e =>
                    e.RoomNumber == roomNumber
                    && e.FacilityId == facilityId
                    && e.DischargeDateTime == null
                    && (!encounterId.HasValue || e.EncounterId != encounterId.Value)
                );

            return Json(new { isValid = !inUse });
        }

        #endregion // end of Dropdowns

        #region PCA Index
        /// <summary>
        /// View PCAIndex page from Encounter Menu
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        [Authorize]
        [PermissionAuthorize("PCAView")]
        public IActionResult PCAIndex(long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            ViewData["ErrorMessage"] = "";

            var id = User.Identity.Name;

            var encounter = _repository.Encounters
                .Include(e => e.Facility)
                .Include(e => e.Department)
                .Include(e => e.AdmitType)
                // .Include(e => e.EncounterPhysicians.Physician)
                .Include(e => e.EncounterType)
                .Include(e => e.PlaceOfService)
                .Include(e => e.PointOfOrigin)
                .Include(e => e.DischargeDispositionNavigation)
                .Include(e => e.Pcarecords)
                .FirstOrDefault(b => b.EncounterId == encounterId);
            if (encounter is null) {
                ViewData["ErrorMessage"] = "No PCA to display.";
                //Issue is that it's looking for the "Error" action in the "Encounter" controller,
                //But there's no such action to be found
                return RedirectToAction("Error");
            }
            
            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);
            

            return View(new ViewEncounterPageModel
            {
                Encounter = encounter,
                Patient   = patient
            });
        }
        #endregion // end of PCA Index

        #region Physician Discharge
        /// <summary>
        /// View Physician Discharge Notes page
        /// </summary>
        /// <param name="id">Id of unique encounter</param>
        // Used in:  Encounter Menu
        [Authorize]
        [PermissionAuthorize("DischargeSummaryView")]
        public IActionResult ViewPhysicianDischarge(long id)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            ViewBag.EncounterId = id;

            var desiredPatientEncounter = _repository.Encounters
                .Include(e => e.EncounterPhysicians)
                .ThenInclude(e => e.Physician)
                .Include(e => e.DischargeAuthoringProvider)
                .Include(e => e.DischargeCoSigningProvider)
                .Include(e => e.PlaceOfService)
                .FirstOrDefault(u => u.EncounterId == id);

            var desiredPatient = _repository.Patients.FirstOrDefault(u => u.Mrn == desiredPatientEncounter.Mrn);
            var desiredPhysicianAssessment = _repository.PhysicianAssessments.FirstOrDefault(x => x.EncounterId == id);

            ViewBag.Patient = desiredPatient;
            ViewBag.Encounter = desiredPatientEncounter;

            var model = new ViewEncounterPageModel(desiredPatientEncounter,desiredPatient,desiredPhysicianAssessment);

            return View(model);
        }

        /// <summary>
        /// View Physician Discharge Edit Notes page
        /// </summary>
        /// <param name="id">Id of unique encounter</param>
        // Used in:  ViewPhysicianDischarge
        [Authorize]
        [PermissionAuthorize("DischargeSummaryEdit")]
         public IActionResult PhysicianDischargeEdit(long id)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            ViewBag.EncounterId = id;

            var desiredPatientEncounter = _repository.Encounters
                .Include(e => e.EncounterPhysicians)
                .ThenInclude(e => e.Physician)
                .Include(e => e.PlaceOfService)
                .FirstOrDefault(u => u.EncounterId == id);
                
            var desiredPatient = _repository.Patients.FirstOrDefault(u => u.Mrn == desiredPatientEncounter.Mrn);
            var desiredPhysicianAssessment = _repository.PhysicianAssessments.FirstOrDefault(x => x.EncounterId == id);

            // check if signed date is null
            ViewBag.checkSignedDateForNull = false;
            if(desiredPatientEncounter.DischargeAuthoringProviderSignedDate is null){
                ViewBag.checkSignedDateForNull = true;
            }

            ViewBag.Patient = desiredPatient;
            ViewBag.Encounter = desiredPatientEncounter;

            var queryPhysician = _repository.Physicians
                .Include(p => p.Specialty)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                .ToList();

            ViewBag.Providers = new SelectList(queryPhysician, "PhysicianId", "FullName", 0);

            var queryCosigner = _repository.Physicians
                .Where(p => p.ProviderStatusId == 1)
                .Include(p => p.Specialty)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new {p.PhysicianId, FullName = $"{p.FirstName} {p.LastName} ({p.Specialty.Name})"})
                .ToList();

            ViewBag.Cosigners = new SelectList(queryCosigner,  "PhysicianId", "FullName",  0);
            
            var model = new ViewEncounterPageModel(desiredPatientEncounter,desiredPatient,desiredPhysicianAssessment){};

            return View(model);
        }
        
        /// <summary>
        /// obtain a physician status id given a physician id 
        /// </summary>
        /// used in multiple views to determine if a co-signer is required, accessed via ajax within a script
        [HttpGet]
        public JsonResult GetProviderStatusId(long id)
        {
            var providerIdNumber = _repository.Physicians.Where(p => p.PhysicianId == id)
            .Select(p => p.ProviderStatusId )
            .FirstOrDefault()
            .ToString();

            return Json(new{providerIdNumber});

        }

        /// <summary>
        /// determine if a History And Physical exists for a given Encounter id
        /// </summary>
        /// used in the _EncounterMenu.cshtml to disable/enable the Discharge Summary menu link if a History and Physical has not yet been performed
        [HttpGet]
        public JsonResult GetHistoryAndPhysicalStatus(long id)
        {
            var historyAndPhysicalId = _repository.PhysicianAssessments
                .Where(p => p.EncounterId == id)
                .Select(p => p.PhysicianAssessmentId)
                .FirstOrDefault()
                .ToString();
            
            return Json(new{historyAndPhysicalId});
        }

        /// <summary>
        /// Save edits to the encounter from Physician Discharge Edit page
        /// </summary>
        /// <param name="model">Encounter model to be edited to allow user to post data to the encounter table</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DischargeSummaryEdit")]
        public IActionResult PhysicianDischargeEdit(Encounter model)
        {
            var editedBy = UserHelper.GetEditedBy(_repository, _userManager, User);

            if(ModelState.IsValid){

                Encounter tempEncounter = _repository.Encounters.FirstOrDefault(e=> e.EncounterId == model.EncounterId);
                tempEncounter.AdmittingDiagnosis=model.AdmittingDiagnosis;
                tempEncounter.DischargeDiagnosis=model.DischargeDiagnosis;
                tempEncounter.HistoryOfPresentIllness= model.HistoryOfPresentIllness;
                tempEncounter.SignificantFindings = model.SignificantFindings;
                tempEncounter.DischargeHospitalCourse = model.DischargeHospitalCourse;
                tempEncounter.DischargeDispositionNote = model.DischargeDispositionNote;
                tempEncounter.MedicationsOnDischarge = model.MedicationsOnDischarge;
                tempEncounter.DischargeDietInstructions = model.DischargeDietInstructions;
                tempEncounter.DischargeAuthoringProviderId = model.DischargeAuthoringProviderId == 0 ? null : model.DischargeAuthoringProviderId;
                tempEncounter.DischargeAuthoringProviderSignature = model.DischargeAuthoringProviderSignature;
                tempEncounter.DischargeAuthoringProviderSignedDate = model.DischargeAuthoringProviderSignedDate;
                tempEncounter.DischargeCoSigningProviderId = model.DischargeCoSigningProviderId == 0 ? null : model.DischargeCoSigningProviderId;
                tempEncounter.DischargeCoSigningProviderSignature = model.DischargeCoSigningProviderSignature;
                tempEncounter.DischargeCoSigningProviderSignedDate = model.DischargeCoSigningProviderSignedDate;

                tempEncounter.LastModified = DateTime.Now;
                tempEncounter.EditedBy = editedBy;
                _repository.EditEncounter(tempEncounter);

                return RedirectToAction("ViewPhysicianDischarge", new {id=model.EncounterId});
            }

            // if ModelState is NOT valid:
            return View(model);
        }
        #endregion // end of Physician Discharge section
    }
}