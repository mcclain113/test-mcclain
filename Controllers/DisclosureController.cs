using IS_Proj_HIT.ViewModels.Disclosure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("RequesterAdd","RequesterView","DisclosureRequestAdd","DisclosureRequestEdit","DisclosureRequestDelete","DisclosureRequestView")]
    public class DisclosureController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public int PageSize = 8;
        public DisclosureController(IWCTCHealthSystemRepository repo, UserManager<IdentityUser> userManager)
        {
            _repository = repo;
            _userManager = userManager;
        }

        [Authorize]
        [PermissionAuthorize("DisclosureRequestView")]
        public ViewResult RequestList()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var requests = _repository.Requests
                .Include(r => r.Requester)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestPriority)
                .Include(r => r.RequestReleaseFormat)
                .Include(r => r.RequestedItems)
                .Include(r => r.RequestStatus)
                .Include(r => r.CompletedByNavigation)
                .AsNoTracking()
                .OrderBy(r => r.RequestId)
                .ToList();

            return View(new RequestListViewModel
            {
                Requests = requests
            });
        }

        /// <summary>
        /// Add/Create Request segment of Disclosure Controller - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("DisclosureRequestAdd","RequesterAdd")]
        public IActionResult CreateRequest()
        {
            var aspNetUsersId = GetCurrentUserId();
            var user = GetUserFromAspNetUsersId(aspNetUsersId);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            var model = new CreateRequestViewModel
            {
                Request = new Request
                {
                    DateCreated = DateOnly.FromDateTime(DateTime.Now),
                    RequestDate = DateOnly.FromDateTime(DateTime.Now),
                    EnteredBy = user.UserId,
                    CompletedBy = null
                },
                EnteredByNavigation = user,
                CompletedByNavigation = null,
                EnteredByUserId = user.UserId,
                EnteredByFullName = $"{user.FirstName} {user.LastName}",
                CompletedByUserId = null,
                CompletedByFullName = null
            };

            PopulateSelectLists(model);
            return View(model);
        }

        /// <summary>
        /// Post a New Request to the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestAdd","RequesterAdd")]
        public IActionResult CreateRequest(CreateRequestViewModel model)
        {
            try
            {
                var request = MapToRequest(model);

                // Check if the PatientRepresentativeId is set from the frontend
                if (model.PatientRepresentativeId.HasValue)
                {
                    request.PatientRepresentativeId = model.PatientRepresentativeId.Value;
                }
                else
                {
                    // Handle Patient Representative creation if needed
                    HandlePatientRepresentative(model, request);
                }

                // Save the request entity to ensure it has a generated primary key
                _repository.AddRequest(request);

                // handle requested items and populate the join table
                HandleRequestedItems(model, request);

                // Redirect to RequestList after successful submission to clear the form
                return RedirectToAction("RequestList");
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while saving the request." });
            }
        }

        /// <summary>
        /// Add/Create Request segment of Disclosure Controller:  Post a New Requester to the database when created within the CreateRequest activity
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("RequesterAdd")]
        public IActionResult AddRequester([FromBody] CreateRequestViewModel model)
        {
            // strip the mask from the Requester phone number
            model.Requester.PhoneNumber = string.IsNullOrEmpty(model.Requester.PhoneNumber) 
                                            ? null 
                                            : PhoneNumberHelper.FormatPhoneNumber(model.Requester.PhoneNumber);

            try
            {
                // Create and save the address
                var newAddress = new Address
                {
                    Address1 = model.Address?.Address1,
                    Address2 = model.Address?.Address2,
                    City = model.Address?.City,
                    AddressStateID = model.Address.AddressStateID,
                    PostalCode = model.Address?.PostalCode,
                    CountryId = model.Address?.CountryId ?? 0,
                    LastModified = DateTime.Now
                };

                _repository.AddAddress(newAddress);

                // Create the requester with the saved address
                var newRequester = new Requester
                {
                    FirstName = model.Requester?.FirstName,
                    LastName = model.Requester?.LastName,
                    CompanyName = model.Requester?.CompanyName,
                    EmailAddress = model.Requester?.EmailAddress,
                    PhoneNumber = model.Requester?.PhoneNumber,
                    RequesterTypeId = model.Requester?.RequesterTypeId ?? 0,
                    RequesterStatusId = model.Requester?.RequesterStatusId ?? "",
                    AddressId = newAddress.AddressId
                };

                _repository.AddRequester(newRequester);

                return Json(new { success = true, requesterId = newRequester.RequesterId, fullName = $"{newRequester.FirstName} {newRequester.LastName}" });
            }
            catch
            {
                return Json(new { success = false, message = "Failed to add requester." });
            }
        }

        /// <summary>
        /// Add/Create Request segment of Disclosure Controller:  Post a new Patient Representative to the database when one is created during the CreateRequest activity
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Disclosure/AddPatientRepresentative")]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestAdd")]
        public IActionResult AddPatientRepresentative([FromBody] CreateRequestViewModel model)
        {
            // strip the mask from the Patient Representative phone number
            model.PatientRepresentative.RepresentativePhoneNumber = 
                string.IsNullOrEmpty(model.PatientRepresentative.RepresentativePhoneNumber) 
                ? null 
                : PhoneNumberHelper.FormatPhoneNumber(model.PatientRepresentative.RepresentativePhoneNumber);

            try
            {
                // Validate required fields
                if (model.PatientRepresentative == null ||
                    string.IsNullOrWhiteSpace(model.PatientRepresentative.RepresentativeFirstName) ||
                    string.IsNullOrWhiteSpace(model.PatientRepresentative.RepresentativeLastName))
                {
                    return Json(new { success = false, message = "First Name and Last Name are required." });
                }

                // Create and save the Patient Representative
                var newRepresentative = new PatientRepresentative
                {
                    RepresentativeFirstName = model.PatientRepresentative.RepresentativeFirstName,
                    RepresentativeLastName = model.PatientRepresentative.RepresentativeLastName,
                    RepresentativePhoneNumber = model.PatientRepresentative.RepresentativePhoneNumber,
                    RepresentativeEmailAddress = model.PatientRepresentative.RepresentativeEmailAddress,
                    Comments = model.PatientRepresentative.Comments
                };

                _repository.AddPatientRepresentative(newRepresentative);

                // Return success with the new representative ID
                return Json(new { success = true, representativeId = newRepresentative.RepresentativeId });
            }
            catch
            {
                return Json(new { success = false, message = "Failed to add patient representative." });
            }
        }

        /// <summary>
        /// Add/Create Request segment of Disclosure Controller:  mapping method
        /// </summary>
        private Request MapToRequest(CreateRequestViewModel model)
        {
            var request = model.Request;

            // Set IsPatientInSystem
            request.IsPatientInSystem = !string.IsNullOrEmpty(request.PatientMrn);

            // Retrieve the FacilityId from UserFacility for the current user
            var aspNetUserId = GetCurrentUserId();
            var user = _repository.UserTables.Include(u => u.UserFacilities)
                                      .FirstOrDefault(u => u.AspNetUsersId == aspNetUserId);

            if (user != null)
            {
                var userFacility = user.UserFacilities.FirstOrDefault();
                if (userFacility != null)
                {
                    request.FacilityId = userFacility.FacilityId;
                }
            }

            // Check if the patient is from the Patients table
            if (request.IsPatientInSystem.GetValueOrDefault() && !string.IsNullOrEmpty(request.PatientMrn))
            {
                var patient = _repository.Patients
                                 .Where(p => p.Mrn == request.PatientMrn)
                                 .Select(p => new
                                 {
                                     p.FirstName,
                                     p.LastName,
                                     p.MiddleName,
                                     p.Dob
                                 })
                                 .FirstOrDefault();

                if (patient == null)
                {
                    throw new Exception("Patient not found in the system.");
                }

                var contactDetails = _repository.PatientContactDetails.FirstOrDefault(cd => cd.Mrn == request.PatientMrn);

                // Populate patient details from the Patients table
                request.PatientFirstName = patient.FirstName;
                request.PatientLastName = patient.LastName;
                request.PatientMiddleName = patient.MiddleName;
                request.PatientDob = patient.Dob.HasValue ? DateOnly.FromDateTime(patient.Dob.Value) : null;

                if (contactDetails != null)
                {
                    request.PatientPhoneNumber = contactDetails.CellPhone ?? contactDetails.HomePhone ?? contactDetails.WorkPhone ?? null;
                    request.PatientEmailAddress = contactDetails.EmailAddress ?? null;
                }
            }
            else
            {
                // Handle patients that do not have an MRN but were previously added to a request

                // strip the mask from the Patient Representative phone number
                model.Request.PatientPhoneNumber = 
                    string.IsNullOrEmpty(model.Request.PatientPhoneNumber) 
                    ? null 
                    : PhoneNumberHelper.FormatPhoneNumber(model.Request.PatientPhoneNumber);
                request.PatientFirstName = model.Request.PatientFirstName;
                request.PatientLastName = model.Request.PatientLastName;
                request.PatientDob = model.Request.PatientDob;
                request.PatientPhoneNumber = model.Request.PatientPhoneNumber;
                request.PatientEmailAddress = model.Request.PatientEmailAddress;
            }

            // Additional form properties
            request.DateCreated = model.Request.DateCreated;
            request.RequestDate = model.Request.RequestDate;
            request.EnteredBy = model.EnteredByUserId ?? request.EnteredBy;
            request.CompletedBy = model.CompletedByUserId ?? request.CompletedBy;
            request.IsAuthorizationValid = model.IsAuthorizationValidChecked;
            request.IsSpecialAuthorizationRequired = model.IsSpecialAuthorizationRequiredChecked;
            request.IsAttestationRequired = model.IsAttestationRequiredChecked;
            request.IsTpodisclosure = model.IsTpoDisclosureChecked;
            request.IsSubpoena = model.IsSubpoenaChecked;
            request.IsCourtOrder = model.IsCourtOrderChecked;
            request.IsRequestForPersonalAppearance = model.IsRequestForPersonalAppearanceChecked;
            request.IsRequestForOriginalRecords = model.IsRequestForOriginalRecordsChecked;
            request.IsRequestForCertifiedRecords = model.IsRequestForCertifiedRecordsChecked;
            request.IsRiskManagementAlert = model.IsRiskManagementAlertChecked;
            request.CourtOrderIssueDate = model.Request.CourtOrderIssueDate;
            request.HearingDate = model.Request.HearingDate;
            request.CaseNumber = model.Request.CaseNumber;
            request.Plaintiff = model.Request.Plaintiff;
            request.Defendant = model.Request.Defendant;
            request.Comments = model.Request.Comments;

            return request;
        }

        /// <summary>
        /// Add/Create Request segment of Disclosure Controller:  managing Patient Representative during Create Request activity
        /// </summary>
        private void HandlePatientRepresentative(CreateRequestViewModel model, Request request)
        {
            // Check if the PatientRepresentative is provided and has required fields
            if (model.PatientRepresentative != null &&
                !string.IsNullOrWhiteSpace(model.PatientRepresentative.RepresentativeFirstName) &&
                !string.IsNullOrWhiteSpace(model.PatientRepresentative.RepresentativeLastName))

            {
                // Create and save the Patient Representative
                var newRepresentative = new PatientRepresentative
                {
                    RepresentativeFirstName = model.PatientRepresentative.RepresentativeFirstName,
                    RepresentativeLastName = model.PatientRepresentative.RepresentativeLastName,
                    RepresentativePhoneNumber = model.PatientRepresentative.RepresentativePhoneNumber,
                    RepresentativeEmailAddress = model.PatientRepresentative.RepresentativeEmailAddress,
                    Comments = model.PatientRepresentative.Comments
                };

                _repository.AddPatientRepresentative(newRepresentative);

                // Associate the new representative with the request
                request.PatientRepresentativeId = newRepresentative.RepresentativeId;
            }
        }

        /// <summary>
        ///     Delete a Patient Representative from the database while in CreateRequest
        /// </summary>
        [HttpPost]
        public JsonResult RemovePatientRepresentative([FromBody] JsonElement json)
        {
            try
            {
                // Extract the 'id' from the JsonElement and parse it as an integer
                int id = int.Parse(json.GetProperty("id").GetString());
                
                var patientRepresentative = _repository.PatientRepresentatives
                    .FirstOrDefault(pr => pr.RepresentativeId == id);
                if (patientRepresentative != null)
                {
                    _repository.DeletePatientRepresentative(patientRepresentative);
                    
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Patient Representative not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return Json(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Details and Edit Request segment of Disclosure Controller
        /// </summary>
        //Initialize RequestDetailsViewModel
        private RequestDetailsViewModel GetRequestDetailsViewModel(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var request = _repository.Requests
                .Include(r => r.Requester)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestPriority)
                .Include(r => r.RequestReleaseFormat)
                .Include(r => r.RequestedItems)
                    .ThenInclude(ri => ri.DocumentRequested)
                .Include(r => r.RequestedItems)
                    .ThenInclude(ri => ri.ItemStatus)
                .Include(r => r.RequestStatus)
                .Include(r => r.RequestStatusReason)
                .Include(r => r.CompletedByNavigation)
                .AsNoTracking()
                .FirstOrDefault(r => r.RequestId == id);

            if (request == null)
            {
                return null;
            }

            var requester = _repository.Requesters
                .Include(r => r.Address)
                .ThenInclude(a => a.AddressState)
                .Include(r => r.Address)
                .ThenInclude(c => c.Country)
                .Include(r => r.RequesterStatus)
                .Include(r => r.RequesterType)
                .AsNoTracking()
                .FirstOrDefault(r => r.RequesterId == request.RequesterId);

            var requestReleaseFormat = _repository.RequestReleaseFormats
                .AsNoTracking()
                .FirstOrDefault(rf => rf.ReleaseFormatId == request.RequestReleaseFormatId);

            var patient = _repository.Patients
                .AsNoTracking()
                .FirstOrDefault(p => p.Mrn == request.PatientMrn);

            var patientRepresentative = _repository.PatientRepresentatives
                .AsNoTracking()
                .FirstOrDefault(pr => pr.RepresentativeId == request.PatientRepresentativeId);

            var disclosure = _repository.Disclosures
                .Include(d => d.DisclosurePayments)
                    .ThenInclude(dp => dp.PaymentType)
                .Include(d => d.Request)
                .Include(d => d.Requester)
                .AsNoTracking()
                .FirstOrDefault(d => d.RequestId == request.RequestId);

            var disclosureFees = (disclosure != null) ?_repository.DisclosureFees
                .Include(df => df.DisclosureFeeType)
                .Where(df => df.DisclosureId == disclosure.DisclosureId)
                .ToList() : null;

            var documentRequestedOptions = _repository.DocumentRequesteds
                .Select(d => new ExtendedSelectListItem
                {
                    Value = d.DocumentRequestedId.ToString(),
                    Text = d.DocumentRequested1,
                    DataAttributes = new Dictionary<string, string> {
                        { "data-description", d.DocumentRequestedDescription }
                    }
                })
                .ToList();

            var itemStatusOptions = _repository.ItemStatuses
                .Select(s => new ExtendedSelectListItem
                {
                    Value = s.ItemStatusId,
                    Text = s.ItemStatus1,
                    DataAttributes = new Dictionary<string, string> {
                        { "data-description", s.ItemStatusDescription }
                    }

                })
                .ToList();

            var requestPurposes = _repository.RequestPurposes
                .Select(requestPurposes => new SelectListItem
                {
                    Value = requestPurposes.PurposeId.ToString(),
                    Text = requestPurposes.RequestPurpose1
                })
                .ToList();

            var requesters = _repository.Requesters
                .OrderBy(r => r.LastName)
                .Select(requesters => new SelectListItem
                {
                    Value = requesters.RequesterId.ToString(),
                    Text = requesters.FirstName + " " + requesters.LastName
                })
                .ToList();

            var requestPriorities = _repository.RequestPriorities
                .Select(requestPriorities => new SelectListItem
                {
                    Value = requestPriorities.PriorityId.ToString(),
                    Text = requestPriorities.RequestPriority1
                })
                .ToList();

            var requestStatuses = _repository.RequestStatuses
                .Select(requestStatuses => new SelectListItem
                {
                    Value = requestStatuses.RequestStatusId.ToString(),
                    Text = requestStatuses.RequestStatus1
                })
                .ToList();

            var requestStatusReasons = _repository.RequestStatusReasons
                .Select(requestStatusReasons => new SelectListItem
                {
                    Value = requestStatusReasons.RequestStatusReasonId.ToString(),
                    Text = requestStatusReasons.RequestStatusReason1
                })
                .ToList();

            var requesterTypes = _repository.RequesterTypes
                .Select(requesterTypes => new SelectListItem
                {
                    Value = requesterTypes.RequesterTypeId.ToString(),
                    Text = requesterTypes.RequesterType1
                })
                .ToList();

            var requesterStatuses = _repository.RequesterStatuses
                .Select(requesterStatuses => new SelectListItem
                {
                    Value = requesterStatuses.RequesterStatusId.ToString(),
                    Text = requesterStatuses.RequesterStatus1
                })
                .ToList();

            var requestReleaseFormats = _repository.RequestReleaseFormats
                .Select(requestReleaseFormats => new SelectListItem
                {
                    Value = requestReleaseFormats.ReleaseFormatId.ToString(),
                    Text = requestReleaseFormats.RequestReleaseFormat1
                })
                .ToList();

            var userTableList = _repository.UserTables
                .Where(cc => cc.FirstName != "OBSOLETE" && cc.LastName != "OBSOLETE") 
                .OrderBy(cc => cc.LastName)
                .Select(cc => new SelectListItem
                {
                    Value = cc.UserId.ToString(),
                    Text = cc.FirstName + " " + cc.LastName
                })
                .ToList();

            var enteredBy = _repository.UserTables
                .FirstOrDefault(u => u.UserId == request.EnteredBy);

            var createdBy = _repository.UserTables
                .FirstOrDefault(u => u.UserId == request.EnteredBy);

            var completedBy = _repository.UserTables
                .FirstOrDefault(u => u.UserId == request.CompletedBy);

            var patientRepresentatives = _repository.PatientRepresentatives
                .Select(patientRepresentatives => new SelectListItem
                {
                    Value = patientRepresentatives.RepresentativeId.ToString(),
                    Text = patientRepresentatives.RepresentativeFirstName + " " + patientRepresentatives.RepresentativeLastName
                })
                .ToList();

            var paymentTypes = _repository.PaymentTypes
                .Select(paymentTypes => new SelectListItem
                {
                    Value = paymentTypes.PaymentTypeId.ToString(),
                    Text = paymentTypes.PaymentType1
                })
                .ToList();

            var disclosureFeeTypeIdsInUse = disclosure?.DisclosureFeeTypes
                .Select(dft => dft.DisclosureFeeTypeId)
                .ToHashSet() ?? new HashSet<int>();

            var disclosureFeeAmounts = _repository.DisclosureFeeTypes
                .Where(dft => !disclosureFeeTypeIdsInUse.Contains(dft.DisclosureFeeTypeId))
                .OrderBy(dft => dft.SortOrder)
                .Select(disclosureFeeTypes => new SelectListItem
                {
                    Value = disclosureFeeTypes.DisclosureFeeTypeId.ToString(),
                    Text = disclosureFeeTypes.FeeAmount.ToString()
                })
                .ToList();

            var disclosureFeeDescriptions = _repository.DisclosureFeeTypes
                .Where(dft => !disclosureFeeTypeIdsInUse.Contains(dft.DisclosureFeeTypeId))
                .OrderBy(dft => dft.SortOrder)
                .Select(disclosureFeeDescriptions => new SelectListItem
                {
                    Value = disclosureFeeDescriptions.DisclosureFeeTypeId.ToString(),
                    Text = disclosureFeeDescriptions.FeeDescription.ToString()
                })
                .ToList();

            var disclosureFeeComments = _repository.DisclosureFeeTypes
                .Where(dft => !disclosureFeeTypeIdsInUse.Contains(dft.DisclosureFeeTypeId))
                .OrderBy(dft => dft.SortOrder)
                .Select(disclosureFeeComments => new SelectListItem
                {
                    Value = disclosureFeeComments.DisclosureFeeTypeId.ToString(),
                    Text = disclosureFeeComments.Comments
                })
                .ToList();

            var disclosureFeeEffectiveDates = _repository.DisclosureFeeTypes
                .Where(dft => !disclosureFeeTypeIdsInUse.Contains(dft.DisclosureFeeTypeId))
                .Select(disclosureFeeEffectiveDates => new SelectListItem
                {
                    Value = disclosureFeeEffectiveDates.DisclosureFeeTypeId.ToString(),
                    Text = disclosureFeeEffectiveDates.EffectiveDate.ToString()
                })
                .ToList();

            var disclosureFeeExpirationDates = _repository.DisclosureFeeTypes
                .Where(dft => !disclosureFeeTypeIdsInUse.Contains(dft.DisclosureFeeTypeId))
                .Select(disclosureFeeExpirationDates => new SelectListItem
                {
                    Value = disclosureFeeExpirationDates.DisclosureFeeTypeId.ToString(),
                    Text = disclosureFeeExpirationDates.ExpirationDate.ToString()
                })
                .ToList();

            var addressStates = AddressHelper.GetStateItemsWithWisconsinFirst(_repository);
            var countries = AddressHelper.GetCountryItems(_repository);

            return new RequestDetailsViewModel
            {
                Request = request,
                Requester = requester,
                RequestReleaseFormat = requestReleaseFormat,
                Patient = patient,
                PatientRepresentative = patientRepresentative,
                Disclosure = disclosure,
                DocumentRequestedOptions = documentRequestedOptions,
                ItemStatusOptions = itemStatusOptions,
                Requesters = requesters,
                RequestPurposes = requestPurposes,
                RequestPriorities = requestPriorities,
                RequestStatuses = requestStatuses,
                RequestStatusReasons = requestStatusReasons,
                RequesterTypes = requesterTypes,
                RequesterStatuses = requesterStatuses,
                UserTableList = userTableList,
                RequestReleaseFormats = requestReleaseFormats,
                EnteredBy = enteredBy,
                CompletedBy = completedBy,
                RequestBeingEdited = request,
                PatientRepresentativeBeingEdited = patientRepresentative,
                PatientRepresentatives = patientRepresentatives,
                DisclosureBeingEdited = disclosure,
                PaymentTypes = paymentTypes,
                DisclosureFeeAmounts = disclosureFeeAmounts,
                DisclosureFeeDescriptions = disclosureFeeDescriptions,
                DisclosureFeeComments = disclosureFeeComments,
                DisclosureFeeEffectiveDates = disclosureFeeEffectiveDates,
                DisclosureFeeExpirationDates = disclosureFeeExpirationDates,
                Countries = countries,
                AddressStates = addressStates,
                DisclosureFeeDetails = (disclosureFees != null) ? disclosureFees.Select(df => new DisclosureFeeViewModel
                {
                    DisclosureId = df.DisclosureId,
                    DisclosureFeeTypeId = df.DisclosureFeeTypeId, 
                    FeeAmount = df.DisclosureFeeType.FeeAmount,
                    FeeDescription = df.DisclosureFeeType.FeeDescription,
                    Comments = df.DisclosureFeeType.Comments,
                    ItemCount = df.ItemCount,
                    PerItemFee = df.PerItemFee,
                    ItemTotal = df.ItemCount * df.PerItemFee
                }).ToList() : null,
            };
        }

        /// <summary>
        /// Details and Edit Request segment of Disclosure Controller
        /// </summary>
        // Return RequestDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit","DisclosureRequestView")]
        public IActionResult RequestDetails(int? id)
        {
                // Fetch User Session permissions using the helper method
	        var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
	            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

                // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, everything should be visible
            ViewBag.ShowAllRequestDetails = true;
            
            var viewModel = GetRequestDetailsViewModel(id);
            if (viewModel == null)

            {
                return NotFound();
            }
            if (viewModel.Request.CompletedBy != null)
            {
                var completedBy = _repository.UserTables
                    .FirstOrDefault(u => u.UserId == viewModel.Request.CompletedBy);

                viewModel.CompletedBy = completedBy;
            }
            var enteredBy = _repository.UserTables
                .FirstOrDefault(u => u.UserId == viewModel.Request.EnteredBy);

            viewModel.EnteredBy = enteredBy;

            return View(viewModel);
        }

        /// <summary>
        /// Details and Edit Request segment of Disclosure Controller
        /// </summary>
        // Post the Edited Request to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditRequest(RequestDetailsViewModel model, bool showAllRequestDetails)
        {
            // this boolean is passed from the RequestDetails view and is used to determine the return route.  If true, return to RequestDetails.  If false, return to RequestedItemsDetails which uses the RequestDetails as a partial view.
            ViewBag.ShowAllRequestDetails = showAllRequestDetails;
            
            Request editedRequest = _repository.Requests
                .Include(r => r.Requester)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestPriority)
                .Include(r => r.RequestReleaseFormat)
                .Include(r => r.RequestedItems)
                    .ThenInclude(ri => ri.DocumentRequested)
                .Include(r => r.RequestStatus)
                .Include(r => r.RequestStatusReason)
                .Include(r => r.CompletedByNavigation)
                .FirstOrDefault(r => r.RequestId == model.RequestBeingEdited.RequestId);

            editedRequest.RequestDate = model.RequestBeingEdited.RequestDate;
            editedRequest.DateNeeded = model.RequestBeingEdited.DateNeeded;
            editedRequest.EnteredBy = model.RequestBeingEdited.EnteredBy;
            editedRequest.DateCreated = model.RequestBeingEdited.DateCreated;
            editedRequest.DateCompleted = model.RequestBeingEdited.DateCompleted;
            editedRequest.CompletedBy = model.RequestBeingEdited.CompletedBy;
            editedRequest.RequesterId = model.RequestBeingEdited.RequesterId;
            editedRequest.RequestPurposeId = model.RequestBeingEdited.RequestPurposeId;
            editedRequest.RequestPriorityId = model.RequestBeingEdited.RequestPriorityId;
            editedRequest.RequestStatusId = model.RequestBeingEdited.RequestStatusId;
            editedRequest.RequestStatusReasonId = model.RequestBeingEdited.RequestStatusReasonId;
            editedRequest.RequestReleaseFormatId = model.RequestBeingEdited.RequestReleaseFormatId;
            editedRequest.AuthorizationExpirationDate = model.RequestBeingEdited.AuthorizationExpirationDate;
            editedRequest.IsSpecialAuthorizationRequired = model.RequestBeingEdited.IsSpecialAuthorizationRequired;
            editedRequest.IsAttestationRequired = model.RequestBeingEdited.IsAttestationRequired;
            editedRequest.IsTpodisclosure = model.RequestBeingEdited.IsTpodisclosure;
            editedRequest.IsAuthorizationValid = model.RequestBeingEdited.IsAuthorizationValid;
            editedRequest.Comments = model.RequestBeingEdited.Comments;

            _repository.EditRequest(editedRequest);

            if(ViewBag.ShowAllRequestDetails)
            {
                return RedirectToAction("RequestDetails", new { id = model.RequestBeingEdited.RequestId });
            }
            else
            {
                return RedirectToAction("RequestedItemsDetails", new { id = model.RequestBeingEdited.RequestId });
            }
            
        }

        /// <summary>
        /// Details and Edit Request segment of Disclosure Controller
        /// </summary>
        // Return the view RequesterDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestView","RequesterView")]
        public IActionResult RequesterDetails(int? id)
        {
            var viewModel = GetRequestDetailsViewModel(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // Return PatientDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit", "DisclosureRequestView")]
        public IActionResult PatientDetails(int? id)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var viewModel = GetRequestDetailsViewModel(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // Edit Patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditPatient(RequestDetailsViewModel model)
        {
            Request editedRequest = _repository.Requests
                .Include(r => r.Requester)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestPriority)
                .Include(r => r.RequestReleaseFormat)
                .Include(r => r.RequestedItems)
                    .ThenInclude(ri => ri.DocumentRequested)
                .Include(r => r.RequestStatus)
                .Include(r => r.CompletedByNavigation)
                .FirstOrDefault(r => r.RequestId == model.RequestBeingEdited.RequestId);

            editedRequest.PatientFirstName = model.RequestBeingEdited.PatientFirstName;
            editedRequest.PatientLastName = model.RequestBeingEdited.PatientLastName;
            editedRequest.PatientDob = model.RequestBeingEdited.PatientDob;
            editedRequest.PatientPhoneNumber = model.RequestBeingEdited.PatientPhoneNumber;
            editedRequest.PatientEmailAddress = model.RequestBeingEdited.PatientEmailAddress;

            _repository.EditRequest(editedRequest);
            return RedirectToAction("PatientDetails", new { id = model.RequestBeingEdited.RequestId });
        }

        // Return PatientRepresentativeDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestView")]
        public IActionResult PatientRepresentativeDetails(int? id)
        {
                // Fetch User Session permissions using the helper method
	        var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
	            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var viewModel = GetRequestDetailsViewModel(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // Edit Existing Patient Representative
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditPatientRepresentative(RequestDetailsViewModel model)
        {
            // strip the mask from any Patient Representative phone numbers
            model.PatientRepresentative.RepresentativePhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.PatientRepresentative.RepresentativePhoneNumber);

            var editedPatientRepresentative = _repository.PatientRepresentatives
                .AsNoTracking()
                .FirstOrDefault(pr => pr.RepresentativeId == model.PatientRepresentativeBeingEdited.RepresentativeId);

            editedPatientRepresentative.RepresentativeFirstName = model.PatientRepresentativeBeingEdited.RepresentativeFirstName;
            editedPatientRepresentative.RepresentativeLastName = model.PatientRepresentativeBeingEdited.RepresentativeLastName;
            editedPatientRepresentative.RepresentativePhoneNumber = model.PatientRepresentativeBeingEdited.RepresentativePhoneNumber;
            editedPatientRepresentative.RepresentativeEmailAddress = model.PatientRepresentativeBeingEdited.RepresentativeEmailAddress;
            editedPatientRepresentative.Comments = model.PatientRepresentativeBeingEdited.Comments;

            _repository.EditPatientRepresentative(editedPatientRepresentative);
            return RedirectToAction("PatientRepresentativeDetails", new { id = model.RequestBeingEdited.RequestId });
        }

        // Add Patient Representative
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult AddPatientRepresentativeDetails(RequestDetailsViewModel model)
        {
            // strip the mask from any Patient Representative phone numbers
            model.PatientRepresentativeBeingEdited.RepresentativePhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.PatientRepresentativeBeingEdited.RepresentativePhoneNumber);

            var request = _repository.Requests
                    .Include(r => r.PatientRepresentative)
                    .FirstOrDefault(r => r.RequestId == model.RequestBeingEdited.RequestId);

            if (model.RequestBeingEdited.PatientRepresentativeId != null)
            {
                var patientRepresentative = _repository.PatientRepresentatives
                    .FirstOrDefault(pr => pr.RepresentativeId == model.RequestBeingEdited.PatientRepresentativeId);

                request.PatientRepresentativeId = model.RequestBeingEdited.PatientRepresentativeId;
                _repository.EditRequest(request);
                patientRepresentative.Requests.Add(request);
            }
            else
            {
                var patientRepresentative = new PatientRepresentative
                {
                    RepresentativeFirstName = model.PatientRepresentativeBeingEdited.RepresentativeFirstName,
                    RepresentativeLastName = model.PatientRepresentativeBeingEdited.RepresentativeLastName,
                    RepresentativePhoneNumber = model.PatientRepresentativeBeingEdited.RepresentativePhoneNumber,
                    RepresentativeEmailAddress = model.PatientRepresentativeBeingEdited.RepresentativeEmailAddress,
                    Comments = model.PatientRepresentativeBeingEdited.Comments
                };
                patientRepresentative.Requests.Add(request);
                _repository.AddPatientRepresentative(patientRepresentative);

            }
            return RedirectToAction("PatientRepresentativeDetails", new { id = model.RequestBeingEdited.RequestId });
        }

        // Remove Patient Representative
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult RemovePatientRepresentative(int patientRepresentativeId, int requestId)
        {
            var request = _repository.Requests
                .Include(r => r.PatientRepresentative)
                .FirstOrDefault(r => r.RequestId == requestId);

            var representative = _repository.PatientRepresentatives.FirstOrDefault(pr => pr.RepresentativeId == patientRepresentativeId);

            request.PatientRepresentativeId = null;
                // remove from the Request
            _repository.EditRequest(request);

            // attempt to remove from the table.  If it is used in multiple requests, it will not be removed
            try
            {
                _repository.DeletePatientRepresentative(representative);
                TempData["SuccessMessage"] = "The Patient Representative has been removed from this Request and its record deleted.";
            }
            catch (Exception)
            {
                TempData["Error"] = "The Patient Representative has been removed from this Request.  It is used in other Requests and therefore its record was not deleted.";
                return RedirectToAction("PatientRepresentativeDetails", new { id = requestId });
            }

            return RedirectToAction("PatientRepresentativeDetails", new { id = requestId });
        }

        // Return RequestedItemsDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestView")]
        public IActionResult RequestedItemsDetails(int? id)
        {
                // Fetch User Session permissions using the helper method
	        var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
	            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, NOT everything should be visible
            ViewBag.ShowAllRequestDetails = false;

            if (id.HasValue)
            {
                var viewModel = GetRequestDetailsViewModel(id);
                if (viewModel == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }

            return BadRequest("Invalid Request");

        }

        // Add RequestedItem to RequestedItemsDetails Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult RequestedItemsDetails(RequestDetailsViewModel model)
        {
            // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, NOT everything should be visible
            ViewBag.ShowAllRequestDetails = false;
            
            RequestedItem requestedItem = model.RequestedItem;
            requestedItem.RequestId = model.Request.RequestId;
            _repository.AddRequestedItem(requestedItem);

            return RedirectToAction("RequestedItemsDetails", new { id = model.Request.RequestId });
        }

        /// <summary>
        ///     Enable Edit Requested Item - getter
        ///     This is connected to the list of Requested Items, the 'Edit' button for each individual item in the list
        /// </summary>
        /// <param name="requestedItemId"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public IActionResult EnableEditRequestedItem(int requestedItemId, int requestId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, NOT everything should be visible
            ViewBag.ShowAllRequestDetails = false;
            
            var viewModel = GetRequestDetailsViewModel(requestId);

            if (viewModel == null)
            {
                return NotFound("Request details not found.");
            }
            var requestedItem = _repository.RequestedItems
                .Include(ri => ri.DocumentRequested)
                .Include(ri => ri.ItemStatus)
                .FirstOrDefault(ri => ri.RequestedItemId == requestedItemId);

            viewModel.RequestedItemBeingEdited = requestedItem;
            viewModel.RequestedItemBeingEditedId = requestedItemId;

            return View("RequestedItemsDetails", viewModel);
        }

        // Edits RequestedItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditRequestedItem(RequestDetailsViewModel model)
        {
            // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, NOT everything should be visible
            ViewBag.ShowAllRequestDetails = false;
            
            RequestedItem requestedItem = model.RequestedItemBeingEdited;
            requestedItem.RequestedItemId = (int)model.RequestedItemBeingEditedId;
            requestedItem.RequestId = model.Request.RequestId;
            _repository.EditRequestedItem(requestedItem);
            return RedirectToAction("RequestedItemsDetails", new { id = model.Request.RequestId });
        }

        /// <summary>
        ///     Disable Editing of the Requested Item - getter
        ///     This is activated by clicking 'Cancel' when editing a Requested Item already in the list of Requested Items.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public IActionResult DisableEditRequestedItem(int requestId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, NOT everything should be visible
            ViewBag.ShowAllRequestDetails = false;
            
            var viewModel = GetRequestDetailsViewModel(requestId);
            return View("RequestedItemsDetails", viewModel);
        }

        // Delete RequestedItem - setter
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult DeleteRequestedItem(int requestedItemId, int requestId)
        {
            // this boolean is used to control which portion of the view will be rendered as this view is used in both RequestDetails (alone) and as a partial within RequestedItemsDetails.  In this view, NOT everything should be visible
            ViewBag.ShowAllRequestDetails = false;
            
            var requestedItem = _repository.RequestedItems
                .FirstOrDefault(ri => ri.RequestedItemId == requestedItemId);

            if (requestedItem != null)
            {
                _repository.DeleteRequestedItem(requestedItem);
            }

            return RedirectToAction("RequestedItemsDetails", new { id = requestId });
        }

        // Return DisclosureDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestView")]
        public IActionResult DisclosureDetails(int? id)
        {
            // this boolean is used to determine which portion of the UI is rendered
            ViewBag.EditDisclosureFeeDetail = false;

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var viewModel = GetRequestDetailsViewModel(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            if(viewModel.Disclosure != null && viewModel.Disclosure.InvoiceTotal != null)
            {
                var disclosureId = viewModel.Disclosure.DisclosureId;

                decimal subtotal = CalculateSubtotal(disclosureId);
                ViewBag.Subtotal = subtotal != 0 ? subtotal : (decimal?)null;

                decimal calculatedSalesTax = CalculateSalesTax(disclosureId);
                ViewBag.CalculatedSalesTax = calculatedSalesTax != 0 ? calculatedSalesTax : (decimal?)null;

                var disclosurePayments = _repository.DisclosurePayments
                    .Where(df => df.DisclosureId == disclosureId)
                    .ToList();

                decimal paymentsReceived = (disclosurePayments.Count != 0) ? disclosurePayments.Sum(dp => dp.PaymentAmount) : 0.00M;

                decimal invoiceBalance = (decimal)(viewModel.Disclosure.InvoiceTotal - paymentsReceived);
                ViewBag.InvoiceBalance = viewModel.Disclosure.InvoiceTotal == 0 ? "" : invoiceBalance.ToString();
            }
            
            return View(viewModel);
        }

        // Edit Disclosure
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditDisclosure(RequestDetailsViewModel model)
        {
            Disclosure editedDisclosure = _repository.Disclosures
                .FirstOrDefault(d => d.DisclosureId == model.DisclosureBeingEdited.DisclosureId);

            editedDisclosure.DisclosureDate = model.DisclosureBeingEdited.DisclosureDate;
            editedDisclosure.InvoiceDate = model.DisclosureBeingEdited.InvoiceDate;
            editedDisclosure.IsPaymentRequired = model.DisclosureBeingEdited.IsPaymentRequired;
            editedDisclosure.Comments = model.DisclosureBeingEdited.Comments;

            _repository.EditDisclosure(editedDisclosure);
            return RedirectToAction("DisclosureDetails", new { id = model.Request.RequestId });
        }

        // Add Disclosure
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult AddDisclosure(RequestDetailsViewModel model)
        {
            var request = _repository.Requests
                .Include(d => d.Disclosures)
                .FirstOrDefault(r => r.RequestId == model.Request.RequestId);

            var disclosure = new Disclosure
            {
                RequestId = model.Request.RequestId,
                RequesterId = model.Requester.RequesterId,
                DisclosureDate = model.DisclosureBeingEdited.DisclosureDate,
                InvoiceDate = model.DisclosureBeingEdited.InvoiceDate,
                InvoiceNumber = null,
                IsPaymentRequired = model.DisclosureBeingEdited.IsPaymentRequired,
                Comments = model.DisclosureBeingEdited.Comments,
                InvoiceTotal = model.DisclosureBeingEdited.InvoiceTotal
            };

            _repository.AddDisclosure(disclosure);

                // once the disclosure is created and has a DisclosureId, set the InvoiceNumber = DisclosureId and repost
            disclosure.InvoiceNumber = disclosure.DisclosureId;
            _repository.EditDisclosure(disclosure);

            request.Disclosures.Add(disclosure);
            return RedirectToAction("DisclosureDetails", new { id = model.Request.RequestId });
        }

        // Delete Disclosure from the Request
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult DeleteDisclosure(int disclosureId, int requestId)
        {
            var disclosure = _repository.Disclosures
                .FirstOrDefault(d => d.DisclosureId == disclosureId);

            _repository.DeleteDisclosure(disclosure);

            return RedirectToAction("DisclosureDetails", new { id = requestId });
        }

        // Add Disclosure Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult AddDisclosurePayment(RequestDetailsViewModel model)
        {
            DisclosurePayment disclosurePayment = model.DisclosurePayment;
            _repository.AddDisclosurePayment(disclosurePayment);

            var disclosure = _repository.Disclosures
                .FirstOrDefault(d => d.DisclosureId == model.DisclosurePayment.DisclosureId);

            if (disclosure != null)
            {
                disclosure.DisclosurePayments.Add(disclosurePayment);
                _repository.EditDisclosure(disclosure);
            }


            return RedirectToAction("DisclosureDetails", new { id = model.Request.RequestId });
        }

        // Edit Disclosure Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditDisclosurePayment(RequestDetailsViewModel model)
        {
            DisclosurePayment disclosurePayment = model.DisclosurePaymentBeingEdited;
            disclosurePayment.DisclosurePaymentId = (int)model.DisclosurePaymentBeingEditedId;
            _repository.EditDisclosurePayment(disclosurePayment);
            return RedirectToAction("DisclosureDetails", new { id = model.Request.RequestId });
        }

        // Enable Edit Disclosure Payment
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EnableEditDisclosurePayment(int disclosurePaymentId, int disclosureId, int requestId)
        {
            var viewModel = GetRequestDetailsViewModel(requestId);

            if (viewModel == null)
            {
                return NotFound("Request details not found.");
            }

            var disclosurePayment = _repository.DisclosurePayments
                .Include(dp => dp.PaymentType)
                .FirstOrDefault(dp => dp.DisclosurePaymentId == disclosurePaymentId);

            viewModel.DisclosurePaymentBeingEdited = disclosurePayment;
            viewModel.DisclosurePaymentBeingEditedId = disclosurePaymentId;

            return View("DisclosureDetails", viewModel);
        }

        /// <summary>
        ///     Disable Edit Disclosure payment - getter
        ///     routed from the DisclosureDetails.cshtml, in the EditDisclosurePayment area, when the 'Cancel' button is selected
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult DisableEditDisclosurePayment(int requestId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var viewModel = GetRequestDetailsViewModel(requestId);
            return View("DisclosureDetails", viewModel);
        }

        // Delete Disclosure Payment
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult DeleteDisclosurePayment(int disclosurePaymentId, int requestId)
        {
            var disclosurePayment = _repository.DisclosurePayments
                .FirstOrDefault(d => d.DisclosurePaymentId == disclosurePaymentId);

            _repository.DeleteDisclosurePayment(disclosurePayment);
            return RedirectToAction("DisclosureDetails", new { id = requestId });
        }

        /// <summary>
        ///     Disclosure Fees:  Add Disclosure Fee Details - setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult AddDisclosureFeeDetail(RequestDetailsViewModel model)
        {            
            var disclosureFeeType = _repository.DisclosureFeeTypes.FirstOrDefault(dft => dft.DisclosureFeeTypeId == model.DisclosureFeeType.DisclosureFeeTypeId);

            DisclosureFee disclosureFee = new DisclosureFee
            {
                DisclosureId = model.Disclosure.DisclosureId,
                DisclosureFeeTypeId = model.DisclosureFeeType.DisclosureFeeTypeId,
                ItemCount = model.DisclosureFeeDetails[0].ItemCount,
                PerItemFee = disclosureFeeType.FeeAmount
            };
            _repository.AddDisclosureFee(disclosureFee);

            if(disclosureFee != null)
            {
                UpdateInvoiceTotal(model.Disclosure.DisclosureId);
            }

            return RedirectToAction("DisclosureDetails", new { id = model.Request.RequestId });
        }

        /// <summary>
        ///     Disclosures:  Calculate the subtotal of any fees applied
        /// </summary>
        /// <param name="disclosureId"></param>
        /// <returns> subtotal:  summation of the results of (PerItemFee * ItemCount) for all DisclosureFee records per one DisclosureId</returns>
        private decimal CalculateSubtotal(int disclosureId)
        {
            var disclosureFees = _repository.DisclosureFees
                .Where(df => df.DisclosureId == disclosureId)
                .ToList();
            return disclosureFees.Sum(f => f.PerItemFee * f.ItemCount);
        }

        /// <summary>
        ///     Disclosures:  Calculate the sales tax to be applied to the subtotal of DisclosureFees
        ///         Note:  uses a hard-coded 5% for the tax percentage
        /// </summary>
        /// <param name="disclosureId"></param>
        /// <returns> calculatedSalesTax </returns>
        private decimal CalculateSalesTax(int disclosureId)
        {
            decimal salesTaxRate = 0.05M;  // Assuming 5% sales tax
            decimal subtotal = CalculateSubtotal(disclosureId);
            return subtotal * salesTaxRate;
        }

        /// <summary>
        ///     Disclosures:  Update the property Disclosure.InvoiceTotal
        /// </summary>
        /// <param name="disclosureId"></param>
        private void UpdateInvoiceTotal(int disclosureId)
        {
            var disclosureFees = _repository.DisclosureFees
                .Where(df => df.DisclosureId == disclosureId)
                .ToList();

            // Perform calculations for InvoiceTotal
            decimal subtotal = CalculateSubtotal(disclosureId);
            decimal calculatedSalesTax = CalculateSalesTax(disclosureId); 

            var disclosureToUpdate = _repository.Disclosures.FirstOrDefault(d => d.DisclosureId == disclosureId);
            disclosureToUpdate.InvoiceTotal = subtotal + calculatedSalesTax;

            // Update the database with the new InvoiceTotal
            _repository.EditDisclosure(disclosureToUpdate);
        }

        /// <summary>
        ///     Disclosure Fees:  Edit Disclosure Fee Details - Getter
        /// </summary>
        /// <param name="disclosureFeeTypeId"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditDisclosureFeeDetail(int disclosureFeeTypeId, int requestId)
        {
            ViewBag.EditDisclosureFeeDetail = false;

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var viewModel = GetRequestDetailsViewModel(requestId);

            if (viewModel == null)
            {
                return NotFound("Request details not found.");
            }

            if (disclosureFeeTypeId != 0)
            {
                ViewBag.EditDisclosureFeeDetail = true;
                ViewBag.EditDisclosureFeeTypeId = disclosureFeeTypeId;
            }

            return View("DisclosureDetails", viewModel);
        }

        /// <summary>
        ///     Disclosure Fees:  Edit Disclosure Fee Details - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditDisclosureFeeDetail(RequestDetailsViewModel model)
        {
            var feeDetail = model.DisclosureFeeDetails.FirstOrDefault();
            var disclosureFeeTypeId = model.DisclosureFeeDetails.FirstOrDefault()?.DisclosureFeeTypeId;

            var disclosureFeeType = _repository.DisclosureFeeTypes.FirstOrDefault(dft => dft.DisclosureFeeTypeId == disclosureFeeTypeId);

            var existingDisclosureFee = _repository.DisclosureFees
                .Where(df => df.DisclosureId == model.Disclosure.DisclosureId)
                .FirstOrDefault(df => df.DisclosureFeeTypeId == disclosureFeeTypeId);

            if (existingDisclosureFee != null)
            {
                existingDisclosureFee.ItemCount = feeDetail.ItemCount;
                existingDisclosureFee.PerItemFee = disclosureFeeType.FeeAmount;

                _repository.EditDisclosureFee(existingDisclosureFee);

                UpdateInvoiceTotal(model.Disclosure.DisclosureId);
            }

            return RedirectToAction("DisclosureDetails", new { id = model.Request.RequestId });
        }


        /// <summary>
        ///     Disclosure Fees:  Delete a Disclosure Fee
        /// </summary>
        /// <param name="disclosureId"></param>
        /// <param name="disclosureFeeTypeId"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult DeleteDisclosureFeeDetail(int disclosureId, int disclosureFeeTypeId, int requestId)
        {
            var feeToDelete = _repository.DisclosureFees
                .Where(df => df.DisclosureId == disclosureId)
                .FirstOrDefault(df => df.DisclosureFeeTypeId == disclosureFeeTypeId);

            _repository.DeleteDisclosureFee(feeToDelete);

            UpdateInvoiceTotal(disclosureId);
            
            return RedirectToAction("DisclosureDetails", new {  id = requestId });
        }
        
        // Return LegalDetails Page
        [Authorize]
        [PermissionAuthorize("DisclosureRequestView")]
        public IActionResult LegalDetails(int? id)
        {
            var viewModel = GetRequestDetailsViewModel(id);
            if (viewModel == null)
            {
                return NotFound();
            }

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View(viewModel);
        }

        // Edit LegalDetails Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestEdit")]
        public IActionResult EditLegalDetails(RequestDetailsViewModel model)
        {
            Request editedRequest = _repository.Requests
                .FirstOrDefault(r => r.RequestId == model.RequestBeingEdited.RequestId);

            editedRequest.IsSubpoena = model.RequestBeingEdited.IsSubpoena;
            editedRequest.IsCourtOrder = model.RequestBeingEdited.IsCourtOrder;
            editedRequest.CourtOrderIssueDate = model.RequestBeingEdited.CourtOrderIssueDate;
            editedRequest.HearingDate = model.RequestBeingEdited.HearingDate;
            editedRequest.CaseNumber = model.RequestBeingEdited.CaseNumber;
            editedRequest.Plaintiff = model.RequestBeingEdited.Plaintiff;
            editedRequest.Defendant = model.RequestBeingEdited.Defendant;
            editedRequest.IsRequestForPersonalAppearance = model.RequestBeingEdited.IsRequestForPersonalAppearance;
            editedRequest.IsRequestForOriginalRecords = model.RequestBeingEdited.IsRequestForOriginalRecords;
            editedRequest.IsRequestForCertifiedRecords = model.RequestBeingEdited.IsRequestForCertifiedRecords;
            editedRequest.IsRiskManagementAlert = model.RequestBeingEdited.IsRiskManagementAlert;

            _repository.EditRequest(editedRequest);
            return RedirectToAction("LegalDetails", new { id = model.RequestBeingEdited.RequestId });
        }

        // Helper method to handle requested items.
        private void HandleRequestedItems(CreateRequestViewModel model, Request request)
        {
            if (model.SelectedRequestedItemIds != null && model.SelectedRequestedItemIds.Any())
            {
                var newRequestedItems = model.SelectedRequestedItemIds.Select(id => new RequestedItem
                {
                    DocumentRequestedId = id,
                    ItemStatusId = "NS",
                    IsDisclosed = false,
                    Comments = null,
                    RequestId = model.Request.RequestId
                }).ToList();

                foreach(var newRequestedItem in newRequestedItems)
                {
                    _repository.AddRequestedItem(newRequestedItem);
                }
            }
        }
        
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("DisclosureRequestView","PatientView")]
        public IActionResult SearchPatient(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new { success = false, message = "Search term is empty." });
            }

            try
            {
                // Search in the Patients table for existing patients
                var patientsFromPatientsTable = _repository.Patients
                    .Where(p => p.Mrn.Contains(searchTerm) ||
                                p.FirstName.Contains(searchTerm) ||
                                p.LastName.Contains(searchTerm) ||
                                (p.Dob.HasValue && p.Dob.Value.ToString().Contains(searchTerm)))
                    .AsEnumerable()
                    .Select(p => new
                    {
                        mrn = p.Mrn,
                        fullName = $"{p.FirstName} {p.LastName}",
                        dob = p.Dob.HasValue ? p.Dob.Value.ToString("MM/dd/yyyy") : null,
                        firstName = p.FirstName,
                        lastName = p.LastName,
                        phoneNumber = p.PatientContactDetails.OrderByDescending(c => c.LastModified).FirstOrDefault()?.CellPhone,
                        emailAddress = p.PatientContactDetails.OrderByDescending(c => c.LastModified).FirstOrDefault()?.EmailAddress
                    })
                    .ToList();

                // Search in the Requests table for patients added manually
                var patientsFromRequestsTable = _repository.Requests
                    .Where(r => r.PatientMrn == null && // Only patients without MRN
                                (r.PatientFirstName.Contains(searchTerm) ||
                                 r.PatientLastName.Contains(searchTerm) ||
                                 (r.PatientDob.HasValue && r.PatientDob.Value.ToString().Contains(searchTerm))))
                    .Select(r => new
                    {
                        mrn = "", // No MRN for manually added patients
                        fullName = $"{r.PatientFirstName} {r.PatientLastName}",
                        dob = r.PatientDob.HasValue ? r.PatientDob.Value.ToString("MM/dd/yyyy") : null,
                        firstName = r.PatientFirstName,
                        lastName = r.PatientLastName,
                        phoneNumber = r.PatientPhoneNumber,
                        emailAddress = r.PatientEmailAddress
                    })
                    .ToList();

                // Combine both lists
                var combinedPatients = patientsFromPatientsTable.Concat(patientsFromRequestsTable).ToList();

                if (!combinedPatients.Any())
                {
                    return Json(new { success = false, message = "No patients found." });
                }

                // Return the combined patient list
                return Json(new { success = true, patients = combinedPatients });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while searching for patients." });
            }
        }

        /// <summary>
        ///  Delete a Request from the database
        /// </summary>
        /// <param name="model"></param>
        [Authorize]
        [PermissionAuthorize("DisclosureRequestDelete")]
        public IActionResult DeleteRequest(int id)
        {
            Request requestToDelete = _repository.Requests.FirstOrDefault(r => r.RequestId == id);

            try
            {
                _repository.DeleteRequest(requestToDelete);
                
                return RedirectToAction("RequestList");
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "Unable to delete the request";
                return RedirectToAction("RequestList");
            }
        }
        private void PopulateSelectLists(CreateRequestViewModel model)
        {
            model.Requesters = _repository.Requesters
                .Select(r => new SelectListItem
                {
                    Value = r.RequesterId.ToString(),
                    Text = $"{r.FirstName} {r.LastName}"
                })
                .ToList();

            model.RequestPurposeOptions = _repository.RequestPurposes
                .Select(rp => new SelectListItem
                {
                    Value = rp.PurposeId.ToString(),
                    Text = rp.RequestPurpose1
                })
                .ToList();

            model.RequestPriorities = _repository.RequestPriorities
                .Select(rp => new SelectListItem
                {
                    Value = rp.PriorityId.ToString(),
                    Text = rp.RequestPriority1
                })
                .ToList();

            model.RequestReleaseFormats = _repository.RequestReleaseFormats
                .Select(rf => new SelectListItem
                {
                    Value = rf.ReleaseFormatId.ToString(),
                    Text = rf.RequestReleaseFormat1
                })
                .ToList();

            model.RequestStatuses = _repository.RequestStatuses
                .Select(rs => new SelectListItem
                {
                    Value = rs.RequestStatusId.ToString(),
                    Text = rs.RequestStatus1
                })
                .ToList();

            model.RequestStatusReasons = _repository.RequestStatusReasons
                .Select(rr => new SelectListItem
                {
                    Value = rr.RequestStatusReasonId.ToString(),
                    Text = rr.RequestStatusReason1
                })
                .ToList();

            model.CountryOptions = _repository.Countries
                .Select(cc => new SelectListItem
                {
                    Value = cc.CountryId.ToString(),
                    Text = cc.Name
                })
                .ToList();

            model.AddressStateOptions = _repository.AddressStates
                .Select(aso => new SelectListItem
                {
                    Value = aso.StateID.ToString(),
                    Text = aso.StateName
                })
                .ToList();

            model.RequesterTypeOptions = _repository.RequesterTypes
                .Select(rt => new SelectListItem
                {
                    Value = rt.RequesterTypeId.ToString(),
                    Text = rt.RequesterType1
                })
                .ToList();
            model.RequesterStatusOptions = _repository.RequesterStatuses
                .Select(rs => new SelectListItem
                {
                    Value = rs.RequesterStatusId,
                    Text = rs.RequesterStatus1
                })
                .ToList();
            model.RequestPriorityOptions = _repository.RequestPriorities
                .Select(rp => new SelectListItem
                {
                    Value = rp.PriorityId.ToString(),
                    Text = rp.RequestPriority1
                })
                .ToList();
            model.RequestedItemsList = _repository.DocumentRequesteds
                .Select(dr => new SelectListItem
                {
                    Value = dr.DocumentRequestedId.ToString(),
                    Text = dr.DocumentRequested1
                })
                .ToList();
            model.RequestStatusOptions = _repository.RequestStatuses
                .Select(rs => new SelectListItem
                {
                    Value = rs.RequestStatusId.ToString(),
                    Text = rs.RequestStatus1
                })
                .ToList();
            model.StatusReasonOptions = _repository.RequestStatusReasons
                .Select(sr => new SelectListItem
                {
                    Value = sr.RequestStatusReasonId.ToString(),
                    Text = sr.RequestStatusReason1
                })
                .ToList();
            model.UserOptions = _repository.UserTables
                .Where(cc => cc.LastName != "OBSOLETE" && cc.FirstName != "OBSOLETE")
                .OrderBy(cc => cc.LastName)
                .Select(cc => new SelectListItem
                {
                    Value = cc.UserId.ToString(),
                    Text = cc.FirstName + " " + cc.LastName
                })
                .ToList();

        }

        private UserTable GetUserFromAspNetUsersId(string aspNetUsersId)
        {
            return _repository.UserTables.SingleOrDefault(u => u.AspNetUsersId == aspNetUsersId);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
