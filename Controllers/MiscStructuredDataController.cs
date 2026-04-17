using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.ViewModels;
using System;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd", "StructuredDataView", "StructuredDataEdit", "StructuredDataDelete")]
    public class MiscStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public MiscStructuredDataController(IWCTCHealthSystemRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        ///     Index page of Miscellaneous Structured Data
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd", "StructuredDataView", "StructuredDataEdit", "StructuredDataDelete")]
        public IActionResult Index()
        {
            return View();
        }

        #region Document Types
        /// <summary>
        ///     View all Document Types - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDocumentTypes()
        {
            List<DocumentType> documentTypes = _repository.DocumentTypes.ToList();
            return View("DocumentType/ViewDocumentTypes", documentTypes);
        }

        /// <summary>
        ///     Create New Document Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDocumentType()
        {
            return View("DocumentType/CreateDocumentType");
        }

        ///<summary>
        ///     Add new Document Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDocumentType(MiscStructuredDataViewModel model)
        {
            _repository.AddDocumentType(model.DocumentType);

            return RedirectToAction("ViewDocumentTypes");
        }

        ///<summary>
        ///     Edit an existing Document Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDocumentType(byte id)
        {
            DocumentType documentType = _repository.DocumentTypes.FirstOrDefault(r => r.DocumentTypeID == id);

            return View("DocumentType/EditDocumentType", new MiscStructuredDataViewModel(documentType));
        }

        ///<summary>
        ///     Edit  an existing Document Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDocumentType(MiscStructuredDataViewModel model)
        {
            _repository.EditDocumentType(model.DocumentType);

            return RedirectToAction("ViewDocumentTypes");
        }

        ///<summary>
        ///     Delete an existing Document Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDocumentType(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            DocumentType documentType = _repository.DocumentTypes.FirstOrDefault(dt => dt.DocumentTypeID == id);
            return View("DocumentType/DeleteDocumentType",new MiscStructuredDataViewModel (documentType));
        }

        ///<summary>
        ///     Delete an existing Document Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDocumentType(MiscStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteDocumentType(model.DocumentType);
                 
                return RedirectToAction("ViewDocumentTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Document Type. Delete not available.";
                return View("DocumentType/DeleteDocumentType", model);                
            }
        }

        #endregion
    }
} 