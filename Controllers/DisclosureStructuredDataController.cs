using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using IS_Proj_HIT.ViewModels.DisclosureStructuredData;
using System;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class DisclosureStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        
        public DisclosureStructuredDataController(IWCTCHealthSystemRepository repo) 
        {
            _repository = repo;
        } 
        
        /// <summary>
        ///     Index page of DisclosureStructuredData - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
        public IActionResult Index() 
        {
            return View();
        }

        #region RequestPurposes
        /// <summary>
        ///     Index page of Purpose of Disclosure (table: RequestPurpose) - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequestPurposes()
        {
            List<RequestPurpose> requestPurposes = _repository.RequestPurposes.ToList();
            return View("RequestPurposes/ViewRequestPurposes", requestPurposes);
        }

        /// <summary>
        ///     Create New RequestPurpose - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestPurpose()
        {
            return View("RequestPurposes/CreateRequestPurpose");
        }

        ///<summary>
        ///     Add new Purpose of Disclosure (RequestPurpose) to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestPurpose(DisclosureStructuredDataViewModel model)      {
            _repository.AddRequestPurpose(model.RequestPurpose);

            return RedirectToAction("ViewRequestPurposes");
        }

        ///<summary>
        ///     Edit fields of an existing RequestPurpose - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestPurpose(byte id) 
        {
            RequestPurpose model = _repository.RequestPurposes.FirstOrDefault(rp => rp.PurposeId == id);
            return View("RequestPurposes/EditRequestPurpose", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing RequestPurpose in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestPurpose(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequestPurpose(model.RequestPurpose);

            return RedirectToAction("ViewRequestPurposes");
        }

        ///<summary>
        ///     Delete an existing RequestPurpose - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestPurpose(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequestPurpose model = _repository.RequestPurposes.FirstOrDefault(rp => rp.PurposeId == id);
            return View("RequestPurposes/DeleteRequestPurpose", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequestPurpose in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestPurpose(DisclosureStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteRequestPurpose(model.RequestPurpose);
                 

                return RedirectToAction("ViewRequestPurposes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Request Purpose. Delete not available.";
                return View("RequestPurposes/DeleteRequestPurpose", model);                
            }

        }
        #endregion  // end of Request Purposes section

        #region RequestPriorities
        /// <summary>
        ///     Index page of list of Priorities (table: RequestPriority) - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequestPriorities()
        {
            List<RequestPriority> requestPriorities = _repository.RequestPriorities.ToList();
            return View("RequestPriorities/ViewRequestPriorities", requestPriorities);
        }
        
        /// <summary>
        ///     Create New RequestPriority - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestPriority(){
            return View("RequestPriorities/CreateRequestPriority");
        }

        ///<summary>
        ///     Add new RequestPriority to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestPriority(DisclosureStructuredDataViewModel model)
        {                       
            // See if any Request Priority exists with this Id
            bool itemInUse = _repository.RequestPriorities.Any(i => i.PriorityId == model.RequestPriority.PriorityId);
            if (itemInUse)
            {
                ViewBag.ErrorMessage = "This Id is already in use.  Please enter a different Id.";
                return View("RequestPriorities/CreateRequestPriority", model);
            }

            _repository.AddRequestPriority(model.RequestPriority);

            return RedirectToAction("ViewRequestPriorities");
        }

        ///<summary>
        ///     Edit fields of an existing RequestPriority - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestPriority(string id) 
        {
            RequestPriority model = _repository.RequestPriorities.FirstOrDefault(p => p.PriorityId == id);
            return View("RequestPriorities/EditRequestPriority", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing RequestPriority in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestPriority(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequestPriority(model.RequestPriority);

            return RedirectToAction("ViewRequestPriorities");
        }

        ///<summary>
        ///     Delete an existing RequestPriority - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestPriority(string id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequestPriority model = _repository.RequestPriorities.FirstOrDefault(p => p.PriorityId == id);
            return View("RequestPriorities/DeleteRequestPriority", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequestPriority in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestPriority(DisclosureStructuredDataViewModel model) 
        {            
            try
            {
                _repository.DeleteRequestPriority(model.RequestPriority);

                return RedirectToAction("ViewRequestPriorities"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Request Priority. Delete not available.";
                return View("RequestPriorities/DeleteRequestPriority", model);                
            }
        }
        #endregion  // end of Request Priority section

        #region Document Requesteds
        /// <summary>
        ///     Index page of list of Documents Requested - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDocumentRequesteds()
        {
            List<DocumentRequested> documentRequested = _repository.DocumentRequesteds.ToList();
            return View("DocumentRequesteds/ViewDocumentRequesteds", documentRequested);
        }

        /// <summary>
        ///     Create New Document Requested - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDocumentRequested()
        {
            return View("DocumentRequesteds/CreateDocumentRequested");
        }

        ///<summary>
        ///     Add new Document Requested item to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDocumentRequested(DisclosureStructuredDataViewModel model)      
        {
            _repository.AddDocumentRequested(model.DocumentRequested);

            return RedirectToAction("ViewDocumentRequesteds");
        }

        ///<summary>
        ///     Edit fields of an existing DocumentRequested - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDocumentRequested(int id) 
        {
            DocumentRequested model = _repository.DocumentRequesteds.FirstOrDefault(p => p.DocumentRequestedId == id);
            return View("DocumentRequesteds/EditDocumentRequested", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing DocumentRequested in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDocumentRequested(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditDocumentRequested(model.DocumentRequested);

            return RedirectToAction("ViewDocumentRequesteds");
        }

        ///<summary>
        ///     Delete an existing DocumentRequested - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDocumentRequested(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            DocumentRequested model = _repository.DocumentRequesteds.FirstOrDefault(p => p.DocumentRequestedId == id);
            return View("DocumentRequesteds/DeleteDocumentRequested", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing DocumentRequested in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDocumentRequested(DisclosureStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteDocumentRequested(model.DocumentRequested); 

                return RedirectToAction("ViewDocumentRequesteds"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Requested Document. Delete not available.";
                return View("DocumentRequesteds/DeleteDocumentRequested", model);                
            }
        }
        #endregion  // end of Document Requesteds section

        #region Request Release Formats
        /// <summary>
        ///     Index page of list of Request Release Formats - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequestReleaseFormats()
        {
            List<RequestReleaseFormat> requestReleaseFormat = _repository.RequestReleaseFormats.ToList();
            return View("RequestReleaseFormats/ViewRequestReleaseFormats", requestReleaseFormat);
        }

        /// <summary>
        ///     Create New Request Release Format - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestReleaseFormat()
        {
            return View("RequestReleaseFormats/CreateRequestReleaseFormat");
        }

        ///<summary>
        ///     Add new Request Release Format to the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestReleaseFormat(DisclosureStructuredDataViewModel model)      
        {
            _repository.AddRequestReleaseFormat(model.RequestReleaseFormat);

            return RedirectToAction("ViewRequestReleaseFormats");
        }

        ///<summary>
        ///     Edit fields of an existing Request Release Format - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestReleaseFormat(byte id) 
        {
            RequestReleaseFormat model = _repository.RequestReleaseFormats.FirstOrDefault(p => p.ReleaseFormatId == id);
            return View("RequestReleaseFormats/EditRequestReleaseFormat", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Request Release Format in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestReleaseFormat(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequestReleaseFormat(model.RequestReleaseFormat);

            return RedirectToAction("ViewRequestReleaseFormats");
        }

        ///<summary>
        ///     Delete an existing RequestReleaseFormat - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestReleaseFormat(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequestReleaseFormat model = _repository.RequestReleaseFormats.FirstOrDefault(p => p.ReleaseFormatId == id);
            return View("RequestReleaseFormats/DeleteRequestReleaseFormat", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequestReleaseFormat in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestReleaseFormat(DisclosureStructuredDataViewModel model) 
        {                        
            try
            {
                _repository.DeleteRequestReleaseFormat(model.RequestReleaseFormat); 

                return RedirectToAction("ViewRequestReleaseFormats"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Request Release Format. Delete not available.";
                return View("RequestReleaseFormats/DeleteRequestReleaseFormat", model);                
            }
        }
        #endregion  // end of Request Release Formats

        #region Request Status
        /// <summary>
        ///     Index page of list of Request Statuses - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequestStatuses()
        {
            List<RequestStatus> requestStatuses = _repository.RequestStatuses.ToList();
            return View("RequestStatus/ViewRequestStatuses", requestStatuses);
        }

        /// <summary>
        ///     Create New Request Status - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestStatus()
        {
            return View("RequestStatus/CreateRequestStatus");
        }

        ///<summary>
        ///     Add new Request Status to the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestStatus(DisclosureStructuredDataViewModel model)
        {
            // See if any Request Status exists with this Id
            bool itemInUse = _repository.RequestStatuses.Any(i => i.RequestStatusId == model.RequestStatus.RequestStatusId);
            if (itemInUse)
            {
                ViewBag.ErrorMessage = "This Id is already in use.  Please enter a different Id.";
                return View("RequestStatus/CreateRequestStatus", model);
            }

            _repository.AddRequestStatus(model.RequestStatus);

            return RedirectToAction("ViewRequestStatuses");
        }

        ///<summary>
        ///     Edit fields of an existing RequestStatus - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestStatus(string id) 
        {
            RequestStatus model = _repository.RequestStatuses.FirstOrDefault(p => p.RequestStatusId == id);
            return View("RequestStatus/EditRequestStatus", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing RequestStatus in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestStatus(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequestStatus(model.RequestStatus);

            return RedirectToAction("ViewRequestStatuses");
        }

        ///<summary>
        ///     Delete an existing RequestStatus - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestStatus(string id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequestStatus model = _repository.RequestStatuses.FirstOrDefault(p => p.RequestStatusId == id);
            return View("RequestStatus/DeleteRequestStatus", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequestStatus in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestStatus(DisclosureStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteRequestStatus(model.RequestStatus); 

                return RedirectToAction("ViewRequestStatuses"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Request Status. Delete not available.";
                return View("RequestStatus/DeleteRequestStatus", model);                
            }
        }
        #endregion  // end of Request Status section

        #region Request Status Reasons
        /// <summary>
        ///     Index page of list of Request Status Reasons - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequestStatusReasons()
        {
            List<RequestStatusReason> requestStatusReasons = _repository.RequestStatusReasons.ToList();
            return View("RequestStatusReasons/ViewRequestStatusReasons", requestStatusReasons);
        }

        /// <summary>
        ///     Create New Request Status Reason - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestStatusReason()
        {
            return View("RequestStatusReasons/CreateRequestStatusReason");
        }

        ///<summary>
        ///     Add new Request Status Reason to the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequestStatusReason(DisclosureStructuredDataViewModel model)      
        {
            _repository.AddRequestStatusReason(model.RequestStatusReason);

            return RedirectToAction("ViewRequestStatusReasons");
        }

        ///<summary>
        ///     Edit fields of an existing RequestStatusReason - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestStatusReason(byte id) 
        {
            RequestStatusReason model = _repository.RequestStatusReasons.FirstOrDefault(p => p.RequestStatusReasonId == id);
            return View("RequestStatusReasons/EditRequestStatusReason", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing RequestStatusReason in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequestStatusReason(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequestStatusReason(model.RequestStatusReason);

            return RedirectToAction("ViewRequestStatusReasons");
        }

        ///<summary>
        ///     Delete an existing RequestStatusReason - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestStatusReason(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequestStatusReason model = _repository.RequestStatusReasons.FirstOrDefault(p => p.RequestStatusReasonId == id);
            return View("RequestStatusReasons/DeleteRequestStatusReason", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequestStatusReason in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequestStatusReason(DisclosureStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteRequestStatusReason(model.RequestStatusReason); 

                return RedirectToAction("ViewRequestStatusReasons"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Request Status Reason. Delete not available.";
                return View("RequestStatusReasons/DeleteRequestStatusReason", model);                
            }
        }
        #endregion  // end of Request Status Reasons section

        #region Item Status
        /// <summary>
        ///     Index page of list of Item Statuses - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewItemStatuses()
        {
            List<ItemStatus> itemStatuses = _repository.ItemStatuses.ToList();
            return View("ItemStatus/ViewItemStatuses", itemStatuses);
        }

        /// <summary>
        ///     Create New Item Status - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateItemStatus()
        {
            return View("ItemStatus/CreateItemStatus");
        }

        ///<summary>
        ///     Add new Item Status to the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateItemStatus(DisclosureStructuredDataViewModel model)      
        {
            // See if any Request Priority exists with this Id
            bool itemInUse = _repository.ItemStatuses.Any(i => i.ItemStatusId == model.ItemStatus.ItemStatusId);
            if (itemInUse)
            {
                ViewBag.ErrorMessage = "This Id is already in use.  Please enter a different Id.";
                return View("ItemStatus/CreateItemStatus", model);
            }

            _repository.AddItemStatus(model.ItemStatus);

            return RedirectToAction("ViewItemStatuses");
        }

        ///<summary>
        ///     Edit fields of an existing Item Status - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditItemStatus(string id) 
        {
            ItemStatus model = _repository.ItemStatuses.FirstOrDefault(p => p.ItemStatusId == id);
            return View("ItemStatus/EditItemStatus", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Item Status in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditItemStatus(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditItemStatus(model.ItemStatus);

            return RedirectToAction("ViewItemStatuses");
        }

        ///<summary>
        ///     Delete an existing Item Status - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteItemStatus(string id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            ItemStatus model = _repository.ItemStatuses.FirstOrDefault(p => p.ItemStatusId == id);
            return View("ItemStatus/DeleteItemStatus", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing Item Status in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteItemStatus(DisclosureStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteItemStatus(model.ItemStatus); 

                return RedirectToAction("ViewItemStatuses"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Item Status. Delete not available.";
                return View("ItemStatus/DeleteItemStatus", model);                
            }
        }
        #endregion  // end of Item Status section

        #region Requester Types
        /// <summary>
        ///     Index page of Requester Types - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequesterTypes()
        {
            List<RequesterType> requesterType = _repository.RequesterTypes.ToList();
            return View("RequesterType/ViewRequesterTypes", requesterType);
        }

        /// <summary>
        ///     Create New Requester Type - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequesterType()
        {
            return View("RequesterType/CreateRequesterType");
        }

        ///<summary>
        ///     Add new RequesterType to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequesterType(DisclosureStructuredDataViewModel model)      {
            _repository.AddRequesterType(model.RequesterType);

            return RedirectToAction("ViewRequesterTypes");
        }

        ///<summary>
        ///     Edit fields of an existing RequesterType - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequesterType(byte id) 
        {
            RequesterType model = _repository.RequesterTypes.FirstOrDefault(r => r.RequesterTypeId == id);
            return View("RequesterType/EditRequesterType", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing RequesterType in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequesterType(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequesterType(model.RequesterType);

            return RedirectToAction("ViewRequesterTypes");
        }

        ///<summary>
        ///     Delete an existing RequesterType - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequesterType(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequesterType model = _repository.RequesterTypes.FirstOrDefault(r => r.RequesterTypeId == id);
            return View("RequesterType/DeleteRequesterType", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequesterType in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequesterType(DisclosureStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteRequesterType(model.RequesterType);
                 

                return RedirectToAction("ViewRequesterTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Requester Type. Delete not available.";
                return View("RequesterType/DeleteRequesterType", model);                
            }

        }
        #endregion  // end of Requester Type section

        #region Requester Status
        /// <summary>
        ///     Index page of list of Requester Status - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRequesterStatuses()
        {
            List<RequesterStatus> requesterStatuses = _repository.RequesterStatuses.ToList();
            return View("RequesterStatus/ViewRequesterStatuses", requesterStatuses);
        }
        
        /// <summary>
        ///     Create New RequesterStatus - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequesterStatus(){
            return View("RequesterStatus/CreateRequesterStatus");
        }

        ///<summary>
        ///     Add new RequesterStatus to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRequesterStatus(DisclosureStructuredDataViewModel model)
        {                       
            // See if any Requester Status exists with this Id
            bool itemInUse = _repository.RequesterStatuses.Any(i => i.RequesterStatusId == model.RequesterStatus.RequesterStatusId);
            if (itemInUse)
            {
                ViewBag.ErrorMessage = "This Id is already in use.  Please enter a different Id.";
                return View("RequesterStatus/CreateRequesterStatus", model);
            }

            _repository.AddRequesterStatus(model.RequesterStatus);

            return RedirectToAction("ViewRequesterStatuses");
        }

        ///<summary>
        ///     Edit fields of an existing RequesterStatus - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequesterStatus(string id) 
        {
            RequesterStatus model = _repository.RequesterStatuses.FirstOrDefault(p => p.RequesterStatusId == id);
            return View("RequesterStatus/EditRequesterStatus", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing RequesterStatus in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRequesterStatus(DisclosureStructuredDataViewModel model) 
        {
            _repository.EditRequesterStatus(model.RequesterStatus);

            return RedirectToAction("ViewRequesterStatuses");
        }

        ///<summary>
        ///     Delete an existing RequesterStatus - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequesterStatus(string id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RequesterStatus model = _repository.RequesterStatuses.FirstOrDefault(p => p.RequesterStatusId == id);
            return View("RequesterStatus/DeleteRequesterStatus", new DisclosureStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Delete an existing RequesterStatus in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRequesterStatus(DisclosureStructuredDataViewModel model) 
        {            
            try
            {
                _repository.DeleteRequesterStatus(model.RequesterStatus);

                return RedirectToAction("ViewRequesterStatuses"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Requester Status. Delete not available.";
                return View("RequesterStatus/DeleteRequesterStatus", model);                
            }
        }
        #endregion  // end of Requester Status section
    }
}
