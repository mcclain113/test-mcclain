using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using IS_Proj_HIT.ViewModels.PhysicianStructuredData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.Extensions.Logging;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class PhysicianStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly ILogger<PhysicianStructuredDataController> _logger;
        
        public PhysicianStructuredDataController(IWCTCHealthSystemRepository repo,ILogger<PhysicianStructuredDataController> logger) 
        {
            _repository = repo;
            _logger = logger;
        } 

        
        /// <summary>
        /// Index page of PhysicianStructuredData
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult Index() 
        {
            return View();
        }

        #region Physician
        /// <summary>
        /// Index page of Physicians
        /// </summary>
        [Authorize(Roles = "Administrator")]
        public IActionResult ViewPhysicians()
        {
            List<Physician> physicians = _repository.Physicians
                                .Include(p => p.Address)
                                .ThenInclude(a => a.Country)
                                .Include(p => p.ProviderType)
                                .Include(p => p.ProviderStatus)
                                .Include(p => p.Specialty).ToList();
            return View(physicians);
        }

        /// <summary>
        /// Create New Physician
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePhysician()
        {
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(_repository);
            ViewData["CountryId"] = AddressHelper.GetCountries(_repository);

            var providerTypes = new SelectList(_repository.ProviderTypes, "ProviderTypeId", "Name");
                var pTypesList = providerTypes.ToList();
                pTypesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["ProviderTypeId"] = pTypesList;

            var specialties = new SelectList(_repository.Specialties, "SpecialtyId", "Name");
                var specialtiesList = specialties.ToList();
                specialtiesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["SpecialtyId"] = specialtiesList;

            var providerStatuses = new SelectList(_repository.ProviderStatuses, "ProviderStatusId", "Status");
                var providerStatusesList = providerStatuses.ToList();
                providerStatusesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["ProviderStatusId"] = providerStatusesList;

            return View();
        }

        ///<summary>
        /// Add new Physician to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePhysician(PhysicianStructuredDataViewModel model)
        {
            model.Physician.PhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.Physician.PhoneNumber);
            
            if(UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.Address,_logger,
                ("CountryId",1),
                ("AddressStateID",50),
                ("LastModified", DateTime.MinValue)))
            {
                Address address = new Address
                {
                    Address1 = model.Address.Address1,
                    Address2 = (model.Address.Address2 == null) ? null : model.Address.Address2,
                    PostalCode = model.Address.PostalCode,
                    City = model.Address.City,
                    County = (model.Address.County == null) ? null : model.Address.County,
                    AddressStateID = model.Address.AddressStateID,
                    CountryId = (model.Address.CountryId == 0) ? 1 : model.Address.CountryId,
                    LastModified = DateTime.Now
                };
                _repository.AddAddress(address);
                
                model.Physician.AddressId = address.AddressId;
                model.Physician.LastModified = DateTime.Now;

                _repository.AddPhysician(model.Physician);

                return RedirectToAction("ViewPhysicians");
            }
            else
            {
                model.Physician.AddressId = null;
                model.Physician.LastModified = DateTime.Now;

                _repository.AddPhysician(model.Physician);

                return RedirectToAction("ViewPhysicians");
            }


        }

        ///<summary>
        /// View the Details of a Physician
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult DetailsPhysician(int id) 
        {
            Physician physician = _repository.Physicians.FirstOrDefault(p => p.PhysicianId == id);

            Address address = null;
            AddressState addressState = null;
            Country country = null;
            
            if(physician != null && physician.AddressId.HasValue)
            {
                address = _repository.Addresses.FirstOrDefault(a => a.AddressId == physician.AddressId.Value);
                if(address != null)
                {
                    addressState = _repository.AddressStates.FirstOrDefault(a => a.StateID == address.AddressStateID);
                    country = _repository.Countries.FirstOrDefault(c => c.CountryId == address.CountryId);
                }
            }
              
            Specialty specialty = _repository.Specialties.FirstOrDefault(s => s.SpecialtyId == physician.SpecialtyId);
            ProviderStatus providerStatus = _repository.ProviderStatuses.FirstOrDefault(ps => ps.ProviderStatusId == physician.ProviderStatusId);
            ProviderType providerType = _repository.ProviderTypes.FirstOrDefault(pt => pt.ProviderTypeId == physician.ProviderTypeId);
            
            return View(new PhysicianStructuredDataViewModel (physician, address, addressState, country, specialty, providerStatus, providerType));
        }

        ///<summary>
        /// Edit fields of an Physician
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPhysician(int id) 
        {
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(_repository);
            ViewData["CountryId"] = AddressHelper.GetCountries(_repository);

            ViewData["ProviderTypeId"] = new SelectList(_repository.ProviderTypes, "ProviderTypeId", "Name");
            ViewData["SpecialtyId"] = new SelectList(_repository.Specialties, "SpecialtyId", "Name");
            ViewData["ProviderStatusId"] = new SelectList(_repository.ProviderStatuses, "ProviderStatusId", "Status");

            Physician physician = _repository.Physicians.FirstOrDefault(p => p.PhysicianId == id);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == physician.AddressId);
            
            return View(new PhysicianStructuredDataViewModel (physician, address));
        }

        ///<summary>
        /// Edit fields of an Physician in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPhysician(PhysicianStructuredDataViewModel model) 
        {
            model.Physician.PhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.Physician.PhoneNumber);

            if(UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.Address,_logger,
                ("CountryId",1),
                ("AddressStateID",50),
                ("LastModified", DateTime.MinValue)))
            {
                model.Address.LastModified = DateTime.Now;
                _repository.EditAddress(model.Address);

                model.Physician.AddressId = model.Address.AddressId;
                model.Physician.LastModified = DateTime.Now;
                _repository.EditPhysician(model.Physician);

                return RedirectToAction("ViewPhysicians");                
            }
            else
            {
                model.Physician.AddressId = null;
                model.Physician.LastModified = DateTime.Now;
                _repository.EditPhysician(model.Physician);

                return RedirectToAction("ViewPhysicians");
            }


        }

        ///<summary>
        /// Delete an existing Physician
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePhysician(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
            
            Physician physician = _repository.Physicians.FirstOrDefault(p => p.PhysicianId == id);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == physician.AddressId);

            if(physician == null)
                { 
                    return NotFound();
                }

            return View(new PhysicianStructuredDataViewModel (physician, address));
        }

        ///<summary>
        /// Delete an existing Physician in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePhysician(PhysicianStructuredDataViewModel model) 
        {
            Physician physician = _repository.Physicians.FirstOrDefault(p => p.PhysicianId == model.Physician.PhysicianId);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == physician.AddressId);
            try
            {
                if(physician.AddressId != null)
                {
                    _repository.DeletePhysician(physician);
                    _repository.DeleteAddress(address);
                }
                else
                {
                    _repository.DeletePhysician(physician);
                }
 

                return RedirectToAction("ViewPhysicians"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Physician. Delete not available.";
                return View(model);                
            }
        }
        #endregion  // end of Physician section

        #region Physician Roles
        /// <summary>
        /// Index page of Physician Roles
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPhysicianRoles()
        {
            List<PhysicianRole> physicianRoles = _repository.PhysicianRoles.ToList();
            return View(physicianRoles);
        }

        /// <summary>
        /// Create New Physician Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePhysicianRole(){

            return View();
        }

        ///<summary>
        /// Add new Physician Role to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePhysicianRole(PhysicianStructuredDataViewModel model)
        {
            model.PhysicianRole.LastModified = DateTime.Now;
            _repository.AddPhysicianRole(model.PhysicianRole);

            return RedirectToAction("ViewPhysicianRoles");
        }

        ///<summary>
        /// Edit fields of an Physician Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPhysicianRole(int id) 
        {
            PhysicianRole model = _repository.PhysicianRoles.FirstOrDefault(p => p.PhysicianRoleId == id);
            return View(new PhysicianStructuredDataViewModel (model));
        }

        ///<summary>
        /// Edit fields of an Physician Role in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPhysicianRole(PhysicianStructuredDataViewModel model) 
        {
            model.PhysicianRole.LastModified = DateTime.Now;
            _repository.EditPhysicianRole(model.PhysicianRole);

            return RedirectToAction("ViewPhysicianRoles");
        }

        ///<summary>
        /// Delete an existing Physician Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePhysicianRole(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
   
            PhysicianRole model = _repository.PhysicianRoles.FirstOrDefault(rp => rp.PhysicianRoleId == id);

            if(model == null)
                { 
                    return NotFound();
                }

            return View(new PhysicianStructuredDataViewModel (model));
        }

        ///<summary>
        /// Delete an existing Physician Role in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePhysicianRole(PhysicianStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeletePhysicianRole(model.PhysicianRole); 

                return RedirectToAction("ViewPhysicianRoles"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Physician Role. Delete not available.";
                return View(model);                
            }            
        }
        #endregion  // end of Physician Role section

        #region Provider Types
        /// <summary>
        /// Index page of Provider Types
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewProviderTypes()
        {
            List<ProviderType> providerTypes = _repository.ProviderTypes.ToList();
            return View(providerTypes);
        }

        /// <summary>
        /// Create New Provider Type
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateProviderType()
        {
            return View();
        }

        ///<summary>
        /// Add new Provider Type to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateProviderType(PhysicianStructuredDataViewModel model)
        {
            model.ProviderType.LastModified = DateTime.Now;
            _repository.AddProviderType(model.ProviderType);

            return RedirectToAction("ViewProviderTypes");
        }

        ///<summary>
        /// Edit fields of an Provider Type
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditProviderType(int id) 
        {
            ProviderType model = _repository.ProviderTypes.FirstOrDefault(p => p.ProviderTypeId == id);
            return View(new PhysicianStructuredDataViewModel (model));
        }

        ///<summary>
        /// Edit fields of an Provider Type in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditProviderType(PhysicianStructuredDataViewModel model) 
        {
            model.ProviderType.LastModified = DateTime.Now;
            _repository.EditProviderType(model.ProviderType);

            return RedirectToAction("ViewProviderTypes");
        }

        ///<summary>
        /// Delete an existing Provider Type 
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteProviderType(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
 
            ProviderType model = _repository.ProviderTypes.FirstOrDefault(rp => rp.ProviderTypeId == id);
            return View(new PhysicianStructuredDataViewModel (model));
        }

        ///<summary>
        /// Delete an existing Provider Type in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteProviderType(PhysicianStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteProviderType(model.ProviderType); 

                return RedirectToAction("ViewProviderTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Physician Provider Type. Delete not available.";
                return View(model);                
            }            
        }
        #endregion  // end of Provider Type region

        #region Specialties
        /// <summary>
        /// Index page of Specialties
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewSpecialties()
        {
            List<Specialty> specialties = _repository.Specialties.ToList();
            return View(specialties);
        }

       /// <summary>
        /// Create New Specialty
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateSpecialty()
        {

            return View();
        }

        ///<summary>
        /// Add new Provider Type to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateSpecialty(PhysicianStructuredDataViewModel model)
        {
            model.Specialty.LastModified = DateTime.Now;
            _repository.AddSpecialty(model.Specialty);

            return RedirectToAction("ViewSpecialties");
        }

        ///<summary>
        /// Edit fields of an Specialty
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditSpecialty(int id) 
        {
            Specialty model = _repository.Specialties.FirstOrDefault(p => p.SpecialtyId == id);
            return View(new PhysicianStructuredDataViewModel (model));
        }

        ///<summary>
        /// Edit fields of an Specialty in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditSpecialty(PhysicianStructuredDataViewModel model) 
        {
            model.Specialty.LastModified = DateTime.Now;
            _repository.EditSpecialty(model.Specialty);

            return RedirectToAction("ViewSpecialties");
        }

        ///<summary>
        /// Delete an existing Specialty 
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteSpecialty(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
 
            Specialty model = _repository.Specialties.FirstOrDefault(rp => rp.SpecialtyId == id);
            return View(new PhysicianStructuredDataViewModel (model));
        }

        ///<summary>
        /// Delete an existing Specialty in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteSpecialty(PhysicianStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteSpecialty(model.Specialty); 

                return RedirectToAction("ViewSpecialties"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Physician Specialty. Delete not available.";
                return View(model);                
            }

        }
        #endregion  // end of Specialty section
    }


}