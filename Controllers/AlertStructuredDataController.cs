using System;
using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using IS_Proj_HIT.ViewModels.AlertStructuredData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd", "StructuredDataView", "StructuredDataEdit", "StructuredDataDelete")]
    public class AlertStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public AlertStructuredDataController(IWCTCHealthSystemRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///   Index page of Alert Structured Data - Getter
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

        #region AlertTypes
        /// <summary>
        ///     Index page of Alert Types - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewAlertTypes()
        {
            List<AlertType> alertTypes = _repository.AlertTypes
                .OrderBy(a => a.Name)
                .ToList();
            return View("AlertType/ViewAlertTypes", alertTypes);
        }

        /// <summary>
        ///     Create New Alert Type - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAlertType()
        {
            return View("AlertType/CreateAlertType");
        }

        ///<summary>
        ///     Add new AlertType to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAlertType(AlertStructuredDataViewModel model)
        {
            _repository.AddAlertType(model.AlertType);

            return RedirectToAction("ViewAlertTypes");
        }

        ///<summary>
        ///     Edit fields of an existing AlertType - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAlertType(int id)
        {
            AlertType model = _repository.AlertTypes.FirstOrDefault(rp => rp.AlertId == id);
            return View("AlertType/EditAlertType", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing AlertType in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAlertType(AlertStructuredDataViewModel model)
        {
            model.AlertType.LastModified = DateTime.Now;
            _repository.EditAlertType(model.AlertType);

            return RedirectToAction("ViewAlertTypes");
        }

        ///<summary>
        ///     Delete an existing AlertType - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAlertType(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            AlertType model = _repository.AlertTypes.FirstOrDefault(rp => rp.AlertId == id);
            return View("AlertType/DeleteAlertType", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing AlertType in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAlertType(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteAlertType(model.AlertType);
                return RedirectToAction("ViewAlertTypes");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Alert Type. Delete not available.";
                return View("AlertType/DeleteAlertType", model);
            }

        }
        #endregion

        #region AdvancedDirectives
        /// <summary>
        ///     Index page of Advanced Directive - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewAdvancedDirectives()
        {
            List<AdvancedDirective> advancedDirectives = _repository.AdvancedDirectives
                .OrderBy(a => a.Name)
                .ToList();
            return View("AdvancedDirective/ViewAdvancedDirectives", advancedDirectives);
        }

        /// <summary>
        ///     Create New Advanced Directive - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAdvancedDirective()
        {
            return View("AdvancedDirective/CreateAdvancedDirective");
        }

        ///<summary>
        ///     Add new Advanced Directive to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAdvancedDirective(AlertStructuredDataViewModel model)
        {
            _repository.AddAdvancedDirective(model.AdvancedDirective);

            return RedirectToAction("ViewAdvancedDirectives");
        }

        ///<summary>
        ///     Edit fields of an existing AdvancedDirective - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAdvancedDirective(short id)
        {
            AdvancedDirective model = _repository.AdvancedDirectives.FirstOrDefault(rp => rp.AdvancedDirectiveId == id);
            return View("AdvancedDirective/EditAdvancedDirective", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing Advanced Directive in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAdvancedDirective(AlertStructuredDataViewModel model)
        {
            _repository.EditAdvancedDirective(model.AdvancedDirective);

            return RedirectToAction("ViewAdvancedDirectives");
        }

        ///<summary>
        ///     Delete an existing Advanced Directive - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAdvancedDirective(short id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            AdvancedDirective model = _repository.AdvancedDirectives.FirstOrDefault(rp => rp.AdvancedDirectiveId == id);
            return View("AdvancedDirective/DeleteAdvancedDirective", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing Advanced Directive in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAdvancedDirective(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteAdvancedDirective(model.AdvancedDirective);
                return RedirectToAction("ViewAdvancedDirectives");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Advanced Directive. Delete not available.";
                return View("AdvancedDirective/DeleteAdvancedDirective", model);
            }

        }
        #endregion

        #region Allergens
        /// <summary>
        ///     Index page of Allergens - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewAllergens()
        {
            List<Allergen> allergens = _repository.Allergens
                .OrderBy(a => a.AllergenName)
                .ToList();
            return View("Allergen/ViewAllergens", allergens);
        }

        /// <summary>
        ///     Create New Allergen - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAllergen()
        {
            return View("Allergen/CreateAllergen");
        }

        ///<summary>
        ///     Add new Allergen to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateAllergen(AlertStructuredDataViewModel model)
        {
            _repository.AddAllergen(model.Allergen);

            return RedirectToAction("ViewAllergens");
        }

        ///<summary>
        ///     Edit fields of an existing Allergen - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAllergen(int id)
        {
            Allergen model = _repository.Allergens.FirstOrDefault(rp => rp.AllergenId == id);
            return View("Allergen/EditAllergen", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing Allergen in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditAllergen(AlertStructuredDataViewModel model)
        {
            model.Allergen.LastModified = DateTime.Now;
            _repository.EditAllergen(model.Allergen);

            return RedirectToAction("ViewAllergens");
        }

        ///<summary>
        ///     Delete an existing Allergen - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAllergen(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Allergen model = _repository.Allergens.FirstOrDefault(rp => rp.AllergenId == id);
            return View("Allergen/DeleteAllergen", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing Allergen in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteAllergen(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteAllergen(model.Allergen);
                return RedirectToAction("ViewAllergens");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Allergen. Delete not available.";
                return View("Allergen/DeleteAllergen", model);
            }

        }
        #endregion

        #region ClinicalReminders
        /// <summary>
        ///     Index page of Clinical Reminders - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewClinicalReminders()
        {
            List<ClinicalReminder> clinicalReminders = _repository.ClinicalReminders
                .OrderBy(a => a.Name)
                .ToList();
            return View("ClinicalReminder/ViewClinicalReminders", clinicalReminders);
        }

        /// <summary>
        ///     Create New Clinical Reminder - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateClinicalReminder()
        {
            return View("ClinicalReminder/CreateClinicalReminder");
        }

        ///<summary>
        ///     Add new Clinical Reminder to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateClinicalReminder(AlertStructuredDataViewModel model)
        {
            _repository.AddClinicalReminder(model.ClinicalReminder);

            return RedirectToAction("ViewClinicalReminders");
        }

        ///<summary>
        ///     Edit fields of an existing Clinical Reminder - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditClinicalReminder(short id)
        {
            ClinicalReminder model = _repository.ClinicalReminders.FirstOrDefault(rp => rp.ClinicalReminderId == id);
            return View("ClinicalReminder/EditClinicalReminder", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing Clinical Reminder in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditClinicalReminder(AlertStructuredDataViewModel model)
        {
            _repository.EditClinicalReminder(model.ClinicalReminder);

            return RedirectToAction("ViewClinicalReminders");
        }

        ///<summary>
        ///     Delete an existing Clinical Reminder - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteClinicalReminder(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            ClinicalReminder model = _repository.ClinicalReminders.FirstOrDefault(rp => rp.ClinicalReminderId == id);
            return View("ClinicalReminder/DeleteClinicalReminder", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing Clinical Reminder in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteClinicalReminder(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteClinicalReminder(model.ClinicalReminder);
                return RedirectToAction("ViewClinicalReminders");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Clinical Reminder. Delete not available.";
                return View("ClinicalReminder/DeleteClinicalReminder", model);
            }

        }
        #endregion

        #region FallRisks
        /// <summary>
        ///     Index page of Fall Risks - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewFallRisks()
        {
            List<FallRisk> fallRisks = _repository.FallRisks
                .OrderBy(a => a.Name)
                .ToList();
            return View("FallRisk/ViewFallRisks", fallRisks);
        }

        /// <summary>
        ///     Create New Fall Risk - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFallRisk()
        {
            return View("FallRisk/CreateFallRisk");
        }

        ///<summary>
        ///     Add new Fall Risk to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFallRisk(AlertStructuredDataViewModel model)
        {
            _repository.AddFallRisk(model.FallRisk);

            return RedirectToAction("ViewFallRisks");
        }

        ///<summary>
        ///     Edit fields of an existing Fall Risk - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFallRisk(byte id)
        {
            FallRisk model = _repository.FallRisks.FirstOrDefault(rp => rp.FallRiskId == id);
            return View("FallRisk/EditFallRisk", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing Fall Risk in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFallRisk(AlertStructuredDataViewModel model)
        {
            _repository.EditFallRisk(model.FallRisk);

            return RedirectToAction("ViewFallRisks");
        }

        ///<summary>
        ///     Delete an existing Fall Risk - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFallRisk(byte id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            FallRisk model = _repository.FallRisks.FirstOrDefault(rp => rp.FallRiskId == id);
            return View("FallRisk/DeleteFallRisk", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing Fall Risk in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFallRisk(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteFallRisk(model.FallRisk);
                return RedirectToAction("ViewFallRisks");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Fall Risk. Delete not available.";
                return View("FallRisk/DeleteFallRisk", model);
            }

        }
        #endregion

        #region Reaction
        /// <summary>
        ///     Index page of Reactions - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewReactions()
        {
            List<Reaction> reactions = _repository.Reactions
                .OrderBy(a => a.Name)
                .ToList();
            return View("Reaction/ViewReactions", reactions);
        }

        /// <summary>
        ///     Create New Reaction - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateReaction()
        {
            return View("Reaction/CreateReaction");
        }

        ///<summary>
        ///     Add new Reaction to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateReaction(AlertStructuredDataViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = new Reaction
            {
                Name = model.Reaction.Name,
                Description = model.Reaction.Description,
                AlertRequired = model.Reaction.AlertRequired.GetValueOrDefault(), // safe conversion
                LastModified = DateTime.Now
            };

            _repository.AddReaction(entity);

            return RedirectToAction("ViewReactions");
        }

        ///<summary>
        ///     Edit fields of an existing Reaction - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditReaction(int id)
        {
            Reaction model = _repository.Reactions.FirstOrDefault(rp => rp.ReactionId == id);
            return View("Reaction/EditReaction", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing Reaction in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditReaction(AlertStructuredDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Reaction rm = _repository.Reactions.FirstOrDefault(rp => rp.ReactionId == model.Reaction.ReactionId);
                return View("Reaction/EditReaction", new AlertStructuredDataViewModel(rm));
            }

            model.Reaction.LastModified = DateTime.Now;
            _repository.EditReaction(model.Reaction);

            return RedirectToAction("ViewReactions");
        }

        ///<summary>
        ///     Delete an existing Reaction - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteReaction(int id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Reaction model = _repository.Reactions.FirstOrDefault(rp => rp.ReactionId == id);
            return View("Reaction/DeleteReaction", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing Reaction in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteReaction(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteReaction(model.Reaction);
                return RedirectToAction("ViewReactions");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Reaction. Delete not available.";
                return View("Reaction/DeleteReaction", model);
            }

        }
        #endregion

        #region Restrictions
        /// <summary>
        ///     Index page of Restrictions - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRestrictions()
        {
            List<Restriction> restrictions = _repository.Restrictions
                .OrderBy(a => a.Name)
                .ToList();
            return View("Restriction/ViewRestrictions", restrictions);
        }

        /// <summary>
        ///     Create New Restriction - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRestriction()
        {
            return View("Restriction/CreateRestriction");
        }

        ///<summary>
        ///     Add new Restriction to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRestriction(AlertStructuredDataViewModel model)
        {
            _repository.AddRestriction(model.Restriction);

            return RedirectToAction("ViewRestrictions");
        }

        ///<summary>
        ///     Edit fields of an existing Restriction - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRestriction(short id)
        {
            Restriction model = _repository.Restrictions.FirstOrDefault(rp => rp.RestrictionId == id);
            return View("Restriction/EditRestriction", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Edit fields of an existing Restriction in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRestriction(AlertStructuredDataViewModel model)
        {
            _repository.EditRestriction(model.Restriction);

            return RedirectToAction("ViewRestrictions");
        }

        ///<summary>
        ///     Delete an existing Restriction - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRestriction(short id)
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Restriction model = _repository.Restrictions.FirstOrDefault(rp => rp.RestrictionId == id);
            return View("Restriction/DeleteRestriction", new AlertStructuredDataViewModel(model));
        }

        ///<summary>
        ///     Delete an existing Restriction in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRestriction(AlertStructuredDataViewModel model)
        {
            try
            {
                _repository.DeleteRestriction(model.Restriction);
                return RedirectToAction("ViewRestrictions");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Restriction. Delete not available.";
                return View("Restriction/DeleteRestriction", model);
            }

        }
        #endregion
    }
}