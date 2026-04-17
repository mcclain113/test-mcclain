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
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class EncounterStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        
        public EncounterStructuredDataController(IWCTCHealthSystemRepository repo) 
        {
            _repository = repo;
        } 

        
        /// <summary>
        /// Index page of EncounterStructuredData
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
        public IActionResult Index() 
        {
            return View();
        }

        #region Admit types // Begin section for Admit Types

        /// <summary>
        /// Index page of Admit Types
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewAdmitTypes()
        {
            List<AdmitType> admitTypes = _repository.AdmitTypes.ToList();
            return View("AdmitType/ViewAdmitTypes",admitTypes);
        }

        /// <summary>
        /// Create New Admit Type
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAdmitType()
        {
            ViewData["ErrorMessage"] = "";
            
            return View("AdmitType/CreateAdmitType");
        }

        ///<summary>
        /// Add new AdmitType to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateAdmitType(EncounterStructuredDataViewModel model)
        {
           try 
            { 
                // Check if WiPopCode is unique 
                var existingAdmitType = _repository.AdmitTypes.FirstOrDefault(a => a.WiPopCode == model.AdmitType.WiPopCode);
                if (existingAdmitType != null) 
                { 
                    throw new Exception(); 
                } 
                
                _repository.AddAdmitType(model.AdmitType); 
                return RedirectToAction("ViewAdmitTypes"); 
            } 
            catch (Exception) 
            {  
                ViewData["ErrorMessage"] = "The WiPopCode entered is already in use.";
                return View("AdmitType/CreateAdmitType", model);
            }
           
        }

        ///<summary>
        /// Edit fields of an existing AdmitType
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAdmitType(int id) 
        {
            AdmitType admitType = _repository.AdmitTypes.FirstOrDefault(r => r.AdmitTypeId == id);

            return View("AdmitType/EditAdmitType",new EncounterStructuredDataViewModel (admitType));
        }

        ///<summary>
        /// Edit fields of an existing AdmitType in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAdmitType(EncounterStructuredDataViewModel model) 
        {
            model.AdmitType.LastModified = DateTime.Now;
            _repository.EditAdmitType(model.AdmitType);

            return RedirectToAction("ViewAdmitTypes");
        }

        ///<summary>
        /// Delete an existing Admit Type
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAdmitType(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            AdmitType admitType = _repository.AdmitTypes.FirstOrDefault(r => r.AdmitTypeId == id);
            return View("AdmitType/DeleteAdmitType",new EncounterStructuredDataViewModel (admitType));
        }

        ///<summary>
        /// Delete an existing Admit Type in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAdmitType(EncounterStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteAdmitType(model.AdmitType);
                 

                return RedirectToAction("ViewAdmitTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Admit Type. Delete not available.";
                return View("AdmitType/DeleteAdmitType", model);                
            }

        }

        #endregion // End section for Admit Types

        #region Department  // Begin section for Department

        /// <summary>
        /// Index page of Departments
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDepartments()
        {
            List<Department> departments = _repository.Departments.ToList();
            return View("Department/ViewDepartments",departments);
        }

        /// <summary>
        /// Create New Department
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDepartment()
        {
            return View("Department/CreateDepartment");
        }

        ///<summary>
        /// Add new Department to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateDepartment(EncounterStructuredDataViewModel model)
        {
            _repository.AddDepartment(model.Department);

            return RedirectToAction("ViewDepartments");
        }

        ///<summary>
        /// Edit fields of an existing Department
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDepartment(int id) 
        {
            Department department = _repository.Departments.FirstOrDefault(r => r.DepartmentId == id);

            return View("Department/EditDepartment",new EncounterStructuredDataViewModel (department));
        }

        ///<summary>
        /// Edit fields of an existing Department in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDepartment(EncounterStructuredDataViewModel model) 
        {
            model.Department.LastModified = DateTime.Now;
            _repository.EditDepartment(model.Department);

            return RedirectToAction("ViewDepartments");
        }

        ///<summary>
        /// Delete an existing Department
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDepartment(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Department department = _repository.Departments.FirstOrDefault(r => r.DepartmentId == id);
            return View("Department/DeleteDepartment",new EncounterStructuredDataViewModel (department));
        }

        ///<summary>
        /// Delete an existing Department in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDepartment(EncounterStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteDepartment(model.Department);
                 

                return RedirectToAction("ViewDepartments"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Department. Delete not available.";
                return View("Department/DeleteDepartment", model);                
            }

        }
        
        #endregion  // End section for Department

        #region Discharge   // Begin section for Discharge

        /// <summary>
        /// Index page of Discharges
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDischarges()
        {
            List<Discharge> discharges = _repository.Discharges.ToList();
            return View("Discharge/ViewDischarges",discharges);
        }

        /// <summary>
        /// Create New Discharge
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDischarge()
        {
            ViewData["ErrorMessage"] = "";
            return View("Discharge/CreateDischarge");
        }

        ///<summary>
        /// Add new Discharge to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateDischarge(EncounterStructuredDataViewModel model)
        {
           try 
            { 
                // Check if WiPopCode is unique 
                var existingWiPopCode = _repository.Discharges.FirstOrDefault(a => a.WiPopCode == model.Discharge.WiPopCode);
                if (existingWiPopCode != null) 
                { 
                    throw new Exception(); 
                } 
                 
                _repository.AddDischarge(model.Discharge); 
                return RedirectToAction("ViewDischarges"); 
            } 
            catch (Exception) 
            {  
                ViewData["ErrorMessage"] = "The WiPopCode entered is already in use.";
                return View("Discharge/CreateDischarge", model);
            }
        }

        ///<summary>
        /// Edit fields of an existing Discharge
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDischarge(int id) 
        {
            Discharge discharge = _repository.Discharges.FirstOrDefault(r => r.DischargeId == id);

            return View("Discharge/EditDischarge",new EncounterStructuredDataViewModel (discharge));
        }

        ///<summary>
        /// Edit fields of an existing Discharge in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDischarge(EncounterStructuredDataViewModel model) 
        {
            model.Discharge.LastModified = DateTime.Now;
            _repository.EditDischarge(model.Discharge);

            return RedirectToAction("ViewDischarges");
        }

        ///<summary>
        /// Delete an existing Discharge
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDischarge(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Discharge discharge = _repository.Discharges.FirstOrDefault(r => r.DischargeId == id);
            return View("Discharge/DeleteDischarge",new EncounterStructuredDataViewModel (discharge));
        }

        ///<summary>
        /// Delete an existing Discharge in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDischarge(EncounterStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteDischarge(model.Discharge);
                 

                return RedirectToAction("ViewDischarges"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Discharge. Delete not available.";
                return View("Discharge/DeleteDischarge", model);                
            }

        }
        
        #endregion  // End section for Discharge

        #region Encounter Type  // Begin section for Encounter Type

        /// <summary>
        /// Index page of Encounter Types
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewEncounterTypes()
        {
            List<EncounterType> encounterTypes = _repository.EncounterTypes.ToList();
            return View("EncounterType/ViewEncounterTypes",encounterTypes);
        }

        /// <summary>
        /// Create New Encounter Type
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateEncounterType()
        {
            return View("EncounterType/CreateEncounterType");
        }

        ///<summary>
        /// Add new EncounterType to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateEncounterType(EncounterStructuredDataViewModel model)
        {
            _repository.AddEncounterType(model.EncounterType);

            return RedirectToAction("ViewEncounterTypes");
        }

        ///<summary>
        /// Edit fields of an existing EncounterType
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditEncounterType(int id) 
        {
            EncounterType encounterType = _repository.EncounterTypes.FirstOrDefault(r => r.EncounterTypeId == id);

            return View("EncounterType/EditEncounterType",new EncounterStructuredDataViewModel (encounterType));
        }

        ///<summary>
        /// Edit fields of an existing EncounterType in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditEncounterType(EncounterStructuredDataViewModel model) 
        {
            model.EncounterType.LastModified = DateTime.Now;
            _repository.EditEncounterType(model.EncounterType);

            return RedirectToAction("ViewEncounterTypes");
        }

        ///<summary>
        /// Delete an existing EncounterType
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteEncounterType(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            EncounterType encounterType = _repository.EncounterTypes.FirstOrDefault(r => r.EncounterTypeId == id);
            return View("EncounterType/DeleteEncounterType",new EncounterStructuredDataViewModel (encounterType));
        }

        ///<summary>
        /// Delete an existing EncounterType in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteEncounterType(EncounterStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteEncounterType(model.EncounterType);
                 

                return RedirectToAction("ViewEncounterTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this EncounterType. Delete not available.";
                return View("EncounterType/DeleteEncounterType", model);                
            }

        }
        
        #endregion  // End section for Encounter Type

        #region Place Of Service Outpatient // Begin section for Place of Service OutPatient

        /// <summary>
        /// Index page of Place of Service OutPatient
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPlaceOfServiceOutPatients()
        {
            List<PlaceOfServiceOutPatient> placeOfServiceOutPatient = _repository.PlaceOfService.ToList();

            return View("PlaceOfServiceOutPatient/ViewPlaceOfServiceOutPatients",placeOfServiceOutPatient);
        }

        /// <summary>
        /// Create New Place of Service OutPatient
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePlaceOfServiceOutPatient()
        {
            ViewData["ErrorMessage"] = "";
            return View("PlaceOfServiceOutPatient/CreatePlaceOfServiceOutPatient");
        }

        ///<summary>
        /// Add new Place of Service OutPatient to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreatePlaceOfServiceOutPatient(EncounterStructuredDataViewModel model)
        {
            try 
            { 
                // Check if WiPopCode is unique 
                var existingWiPopCode = _repository.Discharges.FirstOrDefault(a => a.WiPopCode == model.Discharge.WiPopCode);
                if (existingWiPopCode != null) 
                { 
                    throw new Exception(); 
                } 
                 
                _repository.AddPlaceOfService(model.PlaceOfServiceOutPatient); 
                return RedirectToAction("ViewPlaceOfServiceOutPatients"); 
            } 
            catch (Exception) 
            {  
                ViewData["ErrorMessage"] = "The WiPopCode entered is already in use.";
                return View("PlaceOfServiceOutPatient/CreatePlaceOfServiceOutPatient", model);
            }
            
        }

        ///<summary>
        /// Edit fields of an existing Place of Service OutPatient
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPlaceOfServiceOutPatient(int id) 
        {
            PlaceOfServiceOutPatient placeOfServiceOutPatient = _repository.PlaceOfService.FirstOrDefault(r => r.PlaceOfServiceId == id);

            return View("PlaceOfServiceOutPatient/EditPlaceOfServiceOutPatient",new EncounterStructuredDataViewModel (placeOfServiceOutPatient));
        }

        ///<summary>
        /// Edit fields of an existing Place of Service OutPatient in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPlaceOfServiceOutPatient(EncounterStructuredDataViewModel model) 
        {
            model.PlaceOfServiceOutPatient.LastModified = DateTime.Now;
            _repository.EditPlaceOfService(model.PlaceOfServiceOutPatient);

            return RedirectToAction("ViewPlaceOfServiceOutPatients");
        }

        ///<summary>
        /// Delete an existing Place of Service OutPatient
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePlaceOfServiceOutPatient(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            PlaceOfServiceOutPatient placeOfServiceOutPatient = _repository.PlaceOfService.FirstOrDefault(r => r.PlaceOfServiceId == id);

            return View("PlaceOfServiceOutPatient/DeletePlaceOfServiceOutPatient",new EncounterStructuredDataViewModel (placeOfServiceOutPatient));
        }

        ///<summary>
        /// Delete an existing Place of Service OutPatient in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePlaceOfServiceOutPatient(EncounterStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeletePlaceOfService(model.PlaceOfServiceOutPatient);
                 

                return RedirectToAction("ViewPlaceOfServiceOutPatients"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this EncounterType. Delete not available.";
                return View("EncounterType/DeleteEncounterType", model);                
            }

        }
        
        #endregion  // End section for Place of Service OutPatient

        #region Point of Origin // Begin section for Point of Origin

        /// <summary>
        /// Index page of Point of Origin
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPointOfOrigins()
        {
            List<PointOfOrigin> pointOfOrigins = _repository.PointOfOrigin.ToList();
            return View("PointOfOrigin/ViewPointOfOrigins",pointOfOrigins);
        }

        /// <summary>
        /// Create New Point of Origin
        /// </summary>
        [Authorize(Roles = "Administrator")]
        public IActionResult CreatePointOfOrigin()
        {
            ViewData["ErrorMessage"] = "";
            return View("PointOfOrigin/CreatePointOfOrigin");
        }

        ///<summary>
        /// Add new Point of Origin to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePointOfOrigin(EncounterStructuredDataViewModel model)
        {
            try 
            { 
                // Check if WiPopCode is unique 
                var existingWiPopCode = _repository.PointOfOrigin.FirstOrDefault(a => a.WiPopCode == model.PointOfOrigin.WiPopCode);
                if (existingWiPopCode != null) 
                { 
                    throw new Exception(); 
                } 
                
                _repository.AddPointOfOrigin(model.PointOfOrigin);
                return RedirectToAction("ViewPointOfOrigins"); 
            } 
            catch (Exception) 
            {  
                ViewData["ErrorMessage"] = "The WiPopCode entered is already in use.";
                return View("PointOfOrigin/CreatePointOfOrigin", model);
            }
            
        }

        ///<summary>
        /// Edit fields of an existing Point of Origin
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPointOfOrigin(int id) 
        {
            PointOfOrigin pointOfOrigin = _repository.PointOfOrigin.FirstOrDefault(r => r.PointOfOriginId == id);

            return View("PointOfOrigin/EditPointOfOrigin",new EncounterStructuredDataViewModel (pointOfOrigin));
        }

        ///<summary>
        /// Edit fields of an existing Point of Origin in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPointOfOrigin(EncounterStructuredDataViewModel model) 
        {
            model.PointOfOrigin.LastModified = DateTime.Now;
            _repository.EditPointOfOrigin(model.PointOfOrigin);

            return RedirectToAction("ViewPointOfOrigins");
        }

        ///<summary>
        /// Delete an existing Point of Origin
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePointOfOrigin(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            PointOfOrigin pointOfOrigin = _repository.PointOfOrigin.FirstOrDefault(r => r.PointOfOriginId == id);
            return View("PointOfOrigin/DeletePointOfOrigin",new EncounterStructuredDataViewModel (pointOfOrigin));
        }

        ///<summary>
        /// Delete an existing Point of Origin in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePointOfOrigin(EncounterStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeletePointOfOrigin(model.PointOfOrigin);
                 

                return RedirectToAction("ViewPointOfOrigins"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Point Of Origin. Delete not available.";
                return View("PointOfOrigin/DeletePointOfOrigin", model);                
            }

        }
        
        #endregion  // End section for Point of Origin

    }
    
}