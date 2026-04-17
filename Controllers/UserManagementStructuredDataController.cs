using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.ViewModels.UserManagement;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography; // For SHA256
using System.Text; // For Encoding

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete","SecurityAssignRole","SecurityRemoveRole","SecurityResetPassword")]
    public class UserManagementStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public UserManagementStructuredDataController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IWCTCHealthSystemRepository repo) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _repository = repo;
        } 

        /// <summary>
        /// Index page of UserManagementStructuredData
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult Index() 
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View();
        }

        #region Facility    // Begin section for Facility
        /// <summary>
        /// Index page of Facilities
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewFacilities()
        {
            List<Facility> facilities = _repository.Facilities
                                .Include(p => p.Address)
                                .ThenInclude(a => a.Country)
                                .ToList();
            return View("Facility/ViewFacilities",facilities);
        }

        /// <summary>
        /// Create New Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFacility()
        {
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(_repository);
            ViewData["CountryId"] = AddressHelper.GetCountries(_repository);

            return View("Facility/CreateFacility");
        }

        ///<summary>
        /// Add new Facility to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateFacility(UserManagementStructuredDataViewModel model)
        {
            model.Facility.Phone = PhoneNumberHelper.FormatPhoneNumber(model.Facility.Phone);

            if(model.Address.Address1 != null)
            {
                Address address = new Address
                {
                    Address1 = model.Address.Address1,
                    Address2 = (model.Address.Address2 == null) ? "" : model.Address.Address2,
                    PostalCode = model.Address.PostalCode,
                    City = model.Address.City,
                    County = (model.Address.County == null) ? "" : model.Address.County,
                    AddressStateID = model.Address.AddressStateID,
                    CountryId = (model.Address.CountryId == 0) ? 1 : model.Address.CountryId,
                    LastModified = DateTime.Now
                };
                _repository.AddAddress(address);
                
                model.Facility.AddressId = address.AddressId;
                model.Facility.LastModified = DateTime.Now;

                _repository.AddFacility(model.Facility);

                return RedirectToAction("ViewFacilities");
            }
            else
            {
                model.Facility.AddressId = null;
                model.Facility.LastModified = DateTime.Now;

                _repository.AddFacility(model.Facility);

                return RedirectToAction("ViewFacilities");
            }


        }

        ///<summary>
        /// Edit fields of a Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFacility(int id) 
        {
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(_repository);
            ViewData["CountryId"] = AddressHelper.GetCountries(_repository);
            ViewData["ProviderTypeId"] = new SelectList(_repository.ProviderTypes, "ProviderTypeId", "Name");
            ViewData["SpecialtyId"] = new SelectList(_repository.Specialties, "SpecialtyId", "Name");
            ViewData["ProviderStatusId"] = new SelectList(_repository.ProviderStatuses, "ProviderStatusId", "Status");

            Facility facility = _repository.Facilities.FirstOrDefault(p => p.FacilityId == id);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == facility.AddressId);
            
            return View("Facility/EditFacility",new UserManagementStructuredDataViewModel (facility, address));
        }

        ///<summary>
        /// Edit fields of a Facility in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFacility(UserManagementStructuredDataViewModel model) 
        {
            model.Facility.Phone = PhoneNumberHelper.FormatPhoneNumber(model.Facility.Phone);

            if(model.Address.Address1 != null)
            {
                model.Address.LastModified = DateTime.Now;
                _repository.EditAddress(model.Address);

                model.Facility.AddressId = model.Address.AddressId;
                model.Facility.LastModified = DateTime.Now;
                _repository.EditFacility(model.Facility);

                return RedirectToAction("ViewFacilities");                
            }
            else
            {
                model.Facility.AddressId = null;
                model.Facility.LastModified = DateTime.Now;
                _repository.EditFacility(model.Facility);

                return RedirectToAction("ViewFacilities");
            }
        }

        ///<summary>
        /// View the Details of a Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult DetailsFacility(int id) 
        {
            Facility facility = _repository.Facilities.FirstOrDefault(p => p.FacilityId == id);

            Address address = null;
            AddressState addressState = null;
            Country country = null;
            
            if(facility != null && facility.AddressId.HasValue)
            {
                address = _repository.Addresses.FirstOrDefault(a => a.AddressId == facility.AddressId.Value);
                if(address != null)
                {
                    addressState = _repository.AddressStates.FirstOrDefault(a => a.StateID == address.AddressStateID);
                    country = _repository.Countries.FirstOrDefault(c => c.CountryId == address.CountryId);
                }
            }
            
            return View("Facility/DetailsFacility",new UserManagementStructuredDataViewModel (facility, address, addressState, country));
        }

        ///<summary>
        /// Delete an existing Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFacility(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
            
            Facility facility = _repository.Facilities.FirstOrDefault(p => p.FacilityId == id);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == facility.AddressId);

            if(facility == null)
                { 
                    return NotFound();
                }

            return View("Facility/DeleteFacility",new UserManagementStructuredDataViewModel (facility, address));
        }

        ///<summary>
        /// Delete an existing Facility in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFacility(UserManagementStructuredDataViewModel model) 
        {
            Facility facility = _repository.Facilities.FirstOrDefault(p => p.FacilityId == model.Facility.FacilityId);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == facility.AddressId);
            try
            {
                if(facility.AddressId != null)
                {
                    _repository.DeleteFacility(facility);
                    _repository.DeleteAddress(address);
                }
                else
                {
                    _repository.DeleteFacility(facility);
                }
 

                return RedirectToAction("ViewFacilities"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Facility. Delete not available.";
                return View(model);                
            }
        }
        #endregion // End section for Facility

        // Begin section for Permissions
        #region Permissions
        /// <summary>
        ///     View all Permissions
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPermissions()
        {
            List<AspNetPermission> permissions = _repository.AspNetPermissions.ToList();
            return View("Permissions/ViewPermissions",permissions);
        }

        /// <summary>
        ///     Create New Permission - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePermission()
        {
            return View("Permissions/CreatePermission");
        }

        ///<summary>
        ///     Add new Permission to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreatePermission(UserManagementStructuredDataViewModel model)
        {
            if (model.IsPermissionGroup) // If creating a permission group
            {
                var moduleOrForm = model.AspNetPermission.ModuleOrForm;
                if (string.IsNullOrWhiteSpace(moduleOrForm))
                {
                    ModelState.AddModelError("AspNetPermission.ModuleOrForm", "ModuleOrForm is required for creating a permission set.");
                    return View("Permissions/CreatePermission", model);
                }
                var permissionSet = new List<AspNetPermission>
                {
                    new AspNetPermission { Name = $"{moduleOrForm}Add", Description = $"Add a {moduleOrForm} functionality is accessible", ModuleOrForm = $"{moduleOrForm}" },
                    new AspNetPermission { Name = $"{moduleOrForm}Delete", Description = $"Delete a {moduleOrForm} functionality is accessible", ModuleOrForm = $"{moduleOrForm}" },
                    new AspNetPermission { Name = $"{moduleOrForm}Edit", Description = $"Edit a {moduleOrForm} functionality is accessible", ModuleOrForm = $"{moduleOrForm}" },
                    new AspNetPermission { Name = $"{moduleOrForm}View", Description = $"View a {moduleOrForm} functionality is accessible", ModuleOrForm = $"{moduleOrForm}" }
                };
                foreach (var permission in permissionSet)
                {
                    _repository.AddAspNetPermission(permission);
                }
            }
            else // if creating a single permission
            {
                if (string.IsNullOrWhiteSpace(model.AspNetPermission.Name) ||  
                    string.IsNullOrWhiteSpace(model.AspNetPermission.ModuleOrForm))
                {
                    ModelState.AddModelError(string.Empty, "Required fields for creating a single permission are missing.");
                    return View("Permissions/CreatePermission", model);
                }
                _repository.AddAspNetPermission(model.AspNetPermission);
            }

            return RedirectToAction("ViewPermissions");
        }

        ///<summary>
        ///     Edit an existing Permission - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPermission(int id) 
        {
            AspNetPermission permission = _repository.AspNetPermissions.FirstOrDefault(p => p.Id == id);
            return View("Permissions/EditPermission", new UserManagementStructuredDataViewModel (permission));
        }

        ///<summary>
        ///     Edit an existing Permission in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPermission(UserManagementStructuredDataViewModel model) 
        {
            _repository.EditAspNetPermission(model.AspNetPermission);

            return RedirectToAction("ViewPermissions");
        }

        ///<summary>
        ///     Delete an existing Permission - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePermission(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            AspNetPermission permission = _repository.AspNetPermissions.FirstOrDefault(p => p.Id == id);
            return View("Permissions/DeletePermission", new UserManagementStructuredDataViewModel (permission));
        }

        ///<summary>
        ///     Delete an existing Permission in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePermission(UserManagementStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteAspNetPermission(model.AspNetPermission);
                 

                return RedirectToAction("ViewPermissions"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Permission. Delete not available.";
                return View("Permissions/DeletePermission", model);                
            }

        }

        #endregion  // end of Permissions section

        // Begin section for Program
        #region Program
        /// <summary>
        /// Index page of Program
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult ViewPrograms()
        {
            // Fetch the logged-in user's permissions
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);

            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            if (permissions == null || !permissions.Any())
            {
                // Handle case where no permissions are found
                return Unauthorized();
            }

                // Check if the user has the "StructuredDataView" permission
            if (permissions.Contains("StructuredDataView"))
            {
                // User can see all programs
                List<IS_Proj_HIT.Entities.Program> programs = _repository.Programs.ToList();
                return View("Program/ViewPrograms",programs);
            }
            else if (permissions.Contains("SecurityAssignRole") || permissions.Contains("SecurityRemoveRole"))
            {
                // User should see only their Program
                // Retrieve FacilityId from the session
                var facilityIdString = HttpContext.Session.GetString("Facility");
                if (string.IsNullOrEmpty(facilityIdString))
                {
                    // Handle the case where Facility is not set in the session
                    return Unauthorized("Facility not set for the session.");
                }
                int facilityId = int.Parse(facilityIdString);

                // Fetch ProgramIds for the given FacilityId
                var programIds = _repository.ProgramFacilities
                    .Where(pf => pf.FacilityId == facilityId)
                    .Select(pf => pf.ProgramId)
                    .ToList();

                // If no ProgramIds are found, handle as needed
                if (!programIds.Any())
                {
                    // Optional: Handle no matching programs
                    return NotFound("No programs found for the provided Facility");
                }

                // Fetch Programs based on programIds
                List<IS_Proj_HIT.Entities.Program> programs = _repository.Programs
                    .Where(program => programIds.Contains(program.ProgramId))
                    .ToList();

                return View("Program/ViewPrograms",programs);
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Create New Program
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateProgram()
        {
            return View("Program/CreateProgram");
        }

        ///<summary>
        /// Add new Program to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateProgram(UserManagementStructuredDataViewModel model)
        {
            _repository.AddProgram(model.Program);

            return RedirectToAction("ViewPrograms");
        }

        ///<summary>
        /// Edit fields of an existing Program
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditProgram(int id) 
        {
            IS_Proj_HIT.Entities.Program program = _repository.Programs.FirstOrDefault(r => r.ProgramId == id);

            return View("Program/EditProgram",new UserManagementStructuredDataViewModel (program));
        }

        ///<summary>
        /// Edit fields of an existing Program in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditProgram(UserManagementStructuredDataViewModel model) 
        {
            _repository.EditProgram(model.Program);

            return RedirectToAction("ViewPrograms");
        }

        ///<summary>
        /// Delete an existing Program
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteProgram(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            IS_Proj_HIT.Entities.Program program = _repository.Programs.FirstOrDefault(r => r.ProgramId == id);
            return View("Program/DeleteProgram",new UserManagementStructuredDataViewModel (program));
        }

        ///<summary>
        /// Delete an existing Program in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteProgram(UserManagementStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteProgram(model.Program);
                 

                return RedirectToAction("ViewPrograms"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Program. Delete not available.";
                return View("Program/DeleteProgram", model);                
            }

        }
        #endregion  // End section for Program

        // Begin section for Program Facility
        #region Program Facility
        /// <summary>
        /// Index page of Program Facilities
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewProgramFacilities()
        {
            List<ProgramFacility> programFacilities = _repository.ProgramFacilities
                                .Include(p => p.Program)
                                .Include(p => p.Facility)
                                .ToList();
            return View("ProgramFacility/ViewProgramFacilities",programFacilities);
        }

        /// <summary>
        /// Create New Program Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateProgramFacility()
        {
            ViewData["ProgramId"] = new SelectList(_repository.Programs, "ProgramId", "Name");
            ViewData["FacilityId"] = new SelectList(_repository.Facilities, "FacilityId", "Name");

            return View("ProgramFacility/CreateProgramFacility");
        }

        ///<summary>
        /// Add new Program Facility to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateProgramFacility(UserManagementStructuredDataViewModel model)
        {
                _repository.AddProgramFacility(model.ProgramFacility);

                return RedirectToAction("ViewProgramFacilities");

        }

        ///<summary>
        /// Edit fields of a Program Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditProgramFacility(int id) 
        {
            ViewData["ProgramId"] = new SelectList(_repository.Programs, "ProgramId", "Name");
            ViewData["FacilityId"] = new SelectList(_repository.Facilities, "FacilityId", "Name");

            ProgramFacility programFacility = _repository.ProgramFacilities.FirstOrDefault(p => p.ProgramFacilitiesId == id);
            
            return View("ProgramFacility/EditProgramFacility",new UserManagementStructuredDataViewModel (programFacility));
        }

        ///<summary>
        /// Edit fields of a Program Facility in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditProgramFacility(UserManagementStructuredDataViewModel model) 
        {
            
                _repository.EditProgramFacility(model.ProgramFacility);

                return RedirectToAction("ViewProgramFacilities");                

        }

        ///<summary>
        /// Delete an existing Program Facility
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteProgramFacility(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
            
            ProgramFacility programFacility = _repository.ProgramFacilities
                .Include(pf => pf.Facility)
                .Include(pf => pf.Program)
                .FirstOrDefault(p => p.ProgramFacilitiesId == id);

            if(programFacility == null)
                { 
                    return NotFound();
                }

            var viewModel = new UserManagementStructuredDataViewModel
            {
                ProgramFacility = programFacility
            };


            return View("ProgramFacility/DeleteProgramFacility",viewModel);
        }

        ///<summary>
        /// Delete an existing Program Facility in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteProgramFacility(UserManagementStructuredDataViewModel model) 
        {
            ProgramFacility programFacility = _repository.ProgramFacilities.FirstOrDefault(p => p.ProgramFacilitiesId == model.ProgramFacility.ProgramFacilitiesId);

                    _repository.DeleteProgramFacility(programFacility);
 

                return RedirectToAction("ViewProgramFacilities"); 

        }
        #endregion  // End section for Program Facility

        // Begin section for Role
        #region Role
        /// <summary>
        /// Index page of Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult ViewRoles()
        {
            var roles = _roleManager.Roles;
            
            // Fetch the logged-in user's permissions
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);

            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            if (permissions == null || !permissions.Any())
            {
                // Handle case where no permissions are found
                return Unauthorized();
            }
            
            return View("Roles/ViewRoles",roles);
        }

        /// <summary>
        /// Create New Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRole()
        {
            return View("Roles/CreateRole");
        }

        ///<summary>
        /// Add new Role to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public async Task<IActionResult> CreateRole(UserManagementStructuredDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole
                {
                    Name = model.IdentityRole.Name,
                    ConcurrencyStamp = Guid.NewGuid().ToString() 
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                    return RedirectToAction("ViewRoles");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        ///<summary>
        /// Edit fields of an existing Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public async Task<IActionResult> EditRole(string id) 
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
                return View("NotFound");
            }

            return View("Roles/EditRole",new UserManagementStructuredDataViewModel (role));

        }

        ///<summary>
        /// Edit fields of an existing Role in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public async Task<IActionResult> EditRole(UserManagementStructuredDataViewModel model) 
        {
            var role = await _roleManager.FindByIdAsync(model.IdentityRole.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {model.IdentityRole.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.IdentityRole.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("ViewRoles");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        ///<summary>
        /// Delete an existing Role
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public async Task<IActionResult> DeleteRole(string id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found.";
                return View("NotFound");
            }

            return View("Roles/DeleteRole",new UserManagementStructuredDataViewModel (role));
        }

        ///<summary>
        ///     Delete an existing Role in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public async Task<IActionResult> DeleteRole(UserManagementStructuredDataViewModel model) 
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.IdentityRole.Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with ID = {model.IdentityRole.Id} cannot be found.";
                    return View("NotFound");
                }

                var result = await _roleManager.DeleteAsync(role);

                return RedirectToAction("ViewRoles");
                
 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Role. Delete not available.";
                return View("Roles/DeleteRole", model);                
            }

        }
        #endregion  // End section for Role

        // Begin section for RolePermissions
        #region RolePermissions

        /// <summary>
        ///     View assigned permissions by Role - table
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRolePermissions()
        {
            // Fetch roles and permissions from the database
            var roles = _repository.AspNetRoles
                            .Include(r => r.RolePermissions)
                                .ThenInclude(rp => rp.Permission)
                            .OrderBy(r => r.Name)
                            .ToList();

            var permissions = _repository.AspNetPermissions.ToList();

            // Prepare a view model with structured data
            var viewModel = roles.Select(role => new
            {
                RoleName = role.Name,
                Permissions = role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
            }).ToDictionary(r => r.RoleName, r => r.Permissions);

            ViewData["Permissions"] = permissions.Select(p => p.ModuleOrForm).Distinct().OrderBy(permission => permission).ToList();
            return View("RolePermissions/ViewRolePermissions", viewModel);
        }

        /// <summary>
        ///     Edit assigned Permissions by Role - Getter
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRolePermissions(string roleName)
        {
            // Fetch the selected role and its permissions
            var role = _repository.AspNetRoles
                                .Include(r => r.RolePermissions)
                                .ThenInclude(rp => rp.Permission)
                                .FirstOrDefault(r => r.Name == roleName);

            var allPermissions = _repository.AspNetPermissions.ToList();

            // Build the view model
            var viewModel = new UserManagementStructuredDataViewModel
            {
                RoleName = roleName,
                Permissions = allPermissions.Select(p =>
                {
                    var isSelected = role.RolePermissions.Any(rp => rp.PermissionId == p.Id);
                    return isSelected ? $"{p.Name}|Selected" : p.Name; // Indicate selection with a suffix
                }).ToList()
            };

            ViewData["Permissions"] = allPermissions.Select(p => p.ModuleOrForm).Distinct().OrderBy(permission => permission).ToList();
            return View("RolePermissions/EditRolePermissions", viewModel);
        }

        /// <summary>
        ///     Edit assigned Permissions by Role - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRolePermissions(UserManagementStructuredDataViewModel model)
        {
            // Fetch the selected role
            var role = _repository.AspNetRoles.FirstOrDefault(rn => rn.Name == model.RoleName);
            if (role == null)
            {
                return NotFound(); // Return error if role doesn't exist
            }

            // Fetch all existing permissions for the role
            var existingRolePermissions = _repository.AspNetRolePermissions.Where(rp => rp.RoleId == role.Id).ToList();

            // Initialize collections for permissions to add and delete
            var permissionsToAdd = new List<AspNetRolePermission>();
            var permissionsToDelete = new List<AspNetRolePermission>();

            // Normalize submitted permissions (remove "|Selected" suffix)
            var submittedPermissions = model.Permissions
                        .Select(p => p.Split('|')[0]) // Remove the suffix
                        .ToList();

            // Process the submitted permissions to identify additions
            foreach (var permissionName in submittedPermissions)
            {
                var permissionEntity = _repository.AspNetPermissions.FirstOrDefault(p => p.Name == permissionName);
                if (permissionEntity != null)
                {
                    var existingPermission = existingRolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionEntity.Id);

                    if (existingPermission == null)
                    {
                        // Add new permission
                        permissionsToAdd.Add(new AspNetRolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permissionEntity.Id
                        });
                    }
                }
            }
            
            // Process existing permissions to identify deletions
            foreach (var existingPermission in existingRolePermissions)
            {
                var permissionEntity = _repository.AspNetPermissions.FirstOrDefault(p => p.Id == existingPermission.PermissionId);
                if (permissionEntity != null)
                {
                    if (!submittedPermissions.Contains(permissionEntity.Name))
                    {
                        // Delete unchecked permission
                        permissionsToDelete.Add(existingPermission);
                    }
                }
            }            

            // Execute repository methods to update permissions
            foreach (var permission in permissionsToAdd)
            {
                _repository.AddAspNetRolePermission(permission);
            }

            foreach (var permission in permissionsToDelete)
            {
                _repository.DeleteAspNetRolePermission(permission);
            }

            return RedirectToAction("ViewRolePermissions"); // Redirect back to the initial view
        }

        #endregion  // end of RolePermissions section

        // Begin section for Security Question
        #region Security Question
        /// <summary>
        /// Index page of Security Question
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewSecurityQuestions()
        {
            List<IS_Proj_HIT.Entities.SecurityQuestion> securityQuestions = _repository.SecurityQuestions.ToList();
            return View("SecurityQuestions/ViewSecurityQuestions",securityQuestions);
        }

        /// <summary>
        /// Create New Security Question
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateSecurityQuestion()
        {
            return View("SecurityQuestions/CreateSecurityQuestion");
        }

        ///<summary>
        /// Add new Security Question to database
        /// </summary>
        [HttpPost]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateSecurityQuestion(UserManagementStructuredDataViewModel model)
        {
            _repository.AddSecurityQuestion(model.SecurityQuestion);

            return RedirectToAction("ViewSecurityQuestions");
        }

        ///<summary>
        /// Edit fields of an existing Security Question
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditSecurityQuestion(int id) 
        {
            IS_Proj_HIT.Entities.SecurityQuestion securityQuestion = _repository.SecurityQuestions.FirstOrDefault(r => r.SecurityQuestionId == id);

            return View("SecurityQuestions/EditSecurityQuestion",new UserManagementStructuredDataViewModel (securityQuestion));
        }

        ///<summary>
        /// Edit fields of an existing Security Question in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditSecurityQuestion(UserManagementStructuredDataViewModel model) 
        {
            _repository.EditSecurityQuestion(model.SecurityQuestion);

            return RedirectToAction("ViewSecurityQuestions");
        }

        ///<summary>
        /// Delete an existing Security Question
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteSecurityQuestion(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            IS_Proj_HIT.Entities.SecurityQuestion securityQuestion = _repository.SecurityQuestions.FirstOrDefault(r => r.SecurityQuestionId == id);
            return View("SecurityQuestions/DeleteSecurityQuestion",new UserManagementStructuredDataViewModel (securityQuestion));
        }

        ///<summary>
        /// Delete an existing Security Question in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteSecurityQuestion(UserManagementStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteSecurityQuestion(model.SecurityQuestion);
                 

                return RedirectToAction("ViewSecurityQuestions"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this SecurityQuestion. Delete not available.";
                return View("SecurityQuestions/DeleteSecurityQuestion", model);                
            }

        }
        #endregion  // End section for Security Question

        // Begin section for User maintenance
        #region User Maintenance

        /// <summary>
        ///     View All Users - Getter
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult ViewUsers()
        {
            // Fetch the logged-in user's permissions
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);

            if (permissions == null || !permissions.Any())
            {
                // Handle case where no permissions are found
                return Unauthorized();
            }

                // Check if the user has the "StructuredDataView" permission
            if (permissions.Contains("StructuredDataView"))
            {
                // User can see users in all programs
                var allUsers = _repository.UserTables.ToList();
                var model = new UserManagementStructuredDataViewModel
                {
                    Users = allUsers
                };
                return View("Users/ViewUsers", model);
            }
            else if (permissions.Contains("SecurityAssignRole") || permissions.Contains("SecurityRemoveRole"))
            {
                // User should only see what is in their Program
                // Retrieve FacilityId from the session
                var facilityIdString = HttpContext.Session.GetString("Facility");
                if (string.IsNullOrEmpty(facilityIdString))
                {
                    // Handle the case where Facility is not set in the session
                    return Unauthorized("Facility not set for the session.");
                }
                int facilityId = int.Parse(facilityIdString);

                // Fetch ProgramIds for the given FacilityId
                var programIds = _repository.ProgramFacilities
                    .Where(pf => pf.FacilityId == facilityId)
                    .Select(pf => pf.ProgramId)
                    .ToList();

                // If no ProgramIds are found, handle as needed
                if (!programIds.Any())
                {
                    // Optional: Handle no matching programs
                    return NotFound("No programs found for the provided Facility");
                }

                // Filter UserIds using UserPrograms
                var userIds = _repository.UserPrograms
                    .Where(up => programIds.Contains(up.ProgramId))
                    .Select(up => up.UserId)
                    .ToList();

                // Fetch Users based on filtered UserIds
                var filteredUsers = _repository.UserTables
                    .Where(user => userIds.Contains(user.UserId))
                    .ToList();

                // Create the ViewModel
                var model = new UserManagementStructuredDataViewModel
                {
                    Users = filteredUsers
                };

                return View("Users/ViewUsers", model);
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        ///     View the Details of a User - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult DetailsUser(int id)
        {
             // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // fetch the specified user record
            var detailedUser = _repository.UserTables
                .Include(u => u.UserPrograms)
                .ThenInclude(up => up.Program)
                .FirstOrDefault(u => u.UserId == id);

            var userPrograms = detailedUser.UserPrograms
                .OrderBy(up => up.Program.Name)
                .ToList();
            ViewBag.UserPrograms = userPrograms;

            if (detailedUser == null)
            {
                return NotFound();
            }

            var userRoleIds = _repository.AspNetUserRoles
                .Where(ur => ur.UserId == detailedUser.AspNetUsersId)
                .Select(ur => ur.RoleId)
                .ToList();  //AspNetUserRoles.RoleId in a list

            var userRoles = _repository.AspNetRoles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => new IdentityRole
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .OrderBy(role => role.Name)
                .ToList();

            var userFacilities = _repository.UserFacilities
                .Include(uf => uf.Facility)
                .Where(uf => uf.UserId == detailedUser.UserId)
                .OrderBy(uf => uf.Facility.Name)
                .ToList();  // UserFacility objects in a list

            // Map the user entity to the view model
            var viewModel = new UserManagementStructuredDataViewModel
            {
                Users = new List<UserTable> { detailedUser },
                Roles = userRoles, // Directly assign the roles retrieved
                UserFacilities = userFacilities
            };

            // Pass the view model to the view
            return View("Users/DetailsUser", viewModel);
        }

        /// <summary>
        ///     Edit a specific User - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit","SecurityAssignRole","SecurityRemoveRole")]
        public async Task<IActionResult> EditUser(int id)
        {
            // Fetch the specified user
            var user = _repository.UserTables
                .Include(u => u.UserPrograms)
                .ThenInclude(up => up.Program)
                .Include(u => u.UserFacilities)
                .ThenInclude(uf => uf.Facility)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            var userPrograms = user.UserPrograms.OrderBy(up => up.Program.Name).Select(up => up.ProgramId).ToList(); // Get user programs in memory

            var programList = _repository.Programs
                .ToList() // Fetch all programs into memory
                .OrderBy(p => p.Name)
                .Select(p => new SelectListItem
                {
                    Value = p.ProgramId.ToString(),
                    Text = p.Name,
                    Selected = userPrograms.Contains(p.ProgramId) // Perform the check in memory
                })
                .ToList();

            var userFacilities = user.UserFacilities.OrderBy(uf => uf.Facility.Name).Select(uf => uf.FacilityId).ToList(); // Get user facilities in memory

            var facilityList = _repository.Facilities
                .ToList()   // fetch all facilities into memory
                .OrderBy(f => f.Name)
                .Select(f => new SelectListItem
                {
                    Value = f.FacilityId.ToString(),
                    Text = f.Name,
                    Selected = userFacilities.Contains(f.FacilityId) // Perform the check in memory
                });

            // Fetch the user from ASP.NET Identity
            var identityUser = await _userManager.FindByIdAsync(user.AspNetUsersId); // Ensure this gets the IdentityUser instance
            if (identityUser == null)
            {
                return NotFound("Associated IdentityUser not found.");
            }

            // Fetch the user's currently assigned roles
            var currentRoles = await _userManager.GetRolesAsync(identityUser); // Current roles of the user (list of role names)

            // Fetch all possible roles from RoleManager
            var allRoles = _roleManager.Roles.ToList(); // Get all roles into memory (complete list)

            // Build the SelectList, marking which roles are already assigned to the user
            var roleList = allRoles.OrderBy(r => r.Name).Select(r => new SelectListItem
            {
                Value = r.Id,               // RoleId (unique identifier for the role)
                Text = r.Name,              // RoleName (displayed in the dropdown)
                Selected = currentRoles.Contains(r.Name) // Mark as selected if the user's roles include this role's name
            }).ToList();

            // Map data to the view model
            var viewModel = new UserManagementStructuredDataViewModel
            {
                Users = new List<UserTable> { user }
            };

            // Pass data to the view using ViewBag
            ViewBag.ProgramList = programList;
            ViewBag.RoleList = roleList;
            ViewBag.FacilityList = facilityList;

            return View("Users/EditUser", viewModel);
        }

        /// <summary>
        ///     Edit a specific User - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <param name="selectedPrograms"></param>
        /// <param name="selectedRoles"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit","SecurityAssignRole","SecurityRemoveRole")]
        public async Task<IActionResult> EditUser(UserManagementStructuredDataViewModel model, List<int> selectedPrograms, List<string> selectedRoles, List<int> selectedFacilities)
        {
            if (model.Users == null || !model.Users.Any())
            {
                return BadRequest("No user data provided.");
            }

            var user = model.Users.First(); // Assume editing one user at a time

            // Fetch the UserTable record
            var existingUser = _repository.UserTables
                .Include(u => u.UserPrograms)
                .Include(u => u.UserFacilities)
                .FirstOrDefault(u => u.UserId == user.UserId);

            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Update UserTable properties
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.StartDate = user.StartDate;
            existingUser.EndDate = user.EndDate;
            existingUser.LastModified = DateTime.UtcNow;
            _repository.EditUser(existingUser); // Save changes to UserTable

            // Update IdentityUser properties
            var identityUser = await _userManager.FindByIdAsync(existingUser.AspNetUsersId);
            if (identityUser == null)
            {
                return NotFound("Associated IdentityUser not found.");
            }

            identityUser.Email = user.Email;      // Synchronize email
            identityUser.UserName = user.Email;  // Synchronize username, if needed
            await _userManager.UpdateAsync(identityUser); // Save changes to IdentityUser

            var selectedRoleNames = new List<string>();
            foreach (var roleId in selectedRoles)
            {
                var role = await _roleManager.FindByIdAsync(roleId); // Fetch role by ID
                if (role != null)
                {
                    selectedRoleNames.Add(role.Name); // Get the role name
                }
            }

            // Update user roles
            var currentRoles = await _userManager.GetRolesAsync(identityUser);
            var rolesToAdd = selectedRoleNames.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(selectedRoleNames).ToList();

            foreach (var roleName in rolesToAdd)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    await _userManager.AddToRoleAsync(identityUser, roleName);
                }
            }

            foreach (var roleName in rolesToRemove)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, roleName);
                }
            }

            // Update user programs
            var currentPrograms = existingUser.UserPrograms.Select(up => up.ProgramId).ToList();
            var programsToAdd = selectedPrograms.Except(currentPrograms).ToList();
            var programsToRemove = currentPrograms.Except(selectedPrograms).ToList();

            foreach (var programId in programsToAdd)
            {
                // Construct the UserProgram object with the necessary details
                var programToAdd = new UserProgram
                {
                    UserId = user.UserId,       // The user ID
                    ProgramId = programId       // The program ID to be added
                };
                _repository.AddUserProgram(programToAdd);
            }

            foreach (var programId in programsToRemove)
            {
                var programToRemove = existingUser.UserPrograms.FirstOrDefault(up => up.ProgramId == programId);
                _repository.DeleteUserProgram(programToRemove);
            }

            // Update user facilities
            var currentFacilites = existingUser.UserFacilities.Select(uf => uf.FacilityId).ToList();
            var facilitiesToAdd = selectedFacilities.Except(currentFacilites).ToList();
            var facilitiesToRemove = currentFacilites.Except(selectedFacilities).ToList();

            foreach (var facilityId in facilitiesToAdd)
            {
                // Construct the UserFacility object with the necessary details
                var facilityToAdd = new UserFacility
                {
                    UserId = user.UserId,       // the user Id
                    FacilityId = facilityId     // the facility Id to be added
                };
                _repository.AddUserFacility(facilityToAdd);
            }

            foreach (var facilityId in facilitiesToRemove)
            {
                var facilityToRemove = existingUser.UserFacilities.FirstOrDefault(uf => uf.FacilityId == facilityId);
                _repository.DeleteUserFacility(facilityToRemove);
            }

            return RedirectToAction("DetailsUser", new { id = existingUser.UserId });
        }

        /// <summary>
        ///     Reset Password to a default - Setter
        ///     There isn't email notification so Administrator simply tells the user the password is reset and the default value
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("SecurityResetPassword")]
        public async Task<IActionResult> ResetPassword(int userId)
        {
            var aspNetUser = _repository.UserTables.FirstOrDefault(ut => ut.UserId == userId);
            if (aspNetUser == null)
            {
                TempData["Error"] = "User not found in UserTables.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }

            if (string.IsNullOrEmpty(aspNetUser.AspNetUsersId))
            {
                TempData["Error"] = "AspNetUsersId is not valid.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }
            
            var user = await _userManager.FindByIdAsync(aspNetUser.AspNetUsersId);
            if (user == null)
            {
                TempData["Error"] = "User not found in Identity database.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }

            var newPassword = "r3s3tP@ssw0rd";
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            
            if (result.Succeeded)
                {
                    TempData["Message"] = "Password has been reset successfully.  The temporary password is: r3s3tP@ssw0rd";
                }
                else
                {
                    TempData["Error"] = "Failed to reset the password.";
                }

            var existingUser = _repository.UserTables.FirstOrDefault(ut => ut.UserId == userId);
            return RedirectToAction("DetailsUser", new { id = existingUser.UserId });
        }

        /// <summary>
        ///     Administrator resets the user's security questions
        ///     If the user has forgotten, these are reset to defaults.  Then Admin tells the user to update their security questions.
        ///     No email notifications.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public async Task<IActionResult> ResetSecurityQuestions(int userId)
        {
            var aspNetUser = _repository.UserTables.FirstOrDefault(ut => ut.UserId == userId);
            if (aspNetUser == null)
            {
                TempData["Error"] = "User not found in UserTables.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }

            if (string.IsNullOrEmpty(aspNetUser.AspNetUsersId))
            {
                TempData["Error"] = "AspNetUsersId is not valid.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }

            var user = await _userManager.FindByIdAsync(aspNetUser.AspNetUsersId);
            if (user == null)
            {
                TempData["Error"] = "User not found in Identity database.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }

            // Fetch and delete security questions
            var userSecurityQuestions = _repository.UserSecurityQuestions.Where(u => u.UserId == userId).ToList();
            foreach (var question in userSecurityQuestions)
            {
                _repository.DeleteUserSecurityQuestion(question);
            }

            // Create placeholder questions to prevent the user from being unregistered
            var placeholderQuestions = new List<UserSecurityQuestion>
            {
                new UserSecurityQuestion { UserId = userId, SecurityQuestionId = 1, AnswerHash = HashText("PlaceholderAnswer") },
                new UserSecurityQuestion { UserId = userId, SecurityQuestionId = 2, AnswerHash = HashText("PlaceholderAnswer") },
                new UserSecurityQuestion { UserId = userId, SecurityQuestionId = 3, AnswerHash = HashText("PlaceholderAnswer") }
            };

            foreach (var question in placeholderQuestions)
            {
                _repository.AddUserSecurityQuestion(question);
            }

            TempData["Message"] = "Security questions have been reset successfully.";
            return RedirectToAction("DetailsUser", new { id = userId });

        }

        /// <summary>
        ///     Delete a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var aspNetUser = _repository.UserTables.FirstOrDefault(ut => ut.UserId == userId);
            if (aspNetUser == null)
            {
                TempData["Error"] = "User not found in UserTables.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }

            if (string.IsNullOrEmpty(aspNetUser.AspNetUsersId))
            {
                TempData["Error"] = "AspNetUsersId is not valid.";
                return RedirectToAction("DetailsUser", new { id = userId });
            }
            
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(aspNetUser.AspNetUsersId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("ViewUsers"); // Redirect back to the list of users
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "User successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete user.";
            }

            return RedirectToAction("ViewUsers"); // Redirect back to the list of users
        }

        /// <summary>
        ///     private method needed in resetting the Security Questions, creates the HashText part of the credential
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string HashText(string text)
        {
            using var myHash = SHA256.Create();
            var byteRaw = Encoding.UTF8.GetBytes(text);
            var byteResult = myHash.ComputeHash(byteRaw);

            return string.Concat(Array.ConvertAll(byteResult, h => h.ToString("X2")));
        }

        #endregion  // end of User maintenance section

        //  Begin section for Managing Users in Programs
        #region Users In Program
        
        /// <summary>
        ///     View all Users in the selected Program - Getter
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult ViewUsersInProgram(int programId)
        {
            List<UserProgram> userPrograms = _repository.UserPrograms
                                .Include(up => up.User)
                                .Include(up => up.Program)
                                .Where(up => up.ProgramId == programId)
                                .ToList();

            var program = _repository.Programs.FirstOrDefault(p => p.ProgramId == programId);
            ViewBag.Program = program;

            return View("UsersInProgram/ViewUsersInProgram", userPrograms);
        }

        /// <summary>
        ///     Add or Remove Users from the selected Program - Getter
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult EditUsersInProgram(int programId)
        {
            var program = _repository.Programs.FirstOrDefault(p => p.ProgramId == programId);
            if (program == null)
            {
                return NotFound("Program not found."); // Handle the null case
            }
            var allUsers = _repository.UserTables.ToList();

            //Determine which users are already in the program
            var usersInProgram = _repository.UserPrograms
                                .Include(up => up.User)
                                .Include(up => up.Program)
                                .Where(up => up.ProgramId == programId)
                                .ToList();
            var userIdsInProgram = usersInProgram.Select(u => u.UserId).ToHashSet();

            // Order the list so checked users are at the top
            var orderedUsers = allUsers.OrderByDescending(user => userIdsInProgram.Contains(user.UserId))
                                        .ThenBy(user => user.Email)
                                        .ToList();

            // Populate the view model
            var model = new UserManagementStructuredDataViewModel
            {
                Program = program,
                Users = orderedUsers
            };

            // Pass the role information and the users
            ViewBag.UsersInProgram = userIdsInProgram; // User IDs assigned to the program

            return View("UsersInProgram/EditUsersInProgram", model);
        }

        /// <summary>
        ///     Add or remove Users from a Program - Setter
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="selectedUsers"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult EditUsersInProgram(int programId, List<string> selectedUsers)
        {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Received Selected Users:");
            foreach (var user in selectedUsers) {
                Console.WriteLine($"Value (UserId): {user}, Type: {user.GetType()}"); // Log each user ID
            }
                Console.ResetColor();

            // Convert 'selectedUsers' from strings to integers
            var selectedUserIds = selectedUsers.Select(user =>
            {
                if (int.TryParse(user, out var userId)) 
                    return userId;
                else 
                    throw new Exception($"Invalid UserId: {user}");
            }).ToList();


            // Find the program by programId
            var program = _repository.Programs.FirstOrDefault(p => p.ProgramId == programId);
            if (program == null)
            {
                return NotFound("Program not found.");
            }

            // Get all users in the program currently
            var usersInProgram = _repository.UserPrograms
                                .Include(up => up.User)
                                .Include(up => up.Program)
                                .Where(up => up.ProgramId == programId)
                                .ToList();
            var userIdsInProgram = usersInProgram.Select(up => up.UserId).ToHashSet();

            // Add users to the role (those in the 'selectedUsers' list but not currently in the program)
            foreach (var userId in selectedUserIds.Where(u => !userIdsInProgram.Contains(u)))
            {
                var newUserProgram = new UserProgram
                {
                    UserId = userId,
                    ProgramId = programId
                };
                _repository.AddUserProgram(newUserProgram);
            }

            // Remove users from the program (those currently in the program but not in 'selectedUsers')
            foreach (var userProgram in usersInProgram.Where(up => !selectedUserIds.Contains(up.UserId)))
            {
                _repository.DeleteUserProgram(userProgram);
            }

            // use this redirect instead of standard RedirectToAction as it will allow the ajax and js to finish before doing the redirection;  needs code in the js as well
            return Json(new { success = true, redirectUrl = Url.Action("ViewUsersInProgram", new { programId }) });
        }

        #endregion  // end of Users in Programs section

        // Begin section for Managing Users in Roles
        #region Users in Roles
        /// <summary>
        ///     View all Users in a specified Role - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView","SecurityAssignRole","SecurityRemoveRole")]
        public async Task<IActionResult> ViewUsersInRole(string id)
        {
            // Find the role by ID
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
                return View("NotFound");
            }

            // Fetch users in the role using _userManager
            var identityUsers  = await _userManager.GetUsersInRoleAsync(role.Name);

            // Enrich IdentityUser data by querying UserTable using the repository
            var userTables = identityUsers.Select(identityUser =>
                _repository.UserTables.FirstOrDefault(u => u.AspNetUsersId == identityUser.Id) // Find the corresponding UserTable object
                ).ToList();

            var model = new UserManagementStructuredDataViewModel
            {
                IdentityRole = role,
                Users = userTables // Populate the view model with enriched UserTable data
            };
            return View("UsersInRole/ViewUsersInRole", model);
        }

        /// <summary>
        ///     Edit the Users In Role = getter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit","SecurityAssignRole","SecurityRemoveRole")]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            // Find the role by ID
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
                return View("NotFound");
            }

            // Get all users
            var allUsers = _repository.UserTables.ToList();

            // Determine which users are already in the role
            var identityUsersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var userIdsInRole = identityUsersInRole.Select(u => u.Id).ToHashSet();

            // Order users: checked users at the top
            var orderedUsers = allUsers
                .OrderByDescending(user => userIdsInRole.Contains(user.AspNetUsersId))
                .ThenBy(user => user.Email)
                .ToList();

            // Populate the view model
            var model = new UserManagementStructuredDataViewModel
            {
                IdentityRole = role,
                Users = orderedUsers
            };

            // Pass the role information and the users
            ViewBag.UsersInRole = userIdsInRole; // User IDs assigned to the role

            return View("UsersInRole/EditUsersInRole", model);
        }

        /// <summary>
        ///     Edit the Users In Role - Setter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectedUsers"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit","SecurityAssignRole","SecurityRemoveRole")]
        public async Task<IActionResult> EditUsersInRole(string id, List<string> selectedUsers)
        {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Received Selected Users:");
            foreach (var user in selectedUsers) {
                Console.WriteLine(user); // Log each user ID
                    Console.ResetColor();
            }

            // Find the role by ID
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
                return View("NotFound");
            }

            // Get all users in the role currently
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            // Add users to the role (those in the 'selectedUsers' list but not currently in the role)
            foreach (var userId in selectedUsers.Where(u => !usersInRole.Any(ur => ur.Id == u)))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            // Remove users from the role (those in the role currently but not in 'selectedUsers' list)
            foreach (var user in usersInRole.Where(ur => !selectedUsers.Contains(ur.Id)))
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            // use this redirect instead of standard RedirectToAction as it will allow the ajax and js to finish before doing the redirection;  needs code in the js as well
            return Json(new { success = true, redirectUrl = Url.Action("ViewUsersInRole", new { id }) });

        }

        #endregion

    }
}