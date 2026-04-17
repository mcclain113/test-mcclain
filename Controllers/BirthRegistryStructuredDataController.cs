using System;
using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd", "StructuredDataView", "StructuredDataEdit", "StructuredDataDelete")]
    public class BirthRegistryStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public BirthRegistryStructuredDataController(IWCTCHealthSystemRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Index page of Birth Registry Structured Data
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

        #region Abnormal Conditions
        /// <summary>
        ///     Index page of Abnormal Conditions
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewAbnormalConditions()
        {
            List<AbnormalCondition> abnormalConditions = _repository.AbnormalConditions
                                .ToList();
            return View("AbnormalCondition/ViewAbnormalConditions", abnormalConditions);
        }

        /// <summary>
        ///     Create New AbnormalCondition - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAbnormalCondition()
        {
            return View("AbnormalCondition/CreateAbnormalCondition");
        }

        ///<summary>
        ///     Add new AbnormalCondition to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAbnormalCondition(AbnormalCondition model)
        {
            _repository.AddAbnormalCondition(model);

            return RedirectToAction("ViewAbnormalConditions");
        }

        ///<summary>
        ///     Edit fields of an existing Abnormal Condition - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAbnormalCondition(byte id)
        {
            var model = _repository.AbnormalConditions.FirstOrDefault(rp => rp.AbnormalConditionId == id);

            if (model == null) return NotFound();

            return View("AbnormalCondition/EditAbnormalCondition", model);
        }

        ///<summary>
        ///     Edit fields of an existing Abnormal Condition in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAbnormalCondition(AbnormalCondition model)
        {
            _repository.EditAbnormalCondition(model);

            return RedirectToAction("ViewAbnormalConditions");
        }

        ///<summary>
        ///     Delete an existing Abnormal Condition - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAbnormalCondition(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.AbnormalConditions.FirstOrDefault(rp => rp.AbnormalConditionId == id);
            return View("AbnormalCondition/DeleteAbnormalCondition", model);
        }

        ///<summary>
        ///     Delete an existing Abnormal Condition in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAbnormalCondition(AbnormalCondition model)
        {
            try
            {
                _repository.DeleteAbnormalCondition(model);


                return RedirectToAction("ViewAbnormalConditions");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Abnormal Condition. Delete not available.";
                return View("AbnormalCondition/DeleteAbnormalCondition", model);
            }

        }

        #endregion

        #region Birthplace Type
        /// <summary>
        ///     Index page of Birthplace Types
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewBirthplaceTypes()
        {
            List<BirthPlaceType> birthplaceTypes = _repository.BirthPlaceTypes
                                .ToList();
            return View("BirthplaceType/ViewBirthplaceTypes", birthplaceTypes);
        }

        /// <summary>
        ///     Create New Birth Place Type - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateBirthPlaceType()
        {
            return View("BirthPlaceType/CreateBirthPlaceType");
        }

        ///<summary>
        ///     Add new Birth Place Type to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateBirthPlaceType(BirthPlaceType model)
        {
            _repository.AddBirthPlaceType(model);

            return RedirectToAction("ViewBirthPlaceTypes");
        }

        ///<summary>
        ///     Edit fields of an existing Birth Place Type - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBirthPlaceType(byte id)
        {
            var model = _repository.BirthPlaceTypes.FirstOrDefault(rp => rp.BirthPlaceTypeId == id);

            if (model == null) return NotFound();

            return View("BirthplaceType/EditBirthPlaceType", model);
        }

        ///<summary>
        ///     Edit fields of an existing Birth Place Type in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBirthPlaceType(BirthPlaceType model)
        {
            _repository.EditBirthPlaceType(model);

            return RedirectToAction("ViewBirthPlaceTypes");
        }

        ///<summary>
        ///     Delete an existing Birth Place Type - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteBirthPlaceType(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.BirthPlaceTypes.FirstOrDefault(rp => rp.BirthPlaceTypeId == id);
            return View("BirthplaceType/DeleteBirthPlaceType", model);
        }

        ///<summary>
        ///     Delete an existing Birth Place Type in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteBirthPlaceType(BirthPlaceType model)
        {
            try
            {
                _repository.DeleteBirthPlaceType(model);


                return RedirectToAction("ViewBirthPlaceTypes");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Abnormal Condition. Delete not available.";
                return View("BirthplaceType/DeleteBirthPlaceType", model);
            }

        }

        #endregion

        #region Characteristics of Labor
        /// <summary>
        ///     Index page of Characteristics Of Labor
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewCharacteristicsOfLabors()
        {
            List<CharacteristicOfLabor> characteristicOfLabors = _repository.CharacteristicOfLabors
                                .ToList();
            return View("CharacteristicOfLabor/ViewCharacteristicsOfLabors", characteristicOfLabors);
        }

        /// <summary>
        ///     Create New Characteristic Of Labor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCharacteristicOfLabor()
        {
            return View("CharacteristicOfLabor/CreateCharacteristicOfLabor");
        }

        ///<summary>
        ///     Add new Characteristic Of Labor to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCharacteristicOfLabor(CharacteristicOfLabor model)
        {
            _repository.AddCharacteristicOfLabor(model);

            return RedirectToAction("ViewCharacteristicsOfLabors");
        }

        ///<summary>
        ///     Edit fields of an existing CharacteristicOfLabor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCharacteristicOfLabor(byte id)
        {
            var model = _repository.CharacteristicOfLabors.FirstOrDefault(rp => rp.CharacteristicId == id);

            if (model == null) return NotFound();

            return View("CharacteristicOfLabor/EditCharacteristicOfLabor", model);
        }

        ///<summary>
        ///     Edit fields of an existing CharacteristicOfLabor in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCharacteristicOfLabor(CharacteristicOfLabor model)
        {
            _repository.EditCharacteristicOfLabor(model);

            return RedirectToAction("ViewCharacteristicsOfLabors");
        }

        ///<summary>
        ///     Delete an existing Characteristic Of Labor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCharacteristicOfLabor(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.CharacteristicOfLabors.FirstOrDefault(rp => rp.CharacteristicId == id);
            return View("CharacteristicOfLabor/DeleteCharacteristicOfLabor", model);
        }

        ///<summary>
        ///     Delete an existing CharacteristicOfLabor in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCharacteristicOfLabor(CharacteristicOfLabor model)
        {
            try
            {
                _repository.DeleteCharacteristicOfLabor(model);


                return RedirectToAction("ViewCharacteristicsOfLabors");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Characteristic Of Labor. Delete not available.";
                return View("CharacteristicOfLabor/DeleteCharacteristicOfLabor", model);
            }

        }

        #endregion

        #region Congenital Anomaly
        /// <summary>
        ///     Index page of Congenital Anomalies
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewCongenitalAnomalies()
        {
            List<CongenitalAnomaly> congenitalAnomalies = _repository.CongenitalAnomalies
                                .ToList();
            return View("CongenitalAnomaly/ViewCongenitalAnomalies", congenitalAnomalies);
        }

        /// <summary>
        ///     Create New Congenital Anomaly - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCongenitalAnomaly()
        {
            return View("CongenitalAnomaly/CreateCongenitalAnomaly");
        }

        ///<summary>
        ///     Add new Congenital Anomaly to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCongenitalAnomaly(CongenitalAnomaly model)
        {
            _repository.AddCongenitalAnomaly(model);

            return RedirectToAction("ViewCongenitalAnomalies");
        }

        ///<summary>
        ///     Edit fields of an existing Congenital Anomaly - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCongenitalAnomaly(byte id)
        {
            var model = _repository.CongenitalAnomalies.FirstOrDefault(rp => rp.CongenitalAnomalyId == id);

            if (model == null) return NotFound();

            return View("CongenitalAnomaly/EditCongenitalAnomaly", model);
        }

        ///<summary>
        ///     Edit fields of an existing Congenital Anomaly in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCongenitalAnomaly(CongenitalAnomaly model)
        {
            _repository.EditCongenitalAnomaly(model);

            return RedirectToAction("ViewCongenitalAnomalies");
        }

        ///<summary>
        ///     Delete an existing Congenital Anomaly - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCongenitalAnomaly(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.CongenitalAnomalies.FirstOrDefault(rp => rp.CongenitalAnomalyId == id);
            return View("CongenitalAnomaly/DeleteCongenitalAnomaly", model);
        }

        ///<summary>
        ///     Delete an existing Congenital Anomaly in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCongenitalAnomaly(CongenitalAnomaly model)
        {
            try
            {
                _repository.DeleteCongenitalAnomaly(model);


                return RedirectToAction("ViewCongenitalAnomalies");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Congenital Anomaly. Delete not available.";
                return View("CongenitalAnomaly/DeleteCongenitalAnomaly", model);
            }

        }

        #endregion

        #region Fetal Presentation at Birth
        /// <summary>
        ///     Index page of Fetal Presentation At Birth
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewFetalPresentationAtBirths()
        {
            List<FetalPresentationAtBirth> fetalPresentationAtBirths = _repository.FetalPresentationAtBirths
                                .ToList();
            return View("FetalPresentationAtBirth/ViewFetalPresentationAtBirths", fetalPresentationAtBirths);
        }

        /// <summary>
        ///     Create New Fetal Presentation At Birth - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFetalPresentationAtBirth()
        {
            return View("FetalPresentationAtBirth/CreateFetalPresentationAtBirth");
        }

        ///<summary>
        ///     Add new Fetal Presentation At Birth to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFetalPresentationAtBirth(FetalPresentationAtBirth model)
        {
            _repository.AddFetalPresentationAtBirth(model);

            return RedirectToAction("ViewFetalPresentationAtBirths");
        }

        ///<summary>
        ///     Edit fields of an existing Fetal Presentation At Birth - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFetalPresentationAtBirth(byte id)
        {
            var model = _repository.FetalPresentationAtBirths.FirstOrDefault(rp => rp.FetalPresentationAtBirthId == id);

            if (model == null) return NotFound();

            return View("FetalPresentationAtBirth/EditFetalPresentationAtBirth", model);
        }

        ///<summary>
        ///     Edit fields of an existing Fetal Presentation At Birth in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFetalPresentationAtBirth(FetalPresentationAtBirth model)
        {
            _repository.EditFetalPresentationAtBirth(model);

            return RedirectToAction("ViewFetalPresentationAtBirths");
        }

        ///<summary>
        ///     Delete an existing Fetal Presentation At Birth - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFetalPresentationAtBirth(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.FetalPresentationAtBirths.FirstOrDefault(rp => rp.FetalPresentationAtBirthId == id);
            return View("FetalPresentationAtBirth/DeleteFetalPresentationAtBirth", model);
        }

        ///<summary>
        ///     Delete an existing Fetal Presentation At Birth in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFetalPresentationAtBirth(FetalPresentationAtBirth model)
        {
            try
            {
                _repository.DeleteFetalPresentationAtBirth(model);


                return RedirectToAction("ViewFetalPresentationAtBirths");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Fetal Presentation At Birth. Delete not available.";
                return View("FetalPresentationAtBirth/DeleteFetalPresentationAtBirth", model);
            }

        }

        #endregion

        #region Final Route and Method of Delivery
        /// <summary>
        ///     Index page of Final Routes And Method Of Deliveries
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewFinalRoutesAndMethodOfDeliveries()
        {
            List<FinalRouteAndMethodOfDelivery> finalRouteAndMethodOfDeliveries = _repository.FinalRouteAndMethodOfDeliveries
                                .ToList();
            return View("FinalRouteAndMethodOfDelivery/ViewFinalRoutesAndMethodOfDeliveries", finalRouteAndMethodOfDeliveries);
        }

        /// <summary>
        ///     Create New Final Route And Method Of Delivery - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFinalRouteAndMethodOfDelivery()
        {
            return View("FinalRouteAndMethodOfDelivery/CreateFinalRouteAndMethodOfDelivery");
        }

        ///<summary>
        ///     Add new Final Route And Method Of Delivery to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery model)
        {
            _repository.AddFinalRouteAndMethodOfDelivery(model);

            return RedirectToAction("ViewFinalRoutesAndMethodOfDeliveries");
        }

        ///<summary>
        ///     Edit fields of an existing Final Route And Method Of Delivery - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFinalRouteAndMethodOfDelivery(byte id)
        {
            var model = _repository.FinalRouteAndMethodOfDeliveries.FirstOrDefault(rp => rp.FinalRouteAndMethodId == id);

            if (model == null) return NotFound();

            return View("FinalRouteAndMethodOfDelivery/EditFinalRouteAndMethodOfDelivery", model);
        }

        ///<summary>
        ///     Edit fields of an existing Final Route And Method Of Delivery in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery model)
        {
            _repository.EditFinalRouteAndMethodOfDelivery(model);

            return RedirectToAction("ViewFinalRoutesAndMethodOfDeliveries");
        }

        ///<summary>
        ///     Delete an existing Final Route And Method Of Delivery - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFinalRouteAndMethodOfDelivery(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.FinalRouteAndMethodOfDeliveries.FirstOrDefault(rp => rp.FinalRouteAndMethodId == id);
            return View("FinalRouteAndMethodOfDelivery/DeleteFinalRouteAndMethodOfDelivery", model);
        }

        ///<summary>
        ///     Delete an Final Route And Method Of Delivery in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery model)
        {
            try
            {
                _repository.DeleteFinalRouteAndMethodOfDelivery(model);


                return RedirectToAction("ViewFinalRoutesAndMethodOfDeliveries");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Final Route And Method Of Delivery. Delete not available.";
                return View("FinalRouteAndMethodOfDelivery/DeleteFinalRouteAndMethodOfDelivery", model);
            }

        }

        #endregion

        #region Maternal Morbidity
        /// <summary>
        ///     Index page of Maternal Morbidities
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewMaternalMorbidities()
        {
            List<MaternalMorbidity> maternalMorbidities = _repository.MaternalMorbidities
                                .ToList();
            return View("MaternalMorbidity/ViewMaternalMorbidities", maternalMorbidities);
        }

        /// <summary>
        ///     Create New Maternal Morbidity - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateMaternalMorbidity()
        {
            return View("MaternalMorbidity/CreateMaternalMorbidity");
        }

        ///<summary>
        ///     Add new Maternal Morbidity to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateMaternalMorbidity(MaternalMorbidity model)
        {
            _repository.AddMaternalMorbidity(model);

            return RedirectToAction("ViewMaternalMorbidities");
        }

        ///<summary>
        ///     Edit fields of an existing Maternal Morbidity - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditMaternalMorbidity(byte id)
        {
            var model = _repository.MaternalMorbidities.FirstOrDefault(rp => rp.MaternalMorbidityId == id);

            if (model == null) return NotFound();

            return View("MaternalMorbidity/EditMaternalMorbidity", model);
        }

        ///<summary>
        ///     Edit fields of an existing Maternal Morbidity in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditMaternalMorbidity(MaternalMorbidity model)
        {
            _repository.EditMaternalMorbidity(model);

            return RedirectToAction("ViewMaternalMorbidities");
        }

        ///<summary>
        ///     Delete an existing Maternal Morbidity - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteMaternalMorbidity(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.MaternalMorbidities.FirstOrDefault(rp => rp.MaternalMorbidityId == id);
            return View("MaternalMorbidity/DeleteMaternalMorbidity", model);
        }

        ///<summary>
        ///     Delete a Maternal Morbidity in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteMaternalMorbidity(MaternalMorbidity model)
        {
            try
            {
                _repository.DeleteMaternalMorbidity(model);


                return RedirectToAction("ViewMaternalMorbidities");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Maternal Morbidity. Delete not available.";
                return View("MaternalMorbidity/DeleteMaternalMorbidity", model);
            }

        }

        #endregion

        #region Onset of Labor
        /// <summary>
        ///     Index page of Onset Of Labor
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewOnsetOfLabors()
        {
            List<OnsetOfLabor> onsetOfLabors = _repository.OnsetOfLabors
                                .ToList();
            return View("OnsetOfLabor/ViewOnsetOfLabors", onsetOfLabors);
        }

        /// <summary>
        ///     Create New Onset Of Labor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateOnsetOfLabor()
        {
            return View("OnsetOfLabor/CreateOnsetOfLabor");
        }

        ///<summary>
        ///     Add new Onset Of Labor to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateOnsetOfLabor(OnsetOfLabor model)
        {
            _repository.AddOnsetOfLabor(model);

            return RedirectToAction("ViewOnsetOfLabors");
        }

        ///<summary>
        ///     Edit fields of an existing Onset Of Labor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditOnsetOfLabor(byte id)
        {
            var model = _repository.OnsetOfLabors.FirstOrDefault(rp => rp.OnsetOfLaborId == id);

            if (model == null) return NotFound();

            return View("OnsetOfLabor/EditOnsetOfLabor", model);
        }

        ///<summary>
        ///     Edit fields of an existing Onset Of Labor in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditOnsetOfLabor(OnsetOfLabor model)
        {
            _repository.EditOnsetOfLabor(model);

            return RedirectToAction("ViewOnsetOfLabors");
        }

        ///<summary>
        ///     Delete an existing Onset Of Labor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteOnsetOfLabor(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.OnsetOfLabors.FirstOrDefault(rp => rp.OnsetOfLaborId == id);
            return View("OnsetOfLabor/DeleteOnsetOfLabor", model);
        }

        ///<summary>
        ///     Delete a Onset Of Labor in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteOnsetOfLabor(OnsetOfLabor model)
        {
            try
            {
                _repository.DeleteOnsetOfLabor(model);


                return RedirectToAction("ViewOnsetOfLabors");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Onset Of Labor. Delete not available.";
                return View("OnsetOfLabor/DeleteOnsetOfLabor", model);
            }

        }

        #endregion

        #region Pregnancy Infection
        /// <summary>
        ///     Index page of Pregnancy Infections
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPregnancyInfections()
        {
            List<PregnancyInfection> pregnancyInfections = _repository.PregnancyInfections
                                .ToList();
            return View("PregnancyInfection/ViewPregnancyInfections", pregnancyInfections);
        }

        /// <summary>
        ///     Create New Pregnancy Infection - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePregnancyInfection()
        {
            return View("PregnancyInfection/CreatePregnancyInfection");
        }

        ///<summary>
        ///     Add new Pregnancy Infection to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePregnancyInfection(PregnancyInfection model)
        {
            _repository.AddPregnancyInfection(model);

            return RedirectToAction("ViewPregnancyInfections");
        }

        ///<summary>
        ///     Edit fields of an existing Pregnancy Infection - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPregnancyInfection(byte id)
        {
            var model = _repository.PregnancyInfections.FirstOrDefault(rp => rp.InfectionId == id);

            if (model == null) return NotFound();

            return View("PregnancyInfection/EditPregnancyInfection", model);
        }

        ///<summary>
        ///     Edit fields of an existing Pregnancy Infection in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPregnancyInfection(PregnancyInfection model)
        {
            _repository.EditPregnancyInfection(model);

            return RedirectToAction("ViewPregnancyInfections");
        }

        ///<summary>
        ///     Delete an existing Pregnancy Infection - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePregnancyInfection(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.PregnancyInfections.FirstOrDefault(rp => rp.InfectionId == id);
            return View("PregnancyInfection/DeletePregnancyInfection", model);
        }

        ///<summary>
        ///     Delete a Pregnancy Infection in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePregnancyInfection(PregnancyInfection model)
        {
            try
            {
                _repository.DeletePregnancyInfection(model);


                return RedirectToAction("ViewPregnancyInfections");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Pregnancy Infection. Delete not available.";
                return View("PregnancyInfection/DeletePregnancyInfection", model);
            }

        }

        #endregion

        #region Pregnancy Risk Factor
        /// <summary>
        ///     Index page of Pregnancy Risk Factors
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPregnancyRiskFactors()
        {
            List<PregnancyRiskFactor> pregnancyRiskFactors = _repository.PregnancyRiskFactors
                                .ToList();
            return View("PregnancyRiskFactor/ViewPregnancyRiskFactors", pregnancyRiskFactors);
        }

        /// <summary>
        ///     Create New Pregnancy Risk Factor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePregnancyRiskFactor()
        {
            return View("PregnancyRiskFactor/CreatePregnancyRiskFactor");
        }

        ///<summary>
        ///     Add new Pregnancy Risk Factor to database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePregnancyRiskFactor(PregnancyRiskFactor model)
        {
            _repository.AddPregnancyRiskFactor(model);

            return RedirectToAction("ViewPregnancyRiskFactors");
        }

        ///<summary>
        ///     Edit fields of an existing Pregnancy Risk Factor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPregnancyRiskFactor(byte id) 
        {
            var model = _repository.PregnancyRiskFactors.FirstOrDefault(rp => rp.RiskFactorId == id);

            if (model == null) return NotFound();

            return View("PregnancyRiskFactor/EditPregnancyRiskFactor", model);
        }

        ///<summary>
        ///     Edit fields of an existing Pregnancy Risk Factor in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPregnancyRiskFactor(PregnancyRiskFactor model) 
        {
            _repository.EditPregnancyRiskFactor(model);

            return RedirectToAction("ViewPregnancyRiskFactors");
        }

        ///<summary>
        ///     Delete an existing Pregnancy Risk Factor - getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePregnancyRiskFactor(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            var model = _repository.PregnancyRiskFactors.FirstOrDefault(rp => rp.RiskFactorId == id);
            return View("PregnancyRiskFactor/DeletePregnancyRiskFactor", model);
        }

        ///<summary>
        ///     Delete a Pregnancy Risk Factor in the database - setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePregnancyRiskFactor(PregnancyRiskFactor model) 
        {
            try
            {
                _repository.DeletePregnancyRiskFactor(model);
                 

                return RedirectToAction("ViewPregnancyRiskFactors"); 
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Pregnancy Risk Factor. Delete not available.";
                return View("PregnancyRiskFactor/DeletePregnancyRiskFactor", model);                
            }

        }

        #endregion   
    }
}