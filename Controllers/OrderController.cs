using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("OrdersAdd","OrdersView","OrdersEdit","OrdersDelete")]
    public class OrderController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
    
        public int PageSize = 8;
        public OrderController(IWCTCHealthSystemRepository repo) 
        {
            _repository = repo;
        } 
        
        /// <summary>
        ///     View page of a specific order
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        /// <param name="orderId">Id of unique order</param>
        /// 
        // Used in: ViewOrder 
        [Authorize]
        [PermissionAuthorize("OrdersView")]
        public IActionResult ViewOrder(long encounterId, long orderId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var id = User.Identity.Name;

            var encounter = _repository.Encounters
                .Include(p =>p.EncounterPhysicians)
                    .ThenInclude(ph => ph.Physician)
                .Include(d => d.Department)
                .Include(e => e.EncounterType)
                .FirstOrDefault(b => b.EncounterId == encounterId);

            var order = _repository.OrderInfos
                .Include(o => o.OrderingProvider)
                    .ThenInclude(op => op.ProviderStatus)
                .Include(o => o.AuthenticatingProvider)
                .Include(o => o.Author)
                .Include(o => o.OrderType)
                .Include(o => o.Priority)
                .Include(o => o.CompletedByProvider)
                .Include(o => o.ChargeDefinition)
                .FirstOrDefault(b => b.OrderInfoId == orderId);

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);

            return View(new OrdersViewModel
            {
                OrderInfo     = order,
                Patient   = patient,
                Encounter = encounter
            });
        }

        /// <param name="id">Id of unique encounter</param>
        /// <param name="sortOrder">Order to sort orders</param>
        // Used in: ListOrders
        [Authorize]
        [PermissionAuthorize("OrdersView")]
        public IActionResult ListOrders(long id)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            ViewBag.EncounterId = id;
            ViewBag.Encounter = _repository.Encounters
                                    .FirstOrDefault(e => e.EncounterId == id);
            string Mrn = _repository.Encounters
                                .FirstOrDefault(b => b.EncounterId == id)
                                .Mrn;

            ViewBag.Patient = _repository.Patients
                                .Include(p => p.PatientAlerts)
                                .FirstOrDefault(b => b.Mrn == Mrn);

            ViewBag.Mrn = Mrn;

            var orders = _repository.OrderInfos
                            .Where(o => o.EncounterId == id)
                            .Include(o=>o.OrderType)
                            .Include(o => o.OrderingProvider)
                            .ToList();

            List<OrderInfo> orderInfos = _repository.OrderInfos
                    .Where(o => o.EncounterId == id)
                    .ToList();

            return View(orderInfos);
        }

        ///<summary>
        /// Getter for CreateOrder
        ///</summary>
        /// <param name="encounterId"></param name>
        [Authorize]
        [PermissionAuthorize("OrdersAdd")]
        public IActionResult CreateOrder(long encounterId)
        {
            var encounter = _repository.Encounters
                                .Include(p =>p.EncounterPhysicians)
                                .ThenInclude(ph => ph.Physician)
                                .Include(et => et.EncounterType)
                                .Include(d => d.Department)
                                .FirstOrDefault(b => b.EncounterId == encounterId);

            var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == encounter.Mrn);

            var radiologyCdm = _repository.ChargeDefinitions
                .Where(cd => cd.DepartmentID == 8)
                .ToList();

            var labCdm = _repository.ChargeDefinitions
                .Where(cd => cd.DepartmentID == 7)
                .ToList();

            var queryPhysician = _repository.Physicians
                .Include(p => p.Specialty)
                .OrderBy(p => p.LastName)
                .Select(p => new {p.PhysicianId, FullName = p.FirstName +" "+ p.LastName+",  "+ p.Specialty.Name})
                .ToList();

            var queryCosigner = _repository.Physicians
                .Include(p => p.Specialty)
                .Where(p => p.ProviderStatusId == 1)
                .OrderBy(p => p.LastName)
                .Select(p => new {p.PhysicianId, FullName = p.FirstName +" "+ p.LastName+",  "+ p.Specialty.Name})
                .ToList();

            ViewData["RadiologyCDM"] = new SelectList(radiologyCdm, "ChargeID", "LongDescription");
            ViewData["LaboratoryCDM"] = new SelectList(labCdm, "ChargeID", "LongDescription");
            ViewData["OrderTypes"] = new SelectList(_repository.OrderTypes, "OrderTypeId", "OrderName");
            ViewData["Priorities"] = new SelectList(_repository.Priorities, "PriorityId", "PriorityName");
            ViewData["Providers"] = new SelectList(queryPhysician, "PhysicianId", "FullName", 0);
            ViewData["Cosigners"] = new SelectList(queryCosigner, "PhysicianId", "FullName", 0);

            ViewBag.Encounter = encounter;
            ViewBag.Patient = patient;

            var model = new OrdersViewModel(encounter, patient);

            return View(model);
        }

        ///<summary>
        /// Post for CreateOrder
        ///</summary>
        /// param name = "model"
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("OrdersAdd")]
        public IActionResult CreateOrder(OrdersViewModel model)
        {
            if(ModelState.IsValid)
            {
                foreach(var key in ModelState.Keys)
                {
                    var value = ModelState[key].AttemptedValue;
                    Console.WriteLine($"{key}: {value}");
                }
                
                OrderInfo oi = model.OrderInfo;

                // the model IsFasting field would not bind, so a View Model field is used and passed directly:  IsFastingVm
                oi.IsFasting = model.IsFastingVm;

                _repository.AddOrderInfo(oi);

                return RedirectToAction("ListOrders",new {id = oi.EncounterId});
            }
            
            return View(model);
        }

        ///<summary>
        ///Getter for EditOrder, using the OrderInfoId
        ///</summary>
        ///<param name="id"
        [Authorize]
        [PermissionAuthorize("OrdersEdit")]
        public IActionResult EditOrder(long id)
        {
            var queryPhysician = _repository.Physicians
                .Include(p => p.Specialty)
                .OrderBy(p => p.LastName)
                .Select(p => new {p.PhysicianId, FullName = p.FirstName +" "+ p.LastName+",  "+p.Specialty.Name})
                .ToList();

            var queryCosigner = _repository.Physicians
                .Include(p => p.Specialty)
                .Where(p => p.ProviderStatusId == 1)
                .OrderBy(p => p.LastName)
                .Select(p => new {p.PhysicianId, FullName = p.FirstName +" "+ p.LastName+",  "+p.Specialty.Name})
                .ToList();

            ViewData["Providers"] = new SelectList(queryPhysician, "PhysicianId", "FullName", 0);
            ViewData["Cosigners"] = new SelectList(queryCosigner, "PhysicianId", "FullName", 0);
            ViewData["OrderTypes"] = new SelectList(_repository.OrderTypes, "OrderTypeId", "OrderName");
            ViewData["Priorities"] = new SelectList(_repository.Priorities, "PriorityId", "PriorityName");

            OrderInfo orderInfo = _repository.OrderInfos.FirstOrDefault(oi => oi.OrderInfoId == id);

            var encounter = _repository.Encounters
                                .Include(p =>p.EncounterPhysicians)
                                .ThenInclude(ph => ph.Physician)
                                .Include(et => et.EncounterType)
                                .Include(d => d.Department)
                                .FirstOrDefault(b => b.EncounterId == orderInfo.EncounterId);

            var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == encounter.Mrn);

            ViewBag.Encounter = encounter;
            ViewBag.Patient = patient;

            return View(new OrdersViewModel (encounter, patient, orderInfo));
        }

        ///<summary>
        /// Post for EditOrder
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("OrdersEdit")]
        public IActionResult EditOrder(OrdersViewModel model)
        {
            OrderInfo oi = model.OrderInfo;

            // the model IsFasting field would not bind, so a View Model field is used and passed directly:  IsFastingVm
            oi.IsFasting = model.IsFastingVm;

            _repository.EditOrderInfo(oi);

            return RedirectToAction("ListOrders", new{id = oi.EncounterId});
        }

        ///<summary>
        ///Getter for DeleteOrder
        ///</summary>
        [Authorize]
        [PermissionAuthorize("OrdersDelete")]
        public IActionResult DeleteOrder(long id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var order = _repository.OrderInfos
                .Include(o => o.OrderingProvider)
                    .ThenInclude(op => op.ProviderStatus)
                .Include(o => o.AuthenticatingProvider)
                .Include(o => o.Author)
                .Include(o => o.OrderType)
                .Include(o => o.Priority)
                .Include(o => o.CompletedByProvider)
                .Include(o => o.ChargeDefinition)
                .FirstOrDefault(b => b.OrderInfoId == id);

            var encounter = _repository.Encounters
                .Include(p =>p.EncounterPhysicians)
                    .ThenInclude(ph => ph.Physician)
                .Include(d => d.Department)
                .Include(e => e.EncounterType)
                .FirstOrDefault(b => b.EncounterId == order.EncounterId);

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);

            return View(new OrdersViewModel
            {
                OrderInfo     = order,
                Patient   = patient,
                Encounter = encounter
            });
        }

        ///<summary>
        ///Post for DeleteOrder
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("OrdersDelete")]
        public IActionResult DeleteOrder(OrdersViewModel model)
        {
            try
            {
                _repository.DeleteOrderInfo(model.OrderInfo);

                return RedirectToAction("ListOrders", new{id = model.OrderInfo.EncounterId});
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Order.  Delete not avalable";

                return View(model);
            }
        }
        /// <summary>
        /// Getter for Completing an Order
        /// </summary>
        /// <param name="orderId">Id of unique order</param> 
        public IActionResult CompleteOrder(long orderId)
        {
            var order = _repository.OrderInfos
                .Include(o => o.OrderingProvider)
                    .ThenInclude(op => op.ProviderStatus)
                .Include(o => o.AuthenticatingProvider)
                .Include(o => o.Author)
                .Include(o => o.OrderType)
                .Include(o => o.Priority)
                .Include(o => o.CompletedByProvider)
                .Include(o => o.ChargeDefinition)
                .FirstOrDefault(b => b.OrderInfoId == orderId);

           var encounter = _repository.Encounters
                .Include(ep => ep.EncounterPhysicians)
                    .ThenInclude(p => p.Physician)
                .Include(d => d.Department)
                .Include(e => e.EncounterType)
                .FirstOrDefault(b => b.EncounterId == order.EncounterId);

            var patient = _repository.Patients
                .Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == encounter.Mrn);

            var queryPhysician = _repository.Physicians
                .OrderBy(p => p.LastName)
                .Select(p => new {p.PhysicianId, FullName = p.FirstName +" "+ p.LastName})
                .ToList();

            var queryCosigner = _repository.Physicians
                .Where(p => p.ProviderStatusId == 1)
                .OrderBy(p => p.LastName)
                .Select(p => new {p.PhysicianId, FullName = p.FirstName +" "+ p.LastName})
                .ToList();

            ViewBag.Providers = new SelectList(queryPhysician, "PhysicianId", "FullName", 0);
            ViewBag.Cosigners = new SelectList(queryCosigner,  "PhysicianId", "FullName",  0);

            return View(new OrdersViewModel
            {
                OrderInfo = order,
                Patient   = patient,
                Encounter = encounter
            });
        }

        ///<summary>
        /// Post for CompleteOrder
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("OrdersEdit")]
        public IActionResult CompleteOrder(OrdersViewModel model)
        {
            var orderInfo = _repository.OrderInfos.Where(o => o.OrderInfoId == model.OrderInfo.OrderInfoId).FirstOrDefault();
            orderInfo.IsOrderComplete = model.OrderInfo.IsOrderComplete;
            orderInfo.OrderCompletedDateTime= model.OrderInfo.OrderCompletedDateTime;
            orderInfo.OrderCompletedByID = model.OrderInfo.OrderCompletedByID;

            //append any Order Completion notes to the existing notes field
            if(!string.IsNullOrEmpty(model.CompletionNotes))
            {
                if(string.IsNullOrEmpty(orderInfo.Notes))
                {
                    orderInfo.Notes = model.CompletionNotes;
                }
                else
                {
                    orderInfo.Notes += "\n" + model.CompletionNotes;
                } 
            }

            _repository.EditOrderInfo(orderInfo);

            return RedirectToAction("ListOrders", new{id = model.OrderInfo.EncounterId});
        }        
    }
}