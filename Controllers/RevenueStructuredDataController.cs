using System;
using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class RevenueStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public RevenueStructuredDataController(IWCTCHealthSystemRepository repository)
        {
            _repository = repository;
        }

        ///<summary>
        /// Index page of RevenueStructuredData
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult Index()
        {
            return View();
        }

        #region Disclosure Fee Type
        ///<summary>
        /// List View of all Disclosure Fee Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDisclosureFeeTypes()
        {
            List<DisclosureFeeType> disclosureFeeType = _repository.DisclosureFeeTypes.ToList();
            return View("DisclosureFeeType/ViewDisclosureFeeTypes",disclosureFeeType);
        }

        /// <summary>
        /// Create New Disclosure Fee Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDisclosureFeeType(){
            return View("DisclosureFeeType/CreateDisclosureFeeType");
        }

        // Note:  the CreateDisclosureFeeType setter functionality is included in the AdjustSortOrders method (below)

        ///<summary>
        /// Edit fields of an existing Disclosure Fee Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDisclosureFeeType(int id) 
        {
            DisclosureFeeType model = _repository.DisclosureFeeTypes.FirstOrDefault(p => p.DisclosureFeeTypeId == id);
            return View("DisclosureFeeType/EditDisclosureFeeType",new RevenueStructuredDataViewModel (model));
        }

        // Note:  the EditDisclosureFeeType setter functionality is included in the AdjustSortOrders method (below)

        ///<summary>
        /// Delete an existing Disclosure Fee Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDisclosureFeeType(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            DisclosureFeeType model = _repository.DisclosureFeeTypes.FirstOrDefault(p => p.DisclosureFeeTypeId == id);
            return View("DisclosureFeeType/DeleteDisclosureFeeType",new RevenueStructuredDataViewModel (model));
        }

        ///<summary>
        /// Delete an existing Disclosure Fee Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDisclosureFeeType(RevenueStructuredDataViewModel model) 
        {
            var record = _repository.DisclosureFeeTypes.FirstOrDefault(x => x.DisclosureFeeTypeId == model.DisclosureFeeType.DisclosureFeeTypeId);

            if (record == null)
            {
                ViewData["ErrorMessage"] = "Record not found";
                return View("DisclosureFeeType/DeleteDisclosureFeeType", model);
            };

            try
            {
                _repository.DeleteDisclosureFeeType(record);

                return RedirectToAction("ViewDisclosureFeeTypes");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Disclosure Fee Type. Delete not available.";
                return View("DisclosureFeeType/DeleteDisclosureFeeType", model);
            }
        }

        /// <summary>
        ///     Manage the SortOrder property for DisclosureFeeType - Setter
        ///     It is an AJAX call within ViewDisclosureFeeTypes.cshtml in response to a drag and drop by the User
        /// </summary>
        /// <param name="sortedOrders"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataEdit")]
        public IActionResult UpdateOrder([FromBody] Dictionary<int, int> sortedOrders)
        {
            foreach (var kv in sortedOrders)
            {
                int disclosureFeeTypeId = kv.Key;
                int newSortOrder = kv.Value;

                var item = _repository.DisclosureFeeTypes.FirstOrDefault(x => x.DisclosureFeeTypeId == disclosureFeeTypeId);
                if (item != null)
                {
                    // Update only the SortOrder property.
                    item.SortOrder = newSortOrder;
                    _repository.EditDisclosureFeeType(item);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"Updated: {item.DisclosureFeeTypeId} -> New SortOrder: {item.SortOrder}");
                            Console.ResetColor();
                }
                else
                {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"Warning: ID {disclosureFeeTypeId} not found in database");
                        Console.ResetColor();
                }
            }
            return Ok();
        }

        /// <summary>
        ///     DisclosureFeeType Setter - Adjusts the SortOrder of the dataset when called by either CreateDisclosureFeeType.cshtml or EditDisclosureFeeType.cshtml
        ///     Because it handles record Creation and Updating, the AJAX must send the entire form.
        ///         This method replaces the standard setters for CreateDisclosureFeeType.cshtml and EditDisclosureFeeType.cshtml
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataEdit")]
        public IActionResult AdjustSortOrders([FromBody] RevenueStructuredDataViewModel model)
        {
            if (model == null || model.DisclosureFeeType == null)
                return BadRequest();

            // Use the posted sort order and id from the view model
            int newSortOrder = model.DisclosureFeeType.SortOrder;
            int recordId = model.DisclosureFeeType.DisclosureFeeTypeId; // should be 0 on create

            // Get all existing records ordered by SortOrder.
            var allItems = _repository.DisclosureFeeTypes
                            .OrderBy(x => x.SortOrder)
                            .ToList();

            // If this is an edit (recordId > 0), remove the record being updated from the list.
            // If it's create, that record won't be in the list, so we'll add it.
            DisclosureFeeType editingRecord = null;
            if (recordId > 0)
            {
                // record was modified, find it in the list of allItems
                editingRecord = allItems.FirstOrDefault(x => x.DisclosureFeeTypeId == recordId);

                // map the fields, then remove it from the list of allItems (re-added below)
                if (editingRecord != null)
                    editingRecord.SortOrder = model.DisclosureFeeType.SortOrder;
                    editingRecord.FeeAmount = model.DisclosureFeeType.FeeAmount;
                    editingRecord.FeeDescription = model.DisclosureFeeType.FeeDescription;
                    editingRecord.Comments = model.DisclosureFeeType.Comments;
                    editingRecord.EffectiveDate = model.DisclosureFeeType.EffectiveDate;
                    editingRecord.ExpirationDate = model.DisclosureFeeType.ExpirationDate;
                    allItems.Remove(editingRecord);
            }
            else
            {
                // For create, use the posted model as the new record.
                editingRecord = model.DisclosureFeeType;
            }

            // Determine the insert index.
            // newSortOrder is expected to be 1-based; adjust to 0-based.
            int insertIndex = Math.Min(newSortOrder - 1, allItems.Count);
            allItems.Insert(insertIndex, editingRecord);

            // Now renumber the entire list so that sort orders are contiguous starting at 1.
            for (int i = 0; i < allItems.Count; i++)
            {
                allItems[i].SortOrder = i + 1;

                // if the record is new, add it
                if (allItems[i].DisclosureFeeTypeId == 0)
                {
                    _repository.AddDisclosureFeeType(allItems[i]);
                }
                else
                {
                    // Update each record.
                    _repository.EditDisclosureFeeType(allItems[i]);
                }
            }

            return Ok();
        }

        /// <summary>
        ///     DisclosureFeeType - Setter - reorders the dataset SortOrder after deleting a record
        ///     called via AJAX from within DeleteDisclosureFeeType.cshtml
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult RenumberSortOrders([FromBody] RevenueStructuredDataViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            // Use the DeletedSortOrder from your view model
            int deletedSortOrder = model.DeletedSortOrder;

            // Retrieve all records with a SortOrder greater than the deleted record's sort order.
            var itemsToUpdate = _repository.DisclosureFeeTypes
                                .Where(x => x.SortOrder >= deletedSortOrder)
                                .OrderBy(x => x.SortOrder)
                                .ToList();

            foreach (var item in itemsToUpdate)
            {
                // Decrement the sort order for each subsequent record.
                item.SortOrder = item.SortOrder - 1;
                _repository.EditDisclosureFeeType(item);
            }

            return Ok();
        }

        #endregion  // end of Disclosure Fee Type section

        #region Payment Type
        ///<summary>
        /// List View of all Payment Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPaymentTypes()
        {
            List<PaymentType> paymentType = _repository.PaymentTypes.ToList();
            return View("PaymentType/ViewPaymentTypes",paymentType);
        }

        /// <summary>
        /// Create New Payment Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePaymentType(){
            return View("PaymentType/CreatePaymentType");
        }

        ///<summary>
        /// Add new Payment Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreatePaymentType(RevenueStructuredDataViewModel model)
        {                       
            _repository.AddPaymentType(model.PaymentType);

            return RedirectToAction("ViewPaymentTypes");
        }

        ///<summary>
        /// Edit fields of an existing Payment Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPaymentType(int id) 
        {
            PaymentType model = _repository.PaymentTypes.FirstOrDefault(p => p.PaymentTypeId == id);
            return View("PaymentType/EditPaymentType",new RevenueStructuredDataViewModel (model));
        }

        ///<summary>
        /// Edit fields of an existing Payment Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPaymentType(RevenueStructuredDataViewModel model) 
        {
            _repository.EditPaymentType(model.PaymentType);

            return RedirectToAction("ViewPaymentTypes");
        }

        ///<summary>
        /// Delete an existing Payment Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePaymentType(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            PaymentType model = _repository.PaymentTypes.FirstOrDefault(p => p.PaymentTypeId == id);
            return View("PaymentType/DeletePaymentType",new RevenueStructuredDataViewModel (model));
        }

        ///<summary>
        /// Delete an existing Payment Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePaymentType(RevenueStructuredDataViewModel model) 
        {            
            try
            {
                _repository.DeletePaymentType(model.PaymentType);

                return RedirectToAction("ViewPaymentTypes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Payment Type. Delete not available.";
                return View("PaymentType/DeletePaymentType",model);                
            }
        }
        #endregion  // end of Payment Type section

        #region Revenue Codes
        ///<summary>
        /// List View of all Revenue Codes
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRevenueCodes()
        {
            List<RevenueCode> revenueCodes = _repository.RevenueCodes.ToList();
            return View("RevenueCodes/ViewRevenueCodes", revenueCodes);
        }

        /// <summary>
        /// Create New Revenue Code
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRevenueCode(){
            return View("RevenueCodes/CreateRevenueCode");
        }

        ///<summary>
        /// Add new Revenue Code to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateRevenueCode(RevenueStructuredDataViewModel model)
        {                       
            // See if any RevenueCode exists with this Id
            bool itemInUse = _repository.RevenueCodes.Any(i => i.RevenueCodeID == model.RevenueCode.RevenueCodeID);
            if (itemInUse)
            {
                ViewBag.ErrorMessage = "This Id is already in use.  Please enter a different Id.";
                return View("RevenueCodes/CreateRevenueCode", model);
            }

            _repository.AddRevenueCode(model.RevenueCode);

            return RedirectToAction("ViewRevenueCodes");
        }

        ///<summary>
        /// Edit fields of an existing Revenue Code
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRevenueCode(string id) 
        {
            RevenueCode model = _repository.RevenueCodes.FirstOrDefault(p => p.RevenueCodeID == id);
            return View("RevenueCodes/EditRevenueCode", new RevenueStructuredDataViewModel (model));
        }

        ///<summary>
        /// Edit fields of an existing Revenue Code in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRevenueCode(RevenueStructuredDataViewModel model) 
        {
            _repository.EditRevenueCode(model.RevenueCode);

            return RedirectToAction("ViewRevenueCodes");
        }


        ///<summary>
        /// Delete an existing Revenue Code
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRevenueCode(string id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            RevenueCode model = _repository.RevenueCodes.FirstOrDefault(p => p.RevenueCodeID == id);
            return View("RevenueCodes/DeleteRevenueCode", new RevenueStructuredDataViewModel (model));
        }

        ///<summary>
        /// Delete an existing Revenue Code in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRevenueCode(RevenueStructuredDataViewModel model) 
        {            
            try
            {
                _repository.DeleteRevenueCode(model.RevenueCode);

                return RedirectToAction("ViewRevenueCodes"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Revenue Code. Delete not available.";
                return View("RevenueCodes/DeleteRevenueCode", model);                
            }
        }
        #endregion  // end of Revenue Code section

        #region Charge Definitions
        ///<summary>
        /// List View of all Charge Definitions
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewChargeDefinitions()
        {
            List<ChargeDefinition> chargeDefinitions = _repository.ChargeDefinitions.ToList();
            return View("ChargeDefinitions/ViewChargeDefinitions", chargeDefinitions);
        }

        /// <summary>
        /// Create New Charge Definition
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateChargeDefinition()
        {
            var departments = new SelectList(_repository.Departments, "DepartmentId", "Name");
                var departmentList = departments.ToList();
                departmentList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["DepartmentID"] = departmentList;

            var revenueCodes = new SelectList(_repository.RevenueCodes, "RevenueCodeID", "ShortDescription");
                var revenueCodesList = revenueCodes.ToList();
                revenueCodesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["RevenueCodeID"] = revenueCodesList;

            return View("ChargeDefinitions/CreateChargeDefinition");
        }

        /// <summary>
        ///  Post new Charge Definition to the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateChargeDefinition(RevenueStructuredDataViewModel model)
        {
            _repository.AddChargeDefinition(model.ChargeDefinition);
            
            return RedirectToAction("ViewChargeDefinitions");
        }

        ///<summary>
        /// View Details of a Charge Definition
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult DetailsChargeDefinition(int id)
        {
            ChargeDefinition chargeDefinition = _repository.ChargeDefinitions.FirstOrDefault(cd => cd.ChargeID == id);
            
            Department department = _repository.Departments.FirstOrDefault(d => d.DepartmentId == chargeDefinition.DepartmentID);

            RevenueCode revenueCode = _repository.RevenueCodes.FirstOrDefault(rc => rc.RevenueCodeID == chargeDefinition.RevenueCodeID);
            
            return View("ChargeDefinitions/DetailsChargeDefinition", new RevenueStructuredDataViewModel (chargeDefinition, department, revenueCode));
        }

        ///<summary>
        /// Edit fields of an Charge Definition
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditChargeDefinition(int id) 
        {
            ViewData["DepartmentID"] = new SelectList(_repository.Departments, "DepartmentId", "Name");
            ViewData["RevenueCodeID"] = new SelectList(_repository.RevenueCodes, "RevenueCodeID", "ShortDescription");

            ChargeDefinition chargeDefinition =_repository.ChargeDefinitions.FirstOrDefault(cd => cd.ChargeID == id);

            Department department = _repository.Departments.FirstOrDefault(d => d.DepartmentId == chargeDefinition.DepartmentID);

            RevenueCode revenueCode = _repository.RevenueCodes.FirstOrDefault(rc => rc.RevenueCodeID == chargeDefinition.RevenueCodeID);

            return View("ChargeDefinitions/EditChargeDefinition", new RevenueStructuredDataViewModel (chargeDefinition, department, revenueCode));
        }

        ///<summary>
        /// Edit fields of an Charge Definition in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditChargeDefinition(RevenueStructuredDataViewModel model) 
        {
            if(model.ChargeDefinition.DateDeactivated != null)
            {
                model.ChargeDefinition.IsActive = false;
            }

            _repository.EditChargeDefinition(model.ChargeDefinition);

            return RedirectToAction("ViewChargeDefinitions");
        }

        ///<summary>
        /// Delete an existing Charge Definition
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteChargeDefinition(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";
            
            ChargeDefinition chargeDefinition = _repository.ChargeDefinitions.FirstOrDefault(p => p.ChargeID == id);
            

            if(chargeDefinition == null)
                { 
                    return NotFound();
                }

            return View("ChargeDefinitions/DeleteChargeDefinition", new RevenueStructuredDataViewModel (chargeDefinition));
        }

        ///<summary>
        /// Delete an existing Charge Definition in the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteChargeDefinition(RevenueStructuredDataViewModel model) 
        {
            try
            {
                _repository.DeleteChargeDefinition(model.ChargeDefinition); 

                return RedirectToAction("ViewChargeDefinitions"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Charge Definition. Delete not available.";
                return View("ChargeDefinitions/DeleteChargeDefinition", model);                
            }
        }
        #endregion  // end Charge Definition section
    }

}