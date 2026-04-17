using System;
using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using IS_Proj_HIT.ViewModels.PhysicianStructuredData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class PhysicianDocsStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public PhysicianDocsStructuredDataController(IWCTCHealthSystemRepository repo)
        {
            _repository = repo;
        }

        ///<summary>
        /// Index page of PhysicianDocsStructuredData
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
        public IActionResult Index()
        {
            return View();
        }

        #region Priorities
        ///<summary>
        /// List View of all Priorities
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPriorities()
        {
            List<Priority> priorities = _repository.Priorities.ToList();
            return View(priorities);
        }

        ///<summary>
        /// Getter - Create a Priority
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePriority()
        {
            return View();
        }

        ///<summary>
        /// Post - Create a Priority
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePriority(PhysicianDocsStructuredDataViewModel model)
        {
            _repository.AddPriority(model.Priority);
            return RedirectToAction("ViewPriorities");
        }

        ///<summary>
        /// Getter - Edit a Priority
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPriority(int id)
        {
            Priority priority = _repository.Priorities.FirstOrDefault(p => p.PriorityId == id);
            return View(new PhysicianDocsStructuredDataViewModel (priority));
        }

        ///<summary>
        /// Post - Edit a Priority
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPriority(PhysicianDocsStructuredDataViewModel model)
        {
            _repository.EditPriority(model.Priority);
            return RedirectToAction("ViewPriorities");
        }

        ///<summary>
        /// Getter - Delete a Priority
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePriority(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Priority priority = _repository.Priorities.FirstOrDefault(p => p.PriorityId == id);

            if(priority == null)
            {
                return NotFound();
            }

            return View(new PhysicianDocsStructuredDataViewModel (priority));
        }

        ///<summary>
        /// Post - Delete a Priority
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePriority(PhysicianDocsStructuredDataViewModel model)
        {
            try
            {
                _repository.DeletePriority(model.Priority); 

                return RedirectToAction("ViewPriorities"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Priority. Delete not available.";
                return View(model);                
            }              
        }
        #endregion  // end of Priority section

        #region Note Types
        ///<summary>
        /// List View of all Note Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewNoteTypes()
        {
            List<NoteType> noteTypes = _repository.NoteTypes.ToList();
            return View(noteTypes);
        }

        ///<summary>
        /// Getter - Create a Note Type
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateNoteType()
        {
            return View();
        }

        ///<summary>
        /// Post - Create a Note Type
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateNoteType(PhysicianDocsStructuredDataViewModel model)
        {
            model.NoteType.LastModified = DateTime.Now;
            _repository.AddNoteType(model.NoteType);
            return RedirectToAction("ViewNoteTypes");
        }

        ///<summary>
        /// Getter - Edit a Note Type
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditNoteType(int id)
        {
            NoteType noteType = _repository.NoteTypes.FirstOrDefault(p => p.NoteTypeId == id);
            return View(new PhysicianDocsStructuredDataViewModel (noteType));
        }

        ///<summary>
        /// Post - Edit a Note Type
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
        public IActionResult EditNoteType(PhysicianDocsStructuredDataViewModel model)
        {
            model.NoteType.LastModified = DateTime.Now;
            _repository.EditNoteType(model.NoteType);
            return RedirectToAction("ViewNoteTypes");
        }

        ///<summary>
        /// Getter - Delete a Note Type
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
        public IActionResult DeleteNoteType(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            NoteType noteType = _repository.NoteTypes.FirstOrDefault(p => p.NoteTypeId == id);

            if(noteType == null)
            {
                return NotFound();
            }

            return View(new PhysicianDocsStructuredDataViewModel (noteType));
        }

        ///<summary>
        /// Post - Delete a Note Type
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteNoteType(PhysicianDocsStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteNoteType(model.NoteType); 

                return RedirectToAction("ViewNoteTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Note Type. Delete not available.";
                return View(model);                
            }              
        }
        #endregion  // end of Note Type section

        #region Visit Type
        ///<summary>
        /// List View of all Visit Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewVisitTypes()
        {
            List<VisitType> visitTypes = _repository.VisitTypes.ToList();
            return View(visitTypes);
        }

        ///<summary>
        /// Getter - Create a Visit Type
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateVisitType()
        {
            return View();
        }

        ///<summary>
        /// Post - Create a Visit Type
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateVisitType(PhysicianDocsStructuredDataViewModel model)
        {
            model.VisitType.LastModified = DateTime.Now;
            _repository.AddVisitType(model.VisitType);
            return RedirectToAction("ViewVisitTypes");
        }

        ///<summary>
        /// Getter - Edit a Visit Type
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditVisitType(int id)
        {
            VisitType visitType = _repository.VisitTypes.FirstOrDefault(p => p.VisitTypeId == id);
            return View(new PhysicianDocsStructuredDataViewModel (visitType));
        }

        ///<summary>
        /// Post - Edit a Visit Type
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditVisitType(PhysicianDocsStructuredDataViewModel model)
        {
            model.VisitType.LastModified = DateTime.Now;
            _repository.EditVisitType(model.VisitType);
            return RedirectToAction("ViewVisitTypes");
        }

        ///<summary>
        /// Getter - Delete a Visit Type
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteVisitType(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            VisitType visitType = _repository.VisitTypes.FirstOrDefault(p => p.VisitTypeId == id);

            if(visitType == null)
            {
                return NotFound();
            }

            return View(new PhysicianDocsStructuredDataViewModel (visitType));
        }

        ///<summary>
        /// Post - Delete a Visit Type
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteVisitType(PhysicianDocsStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteVisitType(model.VisitType); 

                return RedirectToAction("ViewVisitTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Visit Type. Delete not available.";
                return View(model);                
            }              
        }
        #endregion  // end of Visit Type section
    }
}