using IS_Proj_HIT.ViewModels.MedicationDataEntry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class MedicationDataEntryController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly IConfiguration _configuration;

        public MedicationDataEntryController(IWCTCHealthSystemRepository repository, IConfiguration configuration) {
             _repository = repository;
             _configuration = configuration;
        }

        /// <summary>
        /// Index page of MedicationDataEntry
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Used in the CreateMedication and ViewMedications Views to link to an external NDC website
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult NDCRedirect()
        {
            return Redirect(_configuration.GetSection("NDCRedirect").GetChildren().FirstOrDefault(config => config.Key == "Url").Value);
        }

        #region Medications
        /// <summary>
        /// Create New Medication
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateMedication() {
            PopulateMedicationViewbag();
            return View();
        }

        /// <summary>
        /// Adds New Medication to database and if given unique BrandNames and GenericNames, create and add those to the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateMedication(MedicationViewModel model) {
            // Check for duplicate NDC, if there is a medication with the ndc, prevent the database from adding duplicates
            if (_repository.Medications.FirstOrDefault(med => med.Ndc == model.Medication.Ndc) != null) {
                // If a medication exists with the ndc, add error message and data to viewbag and return the view
                ViewBag.ErrorMessage = "A medication with that NDC already exists";
                PopulateMedicationViewbag();
                return View(model);
            }

            MedicationBrandName exsistingBrandName = _repository.MedicationBrandNames.FirstOrDefault(bn => bn.BrandName == model.BrandName.BrandName);
            MedicationGenericName exsistingGenericName = _repository.MedicationGenericNames.FirstOrDefault(gn => gn.GenericName == model.GenericName.GenericName);

            // Check if MedicationBrandName is already in database
            if (exsistingBrandName != null) {
                // If it is, use its id
                model.Medication.MedicationBrandName = exsistingBrandName;
            } else {
                // If it isn't, add the new MedicationBrandName to the database
                model.BrandName.IsActive = true;
                model.BrandName.ModifiedDate = DateTime.Now;

                _repository.AddMedicationBrandName(model.BrandName);

                model.Medication.MedicationBrandName = model.BrandName;
            }

            // Check if MedicationGenericName is already in database
            if (exsistingGenericName != null) {
                // If it is, use its id
                model.Medication.MedicationGenericName = exsistingGenericName;
            } else {
                // If it isn't, add the new MedicationGenericName to the database
                model.GenericName.IsActive = true;
                model.GenericName.ModifiedDate = DateTime.Now;

                _repository.AddMedicationGenericName(model.GenericName);

                model.Medication.MedicationGenericName = model.GenericName;
            }

            model.Medication.IsActive = true;
            model.Medication.ModifiedDate = DateTime.Now;
            _repository.AddMedication(model.Medication);

            return RedirectToAction("ViewMedications");
        }

        /// <summary>
        /// Edit fields of an exsisting Medication
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditMedication(int id) {
            PopulateMedicationViewbag();

            Medication model = _repository.Medications
                                            .Include(med => med.MedicationDeliveryRoute)
                                            .Include(med => med.MedicationBrandName)
                                            .Include(med => med.MedicationDosageForm)
                                            .Include(med => med.MedicationGenericName)
                                            .AsSplitQuery()
                                            .First(med => med.MedicationId == id);
            
            return View(new MedicationViewModel(model));
        }

        /// <summary>
        /// Edit fields of an exsisting Medication and saves changes to database
        /// If given unique BrandNames and GenericNames, create and add those to the database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditMedication(MedicationViewModel model) {
            // Check for duplicate NDC, if there is a medication with the ndc, that is not the current medication, prevent the database from adding duplicates
            if (_repository.Medications.FirstOrDefault(med => med.Ndc == model.Medication.Ndc && med.MedicationId != model.Medication.MedicationId) != null) {
                // If a medication exists with the ndc, add error message and data to viewbag and return the view
                ViewBag.ErrorMessage = "A medication with that NDC already exists";
                PopulateMedicationViewbag();
                return View(model);
            }

            MedicationBrandName exsistingBrandName = _repository.MedicationBrandNames.FirstOrDefault(bn => bn.BrandName == model.BrandName.BrandName);
            MedicationGenericName exsistingGenericName = _repository.MedicationGenericNames.FirstOrDefault(gn => gn.GenericName == model.GenericName.GenericName);

            // Check if MedicationBrandName is already in database
            if (exsistingBrandName != null) {
                // If it is, use its id
                model.Medication.MedicationBrandName = exsistingBrandName;
            } else {
                // If it isn't, add the new MedicationBrandName to the database
                model.BrandName.IsActive = true;
                model.BrandName.ModifiedDate = @DateTime.Now;

                _repository.AddMedicationBrandName(model.BrandName);

                model.Medication.MedicationBrandName = model.BrandName;
            }

            // Check if MedicationGenericName is already in database
            if (exsistingGenericName != null) {
                // If it is, use its id
                model.Medication.MedicationGenericName = exsistingGenericName;
            } else {
                // If it isn't, add the new MedicationGenericName to the database
                model.GenericName.IsActive = true;
                model.GenericName.ModifiedDate = DateTime.Now;

                _repository.AddMedicationGenericName(model.GenericName);

                model.Medication.MedicationGenericName = model.GenericName;
            }

            model.Medication.ModifiedDate = DateTime.Now;
            _repository.EditMedication(model.Medication);

            return RedirectToAction("ViewMedications");
        }

        /// <summary>
        /// View details of a medication
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult MedicationDetails(int id) {
            Medication model = _repository.Medications
                                            .Include(med => med.MedicationDeliveryRoute)
                                            .Include(med => med.MedicationBrandName)
                                            .Include(med => med.MedicationDosageForm)
                                            .Include(med => med.MedicationGenericName)
                                            .AsSplitQuery()
                                            .First(med => med.MedicationId == id);
            
            return View(model);
        }

        /// <summary>
        /// Delete Medication
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteMedication(int id) {
            Medication model = _repository.Medications
                                            .Include(med => med.MedicationDeliveryRoute)
                                            .Include(med => med.MedicationBrandName)
                                            .Include(med => med.MedicationDosageForm)
                                            .Include(med => med.MedicationGenericName)
                                            .AsSplitQuery()
                                            .First(med => med.MedicationId == id);
            
            return View(new MedicationViewModel(model));
        }

        /// <summary>
        /// Delete Medication if there is not a patient who has been perscribed the medication.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteMedication(MedicationViewModel model)
        {
            // See if any Patient records exist with this medication
            bool medicationInUse = _repository.PatientMedicationLists.Any(med => med.MedicationID == model.Medication.MedicationId);
            if (medicationInUse)
            {
                ViewBag.ErrorMessage = "Patient records exist using this medication. Delete not available.";
                return View(model);
            }

            _repository.DeleteMedication(model.Medication);

            return RedirectToAction("ViewMedications");
        }

        /// <summary>
        /// Displays a homepage with a table that contains all medications.
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewMedications(int pageNumber = 1) {
            MedicationListViewModel medications = new(_repository.MedicationListPages(pageNumber, 20)
                .Include(m => m.MedicationBrandName)
                .Include(m => m.MedicationGenericName)
                .Include(m => m.MedicationDosageForm)
                .ToList());

            medications.PageNumber = pageNumber;
            AddPagination(20);

            return View(medications);
        }

        /// <summary>
        /// Runs when the user searches through the medication table in ViewMedications
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewMedications(MedicationListViewModel model, int? pageNumber) {
            MedicationListViewModel medications = new()
            {
                // If model.PageNumber is greater then 1, the user is going to the next page and not searching
                PageNumber = (int)(pageNumber != null ? pageNumber : 1),
                BrandSearch = model.BrandSearch,
                GenericSearch = model.GenericSearch
            };

            // Conditionals for searching
            if (string.IsNullOrEmpty(model.BrandSearch) && string.IsNullOrEmpty(model.GenericSearch)){
                AddPagination(20);
                medications.Medications = _repository.MedicationListPages(medications.PageNumber, 20)
                    .Include(m => m.MedicationBrandName)
                    .Include(m => m.MedicationGenericName)
                    .Include(m => m.MedicationDosageForm)
                    .ToList();
            } else if (!string.IsNullOrEmpty(model.BrandSearch) && string.IsNullOrEmpty(model.GenericSearch)) {
                medications.Medications = _repository.Medications
                    .Include(m => m.MedicationBrandName)
                    .Include(m => m.MedicationGenericName)
                    .Include(m => m.MedicationDosageForm)
                    .Where(m => m.MedicationBrandName.BrandName.ToLower().Contains(model.BrandSearch.ToLower()))
                    .ToList();
            } else if (string.IsNullOrEmpty(model.BrandSearch) && !string.IsNullOrEmpty(model.GenericSearch)) {
                medications.Medications = _repository.Medications
                    .Include(m => m.MedicationBrandName)
                    .Include(m => m.MedicationGenericName)
                    .Include(m => m.MedicationDosageForm)
                    .Where(m => m.MedicationGenericName.GenericName.ToLower().Contains(model.GenericSearch.ToLower()))
                    .ToList();
            } else if (!string.IsNullOrEmpty(model.BrandSearch) && !string.IsNullOrEmpty(model.GenericSearch)) {
                medications.Medications = _repository.Medications
                    .Include(m => m.MedicationBrandName)
                    .Include(m => m.MedicationGenericName)
                    .Include(m => m.MedicationDosageForm)
                    .Where(m => m.MedicationBrandName.BrandName.ToLower().Contains(model.BrandSearch.ToLower()) && 
                    m.MedicationGenericName.GenericName.ToLower().Contains(model.GenericSearch.ToLower()))
                    .ToList();
            }

            return View(medications);
        }
        #endregion  // end of Medications section

        #region Dosage
        /// <summary>
        /// Index page of Medication Dosage Forms
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDosageForms() {
            List<MedicationDosageForm> dosageForms = _repository.MedicationDosageForms.ToList();
            return View(dosageForms);
        }

        /// <summary>
        /// Create New Dosage Form
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDosageForm() {
            return View();
        }

        /// <summary>
        /// Adds New Dosage Form to database if given a unique Dosage Form
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDosageForm(MedicationDosageForm newDosageForm) {
            // Check for duplicate, if there is the Dosage Form already, prevent the database from adding it
            if (_repository.MedicationDosageForms.FirstOrDefault(d => d.DosageForm == newDosageForm.DosageForm) != null) {
                // If the Dosage Form Already exists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Dosage Form already exists";
                return View(newDosageForm);
            }

            newDosageForm.IsActive = true;
            newDosageForm.DateModified = DateTime.Now;
            _repository.AddMedicationDosageForm(newDosageForm);

            return RedirectToAction("ViewDosageForms");
        }

        /// <summary>
        /// Edit fields on exsisting Dosage Form
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDosageForm(int id) {
            MedicationDosageForm dosageForm = _repository.MedicationDosageForms.First(d => d.DosageFormId == id);
            return View(dosageForm);
        }

        /// <summary>
        /// Edit fields of an exsisting Dosage Form and saves changes to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDosageForm(MedicationDosageForm model) {
            // Check for duplicate Dosage Form, if there is a duplicate that is not the current Dosage Form, prevent the database from adding it
            if (_repository.MedicationDosageForms.FirstOrDefault(d => d.DosageForm == model.DosageForm && d.DosageFormId != model.DosageFormId) != null) {
                // If the dosage form already exsists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Dosage Form already exists";
                return View(model);
            }

            model.DateModified = DateTime.Now;
            _repository.EditMedicationDosageForm(model);

            return RedirectToAction("ViewDosageForms");
        }

        /// <summary>
        /// Delete Dosage Form
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDosageForm(int id) {
            MedicationDosageForm dosageForm = _repository.MedicationDosageForms.First(d => d.DosageFormId == id);
            
            return View(dosageForm);
        }

        /// <summary>
        /// Delete Dosage Form if there is not a medication with the dosage form.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDosageForm(MedicationDosageForm model)
        {
            // See if any Medications exist with this dosage form
            bool inUse = _repository.MedicationDosageForms
                            .Include(d => d.Medications)
                            .AsSplitQuery()
                            .FirstOrDefault(d => d.Medications.Count != 0 && d.DosageFormId == model.DosageFormId) != null;
            if (inUse)
            {
                ViewBag.ErrorMessage = "Medications exist using this dosage form. Delete not available.";
                return View(model);
            }

            _repository.DeleteMedicationDosageForm(model);

            return RedirectToAction("ViewDosageForms");
        }
        #endregion  // end of Dosage section

        #region Delivery Routes
        /// <summary>
        /// Index page of Medication Delivery Routes
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewDeliveryRoutes() {
            List<MedicationDeliveryRoute> deliveryRoutes = _repository.MedicationDeliveryRoutes.ToList();
            return View(deliveryRoutes);
        }

        /// <summary>
        /// Create New Delivery Route
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDeliveryRoute() {
            return View();
        }

        /// <summary>
        /// Adds New Delivery Route to database if given a unique Delivery Route
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateDeliveryRoute(MedicationDeliveryRoute newDeliveryRoute) {
            // Check for duplicate, if there is the Delivery Route already, prevent the database from adding it
            if (_repository.MedicationDeliveryRoutes.FirstOrDefault(d => d.DeliveryRouteName == newDeliveryRoute.DeliveryRouteName) != null) {
                // If the Delivery Route Already exists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Delivery Route already exists";
                return View(newDeliveryRoute);
            }

            newDeliveryRoute.IsActive = true;
            newDeliveryRoute.ModifiedDate = DateTime.Now;
            _repository.AddMedicationDeliveryRoute(newDeliveryRoute);

            return RedirectToAction("ViewDeliveryRoutes");
        }

        /// <summary>
        /// Edit fields on exsisting Delivery Route
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDeliveryRoute(int id) {
            MedicationDeliveryRoute deliveryRoute = _repository.MedicationDeliveryRoutes.First(d => d.DeliveryRouteId == id);
            return View(deliveryRoute);
        }

        /// <summary>
        /// Edit fields of an exsisting Delivery Route and saves changes to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditDeliveryRoute(MedicationDeliveryRoute model) {
            // Check for duplicate Delivery Route, if there is a duplicate that is not the current Delivery Route, prevent the database from adding it
            if (_repository.MedicationDeliveryRoutes.FirstOrDefault(d => d.DeliveryRouteName == model.DeliveryRouteName && d.DeliveryRouteId != model.DeliveryRouteId) != null) {
                // If the Delivery Route already exsists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Delivery Route already exists";
                return View(model);
            }

            model.ModifiedDate = DateTime.Now;
            _repository.EditMedicationDeliveryRoute(model);

            return RedirectToAction("ViewDeliveryRoutes");
        }

        /// <summary>
        /// Delete Delivery Route
        /// </summary>
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteDeliveryRoute(int id) {
            MedicationDeliveryRoute deliveryRoute = _repository.MedicationDeliveryRoutes.First(d => d.DeliveryRouteId == id);
            
            return View(deliveryRoute);
        }

        /// <summary>
        /// Delete Delivery Route if there is not a medication with the Delivery Route.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteDeliveryRoute(MedicationDeliveryRoute model)
        {
            // See if any Medications exist with this Delivery Route
            bool inUse = _repository.MedicationDeliveryRoutes
                            .Include(d => d.Medications)
                            .AsSplitQuery()
                            .FirstOrDefault(d => d.Medications.Count != 0 && d.DeliveryRouteId == model.DeliveryRouteId) != null;
            if (inUse)
            {
                ViewBag.ErrorMessage = "Medications exist using this Delivery Route. Delete not available.";
                return View(model);
            }

            _repository.DeleteMedicationDeliveryRoute(model);

            return RedirectToAction("ViewDeliveryRoutes");
        }
        #endregion  // end of Delivery Routes section

        #region Generic Names
        /// <summary>
        /// Index page of Medication Generic Names
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewGenericNames() {
            List<MedicationGenericName> genericNames = _repository.MedicationGenericNames.ToList();
            return View(genericNames);
        }

        /// <summary>
        /// Create New Generic Name
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateGenericName() {
            return View();
        }

        /// <summary>
        /// Adds New Generic Name to database if given a unique Generic Name
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateGenericName(MedicationGenericName newGenericName) {
            // Check for duplicate, if there is the Generic Name already, prevent the database from adding it
            if (_repository.MedicationGenericNames.FirstOrDefault(d => d.GenericName == newGenericName.GenericName) != null) {
                // If the Generic Name Already exists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Generic Name already exists";
                return View(newGenericName);
            }

            newGenericName.IsActive = true;
            newGenericName.ModifiedDate = DateTime.Now;
            _repository.AddMedicationGenericName(newGenericName);

            return RedirectToAction("ViewGenericNames");
        }

        /// <summary>
        /// Edit fields on exsisting Generic Name
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditGenericName(int id) {
            MedicationGenericName genericName = _repository.MedicationGenericNames.First(d => d.MedicationGenericId == id);
            return View(genericName);
        }

        /// <summary>
        /// Edit fields of an exsisting Generic Name and saves changes to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditGenericName(MedicationGenericName model) {
            // Check for duplicate Generic Name, if there is a duplicate that is not the current Generic Name, prevent the database from adding it
            if (_repository.MedicationGenericNames.FirstOrDefault(d => d.GenericName == model.GenericName && d.MedicationGenericId != model.MedicationGenericId) != null) {
                // If the Generic Name already exsists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Generic Name already exists";
                return View(model);
            }

            model.ModifiedDate = DateTime.Now;
            _repository.EditMedicationGenericName(model);

            return RedirectToAction("ViewGenericNames");
        }

        /// <summary>
        /// Delete Generic Name
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteGenericName(int id) {
            MedicationGenericName genericName = _repository.MedicationGenericNames.First(d => d.MedicationGenericId == id);
            
            return View(genericName);
        }

        /// <summary>
        /// Delete Generic Name if there is not a medication with the Generic Name.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteGenericName(MedicationGenericName model)
        {
            // See if any Medications exist with this Generic Name
            bool inUse = _repository.MedicationGenericNames
                            .Include(d => d.Medications)
                            .AsSplitQuery()
                            .FirstOrDefault(d => d.Medications.Count != 0 && d.MedicationGenericId == model.MedicationGenericId) != null;
            if (inUse)
            {
                ViewBag.ErrorMessage = "Medications exist using this Generic Name. Delete not available.";
                return View(model);
            }

            _repository.DeleteMedicationGenericName(model);

            return RedirectToAction("ViewGenericNames");
        }
        #endregion  // end of Generic Names section

        #region Brand Names
        /// <summary>
        /// Index page of Medication Brand Names
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewBrandNames() {
            List<MedicationBrandName> brandNames = _repository.MedicationBrandNames.ToList();
            return View(brandNames);
        }

        /// <summary>
        /// Create New Brand Name
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateBrandName() {
            return View();
        }

        /// <summary>
        /// Adds New Brand Name to database if given a unique Brand Name
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateBrandName(MedicationBrandName newBrandName) {
            // Check for duplicate, if there is the Brand Name already, prevent the database from adding it
            if (_repository.MedicationBrandNames.FirstOrDefault(d => d.BrandName == newBrandName.BrandName) != null) {
                // If the Brand Name Already exists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Brand Name already exists";
                return View(newBrandName);
            }

            newBrandName.IsActive = true;
            newBrandName.ModifiedDate = DateTime.Now;
            _repository.AddMedicationBrandName(newBrandName);

            return RedirectToAction("ViewBrandNames");
        }

        /// <summary>
        /// Edit fields on exsisting Brand Name
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBrandName(int id) {
            MedicationBrandName brandName = _repository.MedicationBrandNames.First(d => d.MedicationBrandId == id);
            return View(brandName);
        }

        /// <summary>
        /// Edit fields of an exsisting Brand Name and saves changes to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditBrandName(MedicationBrandName model) {
            // Check for duplicate Brand Name, if there is a duplicate that is not the current Brand Name, prevent the database from adding it
            if (_repository.MedicationBrandNames.FirstOrDefault(d => d.BrandName == model.BrandName && d.MedicationBrandId != model.MedicationBrandId) != null) {
                // If the Brand Name already exsists, add error message to viewbag and return the view
                ViewBag.ErrorMessage = "That Brand Name already exists";
                return View(model);
            }

            model.ModifiedDate = DateTime.Now;
            _repository.EditMedicationBrandName(model);

            return RedirectToAction("ViewBrandNames");
        }

        /// <summary>
        /// Delete Brand Name
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteBrandName(int id) {
            MedicationBrandName BrandName = _repository.MedicationBrandNames.First(d => d.MedicationBrandId == id);
            
            return View(BrandName);
        }

        /// <summary>
        /// Delete Brand Name if there is not a medication with the Brand Name.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteBrandName(MedicationBrandName model)
        {
            // See if any Medications exist with this Brand Name
            bool inUse = _repository.MedicationBrandNames
                            .Include(d => d.Medications)
                            .AsSplitQuery()
                            .FirstOrDefault(d => d.Medications.Count != 0 && d.MedicationBrandId == model.MedicationBrandId) != null;
            if (inUse)
            {
                ViewBag.ErrorMessage = "Medications exist using this Brand Name. Delete not available.";
                return View(model);
            }

            _repository.DeleteMedicationBrandName(model);

            return RedirectToAction("ViewBrandNames");
        }
        #endregion  // end of Brand Names section

        #region Frequencies
        /// <summary>
        /// Index page of Medication Frequencies
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewFrequencies() {
            List<MedicationFrequency> frequencies = _repository.MedicationFrequencies.ToList();
            return View(frequencies);
        }

        /// <summary>
        /// Create New Frequency
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFrequency() {
            return View();
        }

        /// <summary>
        /// Adds New Frequency to database if given a unique Frequency Description and Code
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateFrequency(MedicationFrequency newFrequency) {
            // Check for duplicate description or code, if there is a Frequency with that description or code already, prevent the database from adding it
            bool duplicateDescription = _repository.MedicationFrequencies.FirstOrDefault(d => d.FrequencyDescription.ToLower() == newFrequency.FrequencyDescription.ToLower()) != null;
            bool duplicateCode = _repository.MedicationFrequencies.FirstOrDefault(d => d.FrequencyCode.ToLower() == newFrequency.FrequencyCode.ToLower()) != null;

            if (duplicateDescription || duplicateCode) {
                // If the Frequency Already exists, check for type of duplicate and then add error message to viewbag and return the view
                if (duplicateDescription) {
                    ViewBag.ErrorMessage += "That Frequency already exists. ";
                }
                // Check independantly in case both are duplicates 
                if (duplicateCode) {
                    ViewBag.ErrorMessage += "A Frequency with that code already exists. ";
                }
                return View(newFrequency);
            }

            newFrequency.LastUpdate = DateTime.Now;
            _repository.AddMedicationFrequency(newFrequency);

            return RedirectToAction("ViewFrequencies");
        }

        /// <summary>
        /// Edit fields on exsisting Frequency
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFrequency(int id) {
            MedicationFrequency Frequency = _repository.MedicationFrequencies.First(d => d.MedicationFrequencyId == id);
            return View(Frequency);
        }

        /// <summary>
        /// Edit fields of an exsisting Frequency and saves changes to database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditFrequency(MedicationFrequency model) {
            // Check for duplicate description or code, if there is a Frequency with that description or code already that isn't the current Frequency, throw error and return view
            bool duplicateDescription = _repository.MedicationFrequencies.FirstOrDefault(d => (d.FrequencyDescription.ToLower() == model.FrequencyDescription.ToLower()) && (d.MedicationFrequencyId != model.MedicationFrequencyId)) != null;
            bool duplicateCode = duplicateCode = _repository.MedicationFrequencies.FirstOrDefault(d => (d.FrequencyCode.ToLower() == model.FrequencyCode.ToLower()) && (d.MedicationFrequencyId != model.MedicationFrequencyId)) != null;

            if (duplicateDescription || duplicateCode) {
                // If the Frequency Already exists, check for type of duplicate and then add error message to viewbag and return the view
                if (duplicateDescription) {
                    ViewBag.ErrorMessage = "That Frequency already exists. ";
                } 
                // Check independantly in case both are duplicates 
                if (duplicateCode) {
                    ViewBag.ErrorMessage += "A Frequency with that code already exists. ";
                }
                return View(model);
            }

            model.LastUpdate = DateTime.Now;
            _repository.EditMedicationFrequency(model);

            return RedirectToAction("ViewFrequencies");
        }

        /// <summary>
        /// Delete Frequency
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFrequency(int id) {
            MedicationFrequency Frequency = _repository.MedicationFrequencies.First(d => d.MedicationFrequencyId == id);
            
            return View(Frequency);
        }

        /// <summary>
        /// Delete Frequency if there is not a medication with the Frequency.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteFrequency(MedicationFrequency model)
        {
            // See if any Medications exist with this Frequency
            bool inUse = _repository.MedicationFrequencies
                            .Include(d => d.PatientMedicationLists)
                            .AsSplitQuery()
                            .FirstOrDefault(d => d.PatientMedicationLists.Count != 0 && d.MedicationFrequencyId == model.MedicationFrequencyId) != null;
            if (inUse)
            {
                ViewBag.ErrorMessage = "Medications exist using this Frequency. Delete not available.";
                return View(model);
            }

            _repository.DeleteMedicationFrequency(model);

            return RedirectToAction("ViewFrequencies");
        }
        #endregion  // end of Frequencies section

        #region Private Methods
        /// <summary>
        /// Used to handle the formulas and populate the viewbag to enable pagination on Medication ViewMedications
        /// </summary>
        private void AddPagination(int pageSize) {
            ViewBag.TotalMedications = _repository.Medications.Count();

            int pagesMod = ViewBag.TotalMedications % pageSize;
            int totalPages = ((ViewBag.TotalMedications - pagesMod) / pageSize) + 1;

            ViewBag.Pages = new List<int>(Enumerable.Range(1, totalPages).ToList());
            ViewBag.NumberOfPages = totalPages;
        }

        /// <summary>
        /// Used to add the dropdown and datalist options for the create and edit medication methods.
        /// </summary>
        private void PopulateMedicationViewbag() {
            ViewBag.MedicationBrands = new List<MedicationBrandName>(_repository.MedicationBrandNames).OrderBy(item => item.BrandName);
            ViewBag.MedicationGenerics = new List<MedicationGenericName>(_repository.MedicationGenericNames).OrderBy(item => item.GenericName);
            ViewBag.DosageFormId = new SelectList(_repository.MedicationDosageForms, "DosageFormId","DosageForm").OrderBy(MedicationDosageForm => MedicationDosageForm.Text);
            ViewBag.DeliveryRouteId = new SelectList(_repository.MedicationDeliveryRoutes, "DeliveryRouteId","DeliveryRouteName").OrderBy(MedicationDeliveryRoute => MedicationDeliveryRoute.Text);
        }
        #endregion  // end of Private Methods section
    }
}