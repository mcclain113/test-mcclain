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
using Microsoft.EntityFrameworkCore;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class PCAStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        
        public PCAStructuredDataController(IWCTCHealthSystemRepository repo) 
        {
            _repository = repo;
        } 

        /// <summary>
        /// Index page of PCAStructuredData
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult Index() 
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View();
        }

        #region Blood Pressure Route Type
        ///<summary>
        ///     List View of all Blood Pressure Route Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewBPRouteTypes()
        {
            List<BloodPressureRouteType> bloodPressureRouteTypes = _repository.BloodPressureRouteTypes.OrderBy(bp => bp.Name).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("BloodPressureRouteType/ViewBPRouteTypes",bloodPressureRouteTypes);
        }

        /// <summary>
        ///     Create New Blood Pressure Route Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateBPRouteType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("BloodPressureRouteType/CreateBPRouteType");
        }

        ///<summary>
        ///     Add new Blood Pressure Route Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateBPRouteType(PcaStructuredDataViewModel model)
        {                       
            model.BloodPressureRouteType.LastModified = DateTime.Now;
            _repository.AddBloodPressureRouteType(model.BloodPressureRouteType);

            return RedirectToAction("ViewBPRouteTypes");
        }

        ///<summary>
        ///     Edit fields of an existing Blood Pressure Route Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBPRouteType(byte id) 
        {
            BloodPressureRouteType model = _repository.BloodPressureRouteTypes.FirstOrDefault(p => p.BloodPressureRouteTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("BloodPressureRouteType/EditBPRouteType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Blood Pressure Route Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBPRouteType(PcaStructuredDataViewModel model) 
        {
            model.BloodPressureRouteType.LastModified = DateTime.Now;
            _repository.EditBloodPressureRouteType(model.BloodPressureRouteType);

            return RedirectToAction("ViewBPRouteTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteBPRouteType(byte id)
        {
            var bpRouteTypeToDelete = _repository.BloodPressureRouteTypes.FirstOrDefault(bp => bp.BloodPressureRouteTypeId == id);
            try
            {
                if(bpRouteTypeToDelete != null)
                {
                    _repository.DeleteBloodPressureRouteType(bpRouteTypeToDelete);
                    TempData["SuccessMessage"] = "The Blood Pressure Route Type was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Blood Pressure Route Type was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Blood Pressure Route Type could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewBPRouteTypes");
        }

        #endregion  // end of Blood Pressure Route Type section

        #region Bmi Method
        ///<summary>
        ///     List View of all BMI Methods
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewBmiMethods()
        {
            List<Bmimethod> bmiMethods = _repository.BmiMethods.OrderBy(bp => bp.Name).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("BmiMethod/ViewBmiMethods",bmiMethods);
        }

        /// <summary>
        ///     Create New BMI Methods - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateBmiMethod()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("BmiMethod/CreateBmiMethod");
        }

        ///<summary>
        ///     Add new BMI Methods to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateBmiMethod(PcaStructuredDataViewModel model)
        {                       
            model.Bmimethod.LastModified = DateTime.Now;
            _repository.AddBmiMethod(model.Bmimethod);

            return RedirectToAction("ViewBmiMethods");
        }

        ///<summary>
        ///     Edit fields of an existing BMI Methods - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBmiMethod(byte id) 
        {
            Bmimethod model = _repository.BmiMethods.FirstOrDefault(p => p.BmimethodId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("BmiMethod/EditBmiMethod",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing BMI Methods in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBmiMethod(PcaStructuredDataViewModel model) 
        {
            model.Bmimethod.LastModified = DateTime.Now;
            _repository.EditBmiMethod(model.Bmimethod);

            return RedirectToAction("ViewBmiMethods");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteBmiMethod(byte id)
        {
            var bmiMethodToDelete = _repository.BmiMethods.FirstOrDefault(bp => bp.BmimethodId == id);
            try
            {
                if(bmiMethodToDelete != null)
                {
                    _repository.DeleteBmiMethod(bmiMethodToDelete);
                    TempData["SuccessMessage"] = "The BMI Method was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified BMI Method was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The BMI Method could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewBmiMethods");
        }
        #endregion  // end of the BMI Method section

        #region Care System Parameter
        ///<summary>
        ///     List View of all Care System Parameters
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewCareSystemParameters()
        {
            List<CareSystemParameter> careSystemParameters = _repository.CareSystemParameters
                .Include(c => c.CareSystemType)
                .OrderBy(c => c.CareSystemType.Name)
                .ThenBy(c => c.Name)
                .ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("CareSystemParameter/ViewCareSystemParameters",careSystemParameters);
        }

        /// <summary>
        ///     Create New Care System Parameter - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCareSystemParameter()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var careSystemTypes = new SelectList(_repository.CareSystemAssessmentTypes.OrderBy(c => c.Name), "CareSystemTypeId", "Name");
                var careSystemTypesList = careSystemTypes.ToList();
                careSystemTypesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["CareSystemTypeId"] = careSystemTypesList;

            return View("CareSystemParameter/CreateCareSystemParameter");
        }

        ///<summary>
        ///     Add new Care System Parameter to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateCareSystemParameter(PcaStructuredDataViewModel model)
        {                       
            model.CareSystemParameter.LastModified = DateTime.Now;
            _repository.AddCareSystemParameter(model.CareSystemParameter);

            return RedirectToAction("ViewCareSystemParameters");
        }

        ///<summary>
        ///     Edit fields of an existing Care System Parameter - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCareSystemParameter(short id) 
        {
            CareSystemParameter model = _repository.CareSystemParameters.FirstOrDefault(p => p.CareSystemParameterId == id);

            var careSystemTypes = new SelectList(_repository.CareSystemAssessmentTypes.OrderBy(c => c.Name), "CareSystemTypeId", "Name");
                var careSystemTypesList = careSystemTypes.ToList();
                careSystemTypesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["CareSystemTypeId"] = careSystemTypesList;

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("CareSystemParameter/EditCareSystemParameter",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Care System Parameter in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCareSystemParameter(PcaStructuredDataViewModel model) 
        {
            model.CareSystemParameter.LastModified = DateTime.Now;
            _repository.EditCareSystemParameter(model.CareSystemParameter);

            return RedirectToAction("ViewCareSystemParameters");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCareSystemParameter(short id)
        {
            var careSystemParameterToDelete = _repository.CareSystemParameters.FirstOrDefault(p => p.CareSystemParameterId == id);
            try
            {
                if(careSystemParameterToDelete != null)
                {
                    _repository.DeleteCareSystemParameter(careSystemParameterToDelete);
                    TempData["SuccessMessage"] = "The Care System Parameter was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Care System Parameter was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Care System Parameter could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewCareSystemParameters");
        }
        #endregion  // end of the Care System Parameter section

        #region Care System Type
        ///<summary>
        ///     List View of all Care System Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewCareSystemTypes()
        {
            List<CareSystemType> careSystemTypes = _repository.CareSystemAssessmentTypes.OrderBy(c => c.Name).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("CareSystemType/ViewCareSystemTypes",careSystemTypes);
        }

        /// <summary>
        ///     Create New Care System Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCareSystemType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("CareSystemType/CreateCareSystemType");
        }

        ///<summary>
        ///     Add new Care System Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateCareSystemType(PcaStructuredDataViewModel model)
        {                       
            model.CareSystemType.LastModified = DateTime.Now;
            _repository.AddCareSystemType(model.CareSystemType);

            return RedirectToAction("ViewCareSystemTypes");
        }

        ///<summary>
        ///     Edit fields of an existing Care System Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCareSystemType(byte id) 
        {
            CareSystemType model = _repository.CareSystemAssessmentTypes.FirstOrDefault(c => c.CareSystemTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("CareSystemType/EditCareSystemType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Care System Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCareSystemType(PcaStructuredDataViewModel model) 
        {
            model.CareSystemType.LastModified = DateTime.Now;
            _repository.EditCareSystemType(model.CareSystemType);

            return RedirectToAction("ViewCareSystemTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCareSystemType(byte id)
        {
            var careSystemTypeToDelete = _repository.CareSystemAssessmentTypes.FirstOrDefault(c => c.CareSystemTypeId == id);
            try
            {
                if(careSystemTypeToDelete != null)
                {
                    _repository.DeleteCareSystemType(careSystemTypeToDelete);
                    TempData["SuccessMessage"] = "The CareSystemType was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified CareSystemType was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The CareSystemType could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewCareSystemTypes");
        }
        #endregion  // end of the Care System Type section

        #region O2 Delivery Type
        ///<summary>
        ///     List View of all O2 Delivery Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewO2DeliveryTypes()
        {
            List<O2deliveryType> o2deliveryType = _repository.O2DeliveryTypes.OrderBy(o => o.O2deliveryTypeName).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("O2DeliveryType/ViewO2DeliveryTypes",o2deliveryType);
        }

        /// <summary>
        ///     Create New O2 Delivery Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateO2DeliveryType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("O2DeliveryType/CreateO2DeliveryType");
        }

        ///<summary>
        ///     Add new O2 Delivery Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateO2DeliveryType(PcaStructuredDataViewModel model)
        {                       
            model.O2deliveryType.LastModified = DateTime.Now;
            _repository.AddO2DeliveryType(model.O2deliveryType);

            return RedirectToAction("ViewO2DeliveryTypes");
        }

        ///<summary>
        ///     Edit fields of an existing O2 Delivery Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditO2DeliveryType(byte id) 
        {
            O2deliveryType model = _repository.O2DeliveryTypes.FirstOrDefault(c => c.O2deliveryTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("O2DeliveryType/EditO2DeliveryType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing O2 Delivery Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditO2DeliveryType(PcaStructuredDataViewModel model) 
        {
            model.O2deliveryType.LastModified = DateTime.Now;
            _repository.EditO2DeliveryType(model.O2deliveryType);

            return RedirectToAction("ViewO2DeliveryTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteO2DeliveryType(byte id)
        {
            var o2deliveryTypeToDelete = _repository.O2DeliveryTypes.FirstOrDefault(o => o.O2deliveryTypeId == id);
            try
            {
                if(o2deliveryTypeToDelete != null)
                {
                    _repository.DeleteO2DeliveryType(o2deliveryTypeToDelete);
                    TempData["SuccessMessage"] = "The O2 Delivery Type was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified O2 Delivery Type was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The O2 Delivery Type could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewO2DeliveryTypes");
        }
        #endregion  // end of the O2 Delivery Type section

        #region Pain Parameter
        ///<summary>
        ///     List View of all Pain Parameters
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPainParameters()
        {
            List<PainParameter> painParameters = _repository.PainParameters
                .Include(c => c.PainScaleType)
                .OrderBy(c => c.PainScaleType.PainScaleTypeName)
                .ThenBy(c => c.ParameterName)
                .ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainParameter/ViewPainParameters",painParameters);
        }

        /// <summary>
        ///     Create New Pain Parameter - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePainParameter()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var painScaleTypes = new SelectList(_repository.PainScaleTypes.OrderBy(c => c.PainScaleTypeName), "PainScaleTypeId", "PainScaleTypeName");
                var painScaleTypesList = painScaleTypes.ToList();
                painScaleTypesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["PainScaleTypeId"] = painScaleTypesList;

            return View("PainParameter/CreatePainParameter");
        }

        ///<summary>
        ///     Add new Pain Parameter to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePainParameter(PcaStructuredDataViewModel model)
        {                       
            model.PainParameter.LastModified = DateTime.Now;
            _repository.AddPainParameter(model.PainParameter);

            return RedirectToAction("ViewPainParameters");
        }

        ///<summary>
        ///     Edit fields of an existing Pain Parameter - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPainParameter(int id) 
        {
            PainParameter model = _repository.PainParameters.FirstOrDefault(p => p.PainParameterId == id);

            var painScaleTypes = new SelectList(_repository.PainScaleTypes.OrderBy(c => c.PainScaleTypeName), "PainScaleTypeId", "PainScaleTypeName");
                var painScaleTypesList = painScaleTypes.ToList();
                painScaleTypesList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["PainScaleTypeId"] = painScaleTypesList;

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainParameter/EditPainParameter",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Pain Parameter in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPainParameter(PcaStructuredDataViewModel model) 
        {
            model.PainParameter.LastModified = DateTime.Now;
            _repository.EditPainParameter(model.PainParameter);

            return RedirectToAction("ViewPainParameters");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePainParameter(int id)
        {
            var painParameterToDelete = _repository.PainParameters.FirstOrDefault(p => p.PainParameterId == id);
            try
            {
                if(painParameterToDelete != null)
                {
                    _repository.DeletePainParameter(painParameterToDelete);
                    TempData["SuccessMessage"] = "The Pain Parameter was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Pain Parameter was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Pain Parameter could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewPainParameters");
        }
        #endregion  // end of the Pain Parameter section

        #region Pain Rating
        ///<summary>
        ///     List View of all Pain Ratings
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPainRatings()
        {
            List<PainRating> painRatings = _repository.PainRatings
                .Include(c => c.PainParameter)
                .OrderBy(c => c.PainParameter.ParameterName)
                .ThenBy(c => c.Value)
                .ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainRating/ViewPainRatings",painRatings);
        }

        /// <summary>
        ///     Create New Pain Rating - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePainRating()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var painParameters = new SelectList(_repository.PainParameters
                .OrderBy(c => c.PainScaleType.PainScaleTypeName)
                .Select(c => new {
                    c.PainParameterId,
                    DisplayText = c.ParameterName + ":  " + c.Description
                }), 
                "PainParameterId",
                "DisplayText"
            );
                var painParametersList = painParameters.ToList();
                painParametersList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["PainParameterId"] = painParametersList;

            return View("PainRating/CreatePainRating");
        }

        ///<summary>
        ///     Add new Pain Rating to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePainRating(PcaStructuredDataViewModel model)
        {                       
            model.PainRating.LastModified = DateTime.Now;
            _repository.AddPainRating(model.PainRating);

            return RedirectToAction("ViewPainRatings");
        }

        ///<summary>
        ///     Edit fields of an existing Pain Rating - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPainRating(int id) 
        {
            PainRating model = _repository.PainRatings.FirstOrDefault(p => p.PainRatingId == id);

            var painParameters = new SelectList(_repository.PainParameters
                .OrderBy(c => c.PainScaleType.PainScaleTypeName)
                .Select(c => new {
                    c.PainParameterId,
                    DisplayText = c.ParameterName + ":  " + c.Description
                }), 
                "PainParameterId",
                "DisplayText"
            );
                var painParametersList = painParameters.ToList();
                painParametersList.Insert(0,new SelectListItem {Value = "", Text = ""});
            ViewData["PainParameterId"] = painParametersList;

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainRating/EditPainRating",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Pain Rating in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPainRating(PcaStructuredDataViewModel model) 
        {
            model.PainRating.LastModified = DateTime.Now;
            _repository.EditPainRating(model.PainRating);

            return RedirectToAction("ViewPainRatings");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePainRating(int id)
        {
            var painRatingToDelete = _repository.PainRatings.FirstOrDefault(p => p.PainRatingId == id);
            try
            {
                if(painRatingToDelete != null)
                {
                    _repository.DeletePainRating(painRatingToDelete);
                    TempData["SuccessMessage"] = "The Pain Rating was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Pain Rating was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Pain Rating could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewPainRatings");
        }
        #endregion  // end of the Pain Rating section

        #region  Pain Scale Type
        ///<summary>
        ///     List View of all Pain Scale Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPainScaleTypes()
        {
            List<PainScaleType> painScaleType = _repository.PainScaleTypes.OrderBy(p => p.PainScaleTypeName).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainScaleType/ViewPainScaleTypes",painScaleType);
        }

        /// <summary>
        ///     Create New Pain Scale Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePainScaleType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainScaleType/CreatePainScaleType");
        }

        ///<summary>
        ///     Add new Pain Scale Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePainScaleType(PcaStructuredDataViewModel model)
        {                       
            model.PainScaleType.LastModified = DateTime.Now;
            _repository.AddPainScaleType(model.PainScaleType);

            return RedirectToAction("ViewPainScaleTypes");
        }

        ///<summary>
        ///     Edit fields of an existing Pain Scale Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPainScaleType(int id) 
        {
            PainScaleType model = _repository.PainScaleTypes.FirstOrDefault(p => p.PainScaleTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PainScaleType/EditPainScaleType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Pain Scale Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPainScaleType(PcaStructuredDataViewModel model) 
        {
            model.PainScaleType.LastModified = DateTime.Now;
            _repository.EditPainScaleType(model.PainScaleType);

            return RedirectToAction("ViewPainScaleTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePainScaleType(int id)
        {
            var painScaleTypeToDelete = _repository.PainScaleTypes.FirstOrDefault(o => o.PainScaleTypeId == id);
            try
            {
                if(painScaleTypeToDelete != null)
                {
                    _repository.DeletePainScaleType(painScaleTypeToDelete);
                    TempData["SuccessMessage"] = "The Pain Scale Type was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Pain Scale Type was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Pain Scale Type could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewPainScaleTypes");
        }
        #endregion  // end of the Pain Scale Type section

        #region PCA Comment Type
        ///<summary>
        ///     List View of all PCA Comment Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPcaCommentTypes()
        {
            List<PcacommentType> pcacommentTypes = _repository.PcaCommentTypes.OrderBy(p => p.PcacommentTypeName).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PcaCommentType/ViewPcaCommentTypes",pcacommentTypes);
        }

        /// <summary>
        ///     Create New PCA Comment Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePcaCommentType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PcaCommentType/CreatePcaCommentType");
        }

        ///<summary>
        ///     Add new PCA Comment Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePcaCommentType(PcaStructuredDataViewModel model)
        {                       
            model.PcacommentType.LastModified = DateTime.Now;
            _repository.AddPcaCommentType(model.PcacommentType);

            return RedirectToAction("ViewPcaCommentTypes");
        }

        ///<summary>
        ///     Edit fields of an existing PCA Comment Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPcaCommentType(int id) 
        {
            PcacommentType model = _repository.PcaCommentTypes.FirstOrDefault(p => p.PcacommentTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PcaCommentType/EditPcaCommentType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing PCA Comment Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPcaCommentType(PcaStructuredDataViewModel model) 
        {
            model.PcacommentType.LastModified = DateTime.Now;
            _repository.EditPcaCommentType(model.PcacommentType);

            return RedirectToAction("ViewPcaCommentTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePcaCommentType(int id)
        {
            var pcaCommentTypeToDelete = _repository.PcaCommentTypes.FirstOrDefault(p => p.PcacommentTypeId == id);
            try
            {
                if(pcaCommentTypeToDelete != null)
                {
                    _repository.DeletePcaCommentType(pcaCommentTypeToDelete);
                    TempData["SuccessMessage"] = "The PCA Comment Type was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified PCA Comment Type was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The PCA Comment Type could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewPcaCommentTypes");
        }
        #endregion  // end of the PCA Comment Type section

        #region Pulse Route Type
        ///<summary>
        ///     List View of all Pulse Route Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPulseRouteTypes()
        {
            List<PulseRouteType> pulseRouteTypes = _repository.PulseRouteTypes.OrderBy(p => p.PulseRouteTypeName).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PulseRouteType/ViewPulseRouteTypes",pulseRouteTypes);
        }

        /// <summary>
        ///     Create New Pulse Route Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePulseRouteType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PulseRouteType/CreatePulseRouteType");
        }

        ///<summary>
        ///     Add new Pulse Route Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePulseRouteType(PcaStructuredDataViewModel model)
        {                       
            model.PulseRouteType.LastModified = DateTime.Now;
            _repository.AddPulseRouteType(model.PulseRouteType);

            return RedirectToAction("ViewPulseRouteTypes");
        }

        ///<summary>
        ///     Edit fields of an existing Pulse Route Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPulseRouteType(int id) 
        {
            PulseRouteType model = _repository.PulseRouteTypes.FirstOrDefault(p => p.PulseRouteTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("PulseRouteType/EditPulseRouteType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Pulse Route Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPulseRouteType(PcaStructuredDataViewModel model) 
        {
            model.PulseRouteType.LastModified = DateTime.Now;
            _repository.EditPulseRouteType(model.PulseRouteType);

            return RedirectToAction("ViewPulseRouteTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePulseRouteType(int id)
        {
            var pulseRouteTypeToDelete = _repository.PulseRouteTypes.FirstOrDefault(p => p.PulseRouteTypeId == id);
            try
            {
                if(pulseRouteTypeToDelete != null)
                {
                    _repository.DeletePulseRouteType(pulseRouteTypeToDelete);
                    TempData["SuccessMessage"] = "The Pulse Route Type was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Pulse Route Type was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Pulse Route Type could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewPulseRouteTypes");
        }
        #endregion  // end of the Pulse Route Type section

        #region Temp Route Type
        ///<summary>
        ///     List View of all Temp Route Types
        ///</summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewTempRouteTypes()
        {
            List<TempRouteType> tempRouteTypes = _repository.TempRouteTypes.OrderBy(p => p.TempRouteTypeName).ToList();

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("TempRouteType/ViewTempRouteTypes",tempRouteTypes);
        }

        /// <summary>
        ///     Create New Temp Route Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateTempRouteType()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("TempRouteType/CreateTempRouteType");
        }

        ///<summary>
        ///     Add new Temp Route Type to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateTempRouteType(PcaStructuredDataViewModel model)
        {                       
            model.TempRouteType.LastModified = DateTime.Now;
            _repository.AddTempRouteType(model.TempRouteType);

            return RedirectToAction("ViewTempRouteTypes");
        }

        ///<summary>
        ///     Edit fields of an existing Temp Route Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditTempRouteType(int id) 
        {
            TempRouteType model = _repository.TempRouteTypes.FirstOrDefault(p => p.TempRouteTypeId == id);

                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View("TempRouteType/EditTempRouteType",new PcaStructuredDataViewModel (model));
        }

        ///<summary>
        ///     Edit fields of an existing Temp Route Type in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditTempRouteType(PcaStructuredDataViewModel model) 
        {
            model.TempRouteType.LastModified = DateTime.Now;
            _repository.EditTempRouteType(model.TempRouteType);

            return RedirectToAction("ViewTempRouteTypes");
        }

        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteTempRouteType(int id)
        {
            var tempRouteTypeToDelete = _repository.TempRouteTypes.FirstOrDefault(p => p.TempRouteTypeId == id);
            try
            {
                if(tempRouteTypeToDelete != null)
                {
                    _repository.DeleteTempRouteType(tempRouteTypeToDelete);
                    TempData["SuccessMessage"] = "The Temp Route Type was successfully deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The specified Temp Route Type was not found.";
                }
                
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The Temp Route Type could not be deleted, most likely because it is in use.";
            }
            return RedirectToAction("ViewTempRouteTypes");
        }
        #endregion  // end of the Temperature Route Type section
    }
}