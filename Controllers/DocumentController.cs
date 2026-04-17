using System;   // for lots of things, specifically DateTime
using System.Collections.Generic;   // for List<T>
using System.IO;    // for MemoryStream
using System.Linq;  // for LINQ operators like .Where
using System.Threading.Tasks;   // for Task/async
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;    // for IFormFile
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; // for Controller, IActionResult
using Microsoft.AspNetCore.Mvc.Rendering;   // for SelectList
using Microsoft.EntityFrameworkCore;    // for FirstOrDefaultAsync
using Newtonsoft.Json;  // for JsonConvert

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("PatientDocumentAdd","PatientDocumentDelete","PatientDocumentEdit","PatientDocumentView","EncounterDocumentAdd","EncounterDocumentDelete","EncounterDocumentEdit","EncounterDocumentView")]
    public class DocumentController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly IPatientService _patientService;

        public DocumentController(IWCTCHealthSystemRepository repository, IPatientService patientService)
        {
            _repository = repository;
            _patientService = patientService;
        }

        /// <summary>
        ///     Display a tabulated list of a Documents for a given Mrn or EncounterId - Getter
        /// </summary>
        /// <param name="mrn"></param>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PatientDocumentView","EncounterDocumentView")]
        public async Task<IActionResult> ViewDocuments(string mrn, long? encounterId, int? docType = null)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // set a pass through for the Encounter Menu, Labs & Diagnostics links
            ViewData["docType"] = docType;

            // Load the Patient (needed for the menu/banner partial views)
            var patient = await _repository.Patients.FirstOrDefaultAsync(p => p.Mrn == mrn);

            if (patient == null)
                return NotFound("Patient not found");

            // Load the Encounter (needed for the menu partial view)
            var encounter = await _repository.Encounters.FirstOrDefaultAsync(e => e.EncounterId == encounterId);

            // call the private method to build the docTypes SelectList
            var section = "none";
            var docTypes = await GetDocumentTypeSelectListAsync(encounterId, section);

            // find all documents based upon Mrn
            var query = _repository.Documents.Include(d => d.DocumentType).Where(d => d.Mrn == mrn);

            // return only encounter documents if encounterId is not null
            if (encounterId.HasValue)
            {
                query = query.Where(d =>
                    d.EncounterId == encounterId.Value &&
                    d.DocumentType.DocumentTypeLevel == "Encounter");

                    // further filter the list to accommodate the Encounter Menu, Labs & Diagnostics links
                    if (docType.HasValue)
                    {
                        query = query.Where(d => d.DocumentType.DocumentTypeID == docType.Value);
                    }

            }
            else    // otherwise return only patient documents
            {
                query = query.Where(d =>
                    d.EncounterId == null &&
                    d.DocumentType.DocumentTypeLevel == "Patient");
            }

            // populate a list of docs using the DocumentViewModel
            var docs = await query.
                Select(d => new DocumentViewModel
                {
                    DocumentId = d.DocumentID,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    DocumentTypeName = d.DocumentType.DocumentTypeName,
                    CreatedAt = d.CreatedAt,
                    DocumentDescription = d.DocumentDescription,
                    Notes = d.Notes,
                    EncounterId = encounterId
                })
                .ToListAsync();

            // populate the wrapper view model
            var vm = new DocWrapperViewModel
            {
                Patient = patient,
                Encounter = encounter,
                Documents = docs,
                DocumentTypes = docTypes,
                DocType = docType
            };

            return View(vm);
        }

        /// <summary>
        ///     Download a Document from the database - Getter
        ///     Uses a boolean to determine if the download is a preview or to a folder
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inline"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PatientDocumentView","EncounterDocumentView")]
        public async Task<IActionResult> DownloadDocument(byte id, bool inline = false)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // load the document
            var doc = await GetDocumentByIdAsync(id);
            if (doc == null) return NotFound();

            var bytes = doc.DocumentContent;
            string contentType = doc.FileType.ToLower() switch
            {
                "pdf" => "application/pdf",
                "jpeg" => "image/jpeg",
                "jpg" => "image/jpeg",
                "png" => "image/png",
                _ => "application/octet-stream"
            };
            var filename = doc.FileName;

            // choose disposition:  either download for preview or download to the Downloads Folder
            if (inline)
            {
                // returns Content-Disposition:  inline
                // so browsers that support embedding will render it
                return File(bytes, contentType);
            }
            else
            {
                // returns Content-Disposition: attachment; filename="-"
                // which forces the browser to download to the Downloads folder
                return File(doc.DocumentContent, contentType, filename);
            }
        }

        /// <summary>
        ///     Add Documents to the database - Setter
        /// </summary>
        /// <param name="mrn"></param>
        /// <param name="encounterId"></param>
        /// <param name="Files"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("PatientDocumentAdd","EncounterDocumentAdd")]
        public async Task<IActionResult> UploadMultiple(
            string mrn,
            long? encounterId,
            List<CreateDocumentDto> Files)
        {
            foreach (var dto in Files)
            {
                if (dto.File?.Length > 0)
                {
                    // Read file content
                    byte[] content;
                    await using (var ms = new MemoryStream())
                    {
                        await dto.File.CopyToAsync(ms);
                        content = ms.ToArray();
                    }

                    // Determine the file extension/file type
                    var extension = Path.GetExtension(dto.File.FileName);

                    // call the private method to build the filename
                    var fileName = await BuildFileNameAsync(
                                    dto.DocumentTypeID,
                                    mrn,
                                    encounterId,
                                    dto.File.FileName);

                    // Persist
                    var doc = new Document
                    {
                        Mrn = mrn,
                        EncounterId = encounterId,
                        FileName = fileName,
                        FileType = extension.TrimStart('.'),
                        DocumentContent = content,
                        DocumentTypeID = dto.DocumentTypeID,
                        DocumentDescription = dto.DocumentDescription,
                        Notes = dto.Notes,
                        CreatedAt = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow
                    };
                    _repository.AddDocument(doc);
                }
            }

            return RedirectToAction("ViewDocuments", new { mrn, encounterId });
        }

        /// <summary>
        ///     Display the details of a document - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PatientDocumentView","EncounterDocumentView")]
        public async Task<IActionResult> DetailsPartial(int id, int? docType = null)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // pass docType through to the view
            ViewData["docType"] = docType;

            var doc = await GetDocumentByIdAsync(id);
            if (doc == null)
                return NotFound();

            var documentTypeName = await GetDocumentTypeNameAsync(doc.DocumentTypeID);

            var vm = new DocumentDetailsViewModel
            {
                DocumentId = doc.DocumentID,
                FileName = doc.FileName,
                MimeType = doc.FileType.Equals("pdf", StringComparison.OrdinalIgnoreCase)
                    ? "application/pdf"
                    : $"image/{doc.FileType}",
                Base64Content = Convert.ToBase64String(doc.DocumentContent),
                DocumentTypeName = documentTypeName,
                Mrn = doc.Mrn,
                DocumentDescription = doc.DocumentDescription,
                Notes = doc.Notes,
                EncounterId = doc.EncounterId
            };
            return PartialView("_DocumentDetailsPartial", vm);
        }

        /// <summary>
        ///     Load the Document Edit form into the modal - GETTER
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("PatientDocumentEdit","EncounterDocumentEdit")]
        public async Task<IActionResult> EditPartial(int id, string section, string selectedMrn)
        {
            // permissions for rendering buttons
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            ViewData["UserPermissions"] = permissions;

            var doc = await _repository.Documents.FirstOrDefaultAsync(d => d.DocumentID == id);
            if (doc == null) return NotFound();

            // normalize section to avoid casing issues
            var sectionNormalized = (section ?? string.Empty).Trim().ToLowerInvariant();

            // build select lists
            var docTypes = await GetDocumentTypeSelectListAsync(doc.EncounterId, section);
            var encList = await GetEncounterSelectListAsync(doc.Mrn);

            // safe mime type mapping
            string mimeType;
            if (!string.IsNullOrWhiteSpace(doc.FileType) && doc.FileType.Equals("pdf", StringComparison.OrdinalIgnoreCase))
            {
                mimeType = "application/pdf";
            }
            else
            {
                var ft = (doc.FileType ?? string.Empty).Trim().ToLowerInvariant();
                if (ft == "jpg") ft = "jpeg";
                mimeType = string.IsNullOrEmpty(ft) ? "application/octet-stream" : $"image/{ft}";
            }

            var vm = new DocumentEditViewModel
            {
                DocumentId = doc.DocumentID,
                Mrn = doc.Mrn,
                EncounterId = doc.EncounterId,
                DocumentTypeID = doc.DocumentTypeID,
                DocumentDescription = doc.DocumentDescription,
                Notes = doc.Notes,
                DocumentTypes = docTypes,
                EncounterSelectList = encList,
                MimeType = mimeType,
                Base64Content = doc.DocumentContent != null ? Convert.ToBase64String(doc.DocumentContent) : string.Empty,
                Section = sectionNormalized
            };

            return PartialView("_DocumentEditPartial", vm);
        }

        /// <summary>
        ///     Apply edits, return JSON success or re-render the _DocumentEditPartial.cshtml if the model is invalid - SETTER
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("PatientDocumentEdit","EncounterDocumentEdit")]
        public async Task<IActionResult> EditPartial(DocumentEditViewModel vm)
        {
            // normalize section early to avoid casing issues
            vm.Section = (vm.Section ?? string.Empty).Trim().ToLowerInvariant();

            // server-side validation: when MRN may be changed/required, ensure it exists and belongs to facility
            var sectionsThatRequirePatientCheck = new[]
            {
                "patient2differentpatient",
                "patient2differentpatientencounter",
                "encounter2differentpatientencounter"
            };

            if (sectionsThatRequirePatientCheck.Contains(vm.Section))
            {
                var facilityIdString = HttpContext.Session.GetString("Facility");
                if (string.IsNullOrEmpty(facilityIdString) || !int.TryParse(facilityIdString, out var facilityId))
                {
                    return Unauthorized();
                }

                if (string.IsNullOrWhiteSpace(vm.Mrn))
                {
                    ModelState.AddModelError(nameof(vm.Mrn), "Please select a patient.");
                }
                else
                {
                    var patient = await _patientService.GetPatientByMrnAsync(vm.Mrn);
                    if (patient == null || patient.FacilityId != facilityId)
                    {
                        ModelState.AddModelError(nameof(vm.Mrn), "Selected patient not found or not in your facility.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                // rehydrate UI data for the partial
                var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                ViewData["UserPermissions"] = permissions;

                vm.DocumentTypes = await GetDocumentTypeSelectListAsync(vm.EncounterId, vm.Section);
                vm.EncounterSelectList = await GetEncounterSelectListAsync(vm.Mrn);

                return PartialView("_DocumentEditPartial", vm);
            }

            var doc = await _repository.Documents.FirstOrDefaultAsync(d => d.DocumentID == vm.DocumentId);
            if (doc == null) return NotFound();

            // capture originals for rename detection
            var origMrn = doc.Mrn;
            var origEncounterId = doc.EncounterId;
            var origDocType = doc.DocumentTypeID;

            // apply changes according to section
            switch (vm.Section)
            {
                case "edit":
                    doc.Mrn = vm.Mrn;
                    doc.EncounterId = vm.EncounterId;
                    doc.DocumentTypeID = vm.DocumentTypeID;
                    doc.DocumentDescription = vm.DocumentDescription;
                    doc.Notes = vm.Notes;
                    break;

                case "patient2differentpatient":
                    doc.Mrn = vm.Mrn;
                    doc.EncounterId = null;
                    break;

                case "patient2encounter":
                    doc.EncounterId = vm.EncounterId;
                    doc.DocumentTypeID = vm.DocumentTypeID;
                    break;

                case "encounter2encounter":
                    doc.EncounterId = vm.EncounterId;
                    break;

                case "patient2differentpatientencounter":
                    doc.Mrn = vm.Mrn;
                    doc.EncounterId = vm.EncounterId;
                    doc.DocumentTypeID = vm.DocumentTypeID;
                    break;

                case "encounter2patient":
                    doc.Mrn = vm.Mrn;
                    doc.EncounterId = null;
                    doc.DocumentTypeID = vm.DocumentTypeID;
                    break;

                case "encounter2differentpatientencounter":
                    doc.Mrn = vm.Mrn;
                    doc.EncounterId = vm.EncounterId;
                    break;

                default:
                    break;
            }

            // determine if file name should be rebuilt (compare original values to final values)
            var needsRename = origMrn != doc.Mrn || origEncounterId != doc.EncounterId || origDocType != doc.DocumentTypeID;
            if (needsRename)
            {
                var oldFileName = string.IsNullOrWhiteSpace(doc.FileName) ? string.Empty : doc.FileName;
                doc.FileName = await BuildFileNameAsync(doc.DocumentTypeID, doc.Mrn, doc.EncounterId, oldFileName);
            }

            doc.LastModified = DateTime.UtcNow;
            _repository.EditDocument(doc);

            return Json(new { success = true });
        }


        /// <summary>
        ///     Delete a Document from the database - Setter
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("PatientDocumentDelete","EncounterDocumentDelete")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            var doc = await GetDocumentByIdAsync(documentId);
            if (doc == null)
            {
                TempData["Error"] = "Document not found.";
                return RedirectToAction("ViewDocuments", new { mrn = doc.Mrn, encounterId = doc.EncounterId });
            }

            try
            {
                _repository.DeleteDocument(doc);
                TempData["Message"] = "Document successfully deleted.";
            }
            catch
            {
                TempData["Error"] = "Failed to delete document.";
            }
            return RedirectToAction("ViewDocuments", new { mrn = doc.Mrn, encounterId = doc.EncounterId });
        }

        /// <summary>
        ///     AJAX endpoint for grabbing the list of Encounters for a specific Mrn
        ///     used in the _DocumentDetailsPartial.cshtml in the scenario patient2differentpatientencounter
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("PatientDocumentEdit","EncounterDocumentEdit")]
        public async Task<IActionResult> GetEncountersForPatient(string mrn)
        {
            if (string.IsNullOrWhiteSpace(mrn))
                // return just the empty placeholder option
                return Json(new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "" }
                });

            var list = await GetEncounterSelectListAsync(mrn);
            return Json(list);
        }

        #region Private Methods

        /// <summary>
        ///     Retrieve a Document given the DocumentID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _repository.Documents.FirstOrDefaultAsync(d => d.DocumentID == id);
        }

        /// <summary>
        ///     Retrieve the DocumentTypeName given the DocumentTypeID
        /// </summary>
        /// <param name="documentTypeId"></param>
        /// <returns></returns>
        private async Task<string> GetDocumentTypeNameAsync(int documentTypeId)
        {
            // Read-only query: no tracking for slightly better performance
            return await _repository.DocumentTypes
                                .AsNoTracking()
                                .Where(dt => dt.DocumentTypeID == documentTypeId)
                                .Select(dt => dt.DocumentTypeName)
                                .FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Generates a filename in the format:
        ///     {DocumentTypeName}-{MRN}-{EncounterId?}-{yyyyMMddHHmmssfff}{extension}
        /// </summary>
        private async Task<string> BuildFileNameAsync(
            int documentTypeId,
            string mrn,
            long? encounterId,
            string originalFileName)
        {
            //  Current business rules wants the filename to be:  
            //  DocumentType-MRN-ENCOUNTER (if encounter level)-Date in YYYYMMDDTTTT-sequence (if more than one with this date/time stamp);  
            // however, instead of sequence, start with the timestamp in milliseconds:  yyyMMddHHmmssfff.  Two files uploaded in the same batch literally never share the same timestamp, so sequence can either be hardcoded or omitted.  
            // This code omits the sequence.

            // look up the human‐readable document type
            var docType = await GetDocumentTypeNameAsync(documentTypeId);

            // timestamp with milliseconds
            // DateTime.Now returns local time where the server is located.  Use DateTime.UtcNow if prefer a global time regardless of server location
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            // include encounter if we have one
            var encounterPart = encounterId.HasValue
                ? "-" + encounterId.Value
                : string.Empty;

            // Determine sequence here if needed for this timestamp

            // preserve original extension
            var extension = Path.GetExtension(originalFileName);

            return $"{docType}-{mrn}{encounterPart}-{timestamp}{extension}";
        }

        /// <summary>
        ///     Returns a list of SelectListItems for either 'Patient' or 'Encounter'
        ///     document‐type levels, based on whether encounterId has a value.
        /// </summary>
        /// <param name="encounterId"></param>
        private async Task<List<SelectListItem>> GetDocumentTypeSelectListAsync(long? encounterId, string section)
        {
            // decide which level to show
            var level = (encounterId.HasValue || section == "patient2encounter" )
                ? "Encounter"
                : "Patient";

            if(section == "encounter2patient"){ level = "Patient"; }
            if(section == "patient2differentpatientencounter"){ level = "Encounter"; }
            if(section == "encounter2differentpatientencounter"){ level = "Encounter"; }

            // query, filter, project, execute
            return await _repository.DocumentTypes
                .Where(dt => dt.DocumentTypeLevel == level)
                .Select(dt => new SelectListItem
                {
                    Value = dt.DocumentTypeID.ToString(),
                    Text = dt.DocumentTypeName
                })
                .ToListAsync();
        }


        /// <summary>
        ///     List all Encounters for a given MRN
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns>list</returns>
        private async Task<List<SelectListItem>> GetEncounterSelectListAsync(string mrn)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "" }
            };

            var items = await _repository.Encounters
                .Where(e => e.Mrn == mrn)
                .OrderByDescending(e => e.AdmitDateTime)
                .Select(e => new SelectListItem
                {
                    Value = e.EncounterId.ToString(),
                    Text = $"{e.EncounterId,-10}Admit Date: {e.AdmitDateTime:MM-dd-yyyy}"
                })
                .ToListAsync();

            list.AddRange(items);
            return list;
        }

        #endregion
    }
}