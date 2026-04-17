using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.ViewModels;
using System;
using IS_Proj_HIT.Services;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("RequesterAdd","RequesterView","RequesterEdit","RequesterDelete")]
    public class RequesterController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        
        public RequesterController(IWCTCHealthSystemRepository repo) 
        {
            _repository = repo;
        } 

        /// <summary>
        ///     List view of Requesters - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("RequesterAdd","RequesterView","RequesterEdit","RequesterDelete")]
        public IActionResult ViewRequesters()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            List<Requester> requesters = _repository.Requesters
                                .Include(p => p.Address)
                                .ThenInclude(a => a.Country)
                                .Include(p => p.RequesterType)
                                .Include(p => p.RequesterStatus).ToList();
            return View(requesters);
        }

        /// <summary>
        ///     Getter - Create New Requester
        /// </summary>
        [Authorize]
        [PermissionAuthorize("RequesterAdd")]
        public IActionResult CreateRequester()
        {
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(_repository);
            ViewData["CountryId"] = AddressHelper.GetCountries(_repository);

            var requesterTypes = new SelectList(_repository.RequesterTypes, "RequesterTypeId", "RequesterType1");
                var rTypesList = requesterTypes.ToList();
                rTypesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["RequesterTypeId"] = rTypesList;

            var statuses = new SelectList(_repository.RequesterStatuses, "RequesterStatusId", "RequesterStatus1");
                var statusList = statuses.ToList();
                statusList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["RequesterStatusId"] = statusList;

            return View();
        }

        ///<summary>
        ///     Add new Requester to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("RequesterAdd")] 
        public IActionResult CreateRequester(RequestersViewModel model)
        {
            model.Requester.PhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.Requester.PhoneNumber);
            
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
                };
                _repository.AddAddress(address);
                
                model.Requester.AddressId = address.AddressId;

                _repository.AddRequester(model.Requester);

                return RedirectToAction("ViewRequesters");
            }
            else
            {
                model.Requester.AddressId = null;

                _repository.AddRequester(model.Requester);

                return RedirectToAction("ViewRequesters");
            }
        }

        ///<summary>
        ///     View the Details of a Requester - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("RequesterView")]
        public IActionResult DetailsRequester(int id) 
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            Requester requester = _repository.Requesters.FirstOrDefault(p => p.RequesterId == id);

            Address address = null;
            AddressState addressState = null;
            Country country = null;
            
            if(requester != null && requester.AddressId.HasValue)
            {
                address = _repository.Addresses.FirstOrDefault(a => a.AddressId == requester.AddressId.Value);
                if(address != null)
                {
                    addressState = _repository.AddressStates.FirstOrDefault(a => a.StateID == address.AddressStateID);
                    country = _repository.Countries.FirstOrDefault(c => c.CountryId == address.CountryId);
                }
            }
              
            RequesterStatus requesterStatus = _repository.RequesterStatuses.FirstOrDefault(ps => ps.RequesterStatusId == requester.RequesterStatusId);
            RequesterType requesterType = _repository.RequesterTypes.FirstOrDefault(pt => pt.RequesterTypeId == requester.RequesterTypeId);
            
            return View(new RequestersViewModel (requester, address, addressState, country, requesterStatus, requesterType));
        }

        ///<summary>
        ///     Edit fields of an Requester - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("RequesterEdit")]
        public IActionResult EditRequester(int id) 
        {
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(_repository);
            ViewData["CountryId"] = AddressHelper.GetCountries(_repository);

            ViewData["RequesterTypeId"] = new SelectList(_repository.RequesterTypes, "RequesterTypeId", "RequesterType1");
            
            ViewData["RequesterStatusId"] = new SelectList(_repository.RequesterStatuses, "RequesterStatusId", "RequesterStatus1");

            Requester requester = _repository.Requesters.FirstOrDefault(p => p.RequesterId == id);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == requester.AddressId);
            RequesterStatus requesterStatus = _repository.RequesterStatuses.FirstOrDefault(r => r.RequesterStatusId == requester.RequesterStatusId);
            
            return View(new RequestersViewModel (requester, address, requesterStatus));
        }

        ///<summary>
        ///     Edit fields of an Requester in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("RequesterEdit")]
        public IActionResult EditRequester(RequestersViewModel model) 
        {
            model.Requester.PhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.Requester.PhoneNumber);
            
            if(model.Address.Address1 != null)
            {
                model.Address.LastModified = DateTime.Now;
                _repository.EditAddress(model.Address);

                model.Requester.AddressId = model.Address.AddressId;
                
                _repository.EditRequester(model.Requester);

                return RedirectToAction("ViewRequesters");                
            }
            else
            {
                model.Requester.AddressId = null;
                _repository.EditRequester(model.Requester);

                return RedirectToAction("ViewRequesters");
            }


        }

        ///<summary>
        ///     Delete an existing Requester - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("RequesterDelete")]
        public IActionResult DeleteRequester(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
            
            Requester requester = _repository.Requesters.FirstOrDefault(p => p.RequesterId == id);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == requester.AddressId);
            RequesterStatus requesterStatus = _repository.RequesterStatuses.FirstOrDefault(ps => ps.RequesterStatusId == requester.RequesterStatusId);

            if(requester == null)
                { 
                    return NotFound();
                }

            return View(new RequestersViewModel (requester, address, requesterStatus));
        }

        ///<summary>
        ///     Delete an existing Requester in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("RequesterDelete")]
        public IActionResult DeleteRequester(RequestersViewModel model) 
        {
            Requester requester = _repository.Requesters.FirstOrDefault(p => p.RequesterId == model.Requester.RequesterId);
            Address address = _repository.Addresses.FirstOrDefault(a => a.AddressId == requester.AddressId);

            try
            {
                if(requester.AddressId != null)
                {
                    _repository.DeleteRequester(requester);
                    _repository.DeleteAddress(address);  
                }
                else
                {
                    _repository.DeleteRequester(requester);
                }
 

                return RedirectToAction("ViewRequesters"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Requester. Delete not available.";
                return View(model);                
            }
        }

    }
}