using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using IS_Proj_HIT.ViewModels.PatientVm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.Extensions.Logging;

namespace IS_Proj_HIT.Controllers
{

    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete")]
    public class PatientStructuredDataController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly ILogger<PatientStructuredDataController> _logger;
        
        public PatientStructuredDataController(IWCTCHealthSystemRepository repo,ILogger<PatientStructuredDataController> logger) 
        {
            _repository = repo;
            _logger = logger;
        } 

        
        /// <summary>
        /// Index page of PatientStructuredData
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult Index() 
        {
            return View();
        }

        #region Countries
        /// <summary>
        /// List view page of Countries
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewCountries()
        {
            List<Country> countries = _repository.Countries.ToList();
            return View("Countries/ViewCountries",countries);
        }

        /// <summary>
        /// Create New Country - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateCountry()
        {
            return View("Countries/CreateCountry");
        }

        ///<summary>
        /// Add new Country to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateCountry(PatientStructuredDataViewModel model)
        {
            model.Country.LastModified = DateTime.Now;

            _repository.AddCountry(model.Country);

            return RedirectToAction("ViewCountries");
        }

        ///<summary>
        /// Edit fields of a Country - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCountry(int id) 
        {
            Country country = _repository.Countries.FirstOrDefault(p => p.CountryId == id);
            return View("Countries/EditCountry",new PatientStructuredDataViewModel (country));
        }

        ///<summary>
        /// Edit fields of an Country in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditCountry(PatientStructuredDataViewModel model) 
        {
            model.Country.LastModified = DateTime.Now;
            _repository.EditCountry(model.Country);

            return RedirectToAction("ViewCountries");
        }

        ///<summary>
        /// Delete an existing Country - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCountry(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Country country = _repository.Countries.FirstOrDefault(p => p.CountryId == id);

            return View("Countries/DeleteCountry",new PatientStructuredDataViewModel (country));
        }

        ///<summary>
        /// Delete an existing Country in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteCountry(PatientStructuredDataViewModel model) 
        {
            Country country = _repository.Countries.FirstOrDefault(p => p.CountryId == model.Country.CountryId);

            try
            {
                _repository.DeleteCountry(country);
                return RedirectToAction("ViewCountries");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Country. Delete not available.";
                return View("Countries/DeleteCountry",model);
            }
        }
        #endregion  // end of Countries section
 
        #region Education Levels
        /// <summary>
        /// List view page of Education Levels
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewEducationLevels()
        {
            List<EducationLevel> educationLevels = _repository.EducationLevels.ToList();
            return View("EducationLevels/ViewEducationLevels",educationLevels);
        }

        /// <summary>
        /// Create New Education Level - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateEducationLevel()
        {
            return View("EducationLevels/CreateEducationLevel");
        }

        ///<summary>
        /// Add new Education Level to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateEducationLevel(PatientStructuredDataViewModel model)
        {
            _repository.AddEducationLevel(model.EducationLevel);

            return RedirectToAction("ViewEducationLevels");
        }

        ///<summary>
        /// Edit fields of a Education Level - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditEducationLevel(byte id) 
        {
            EducationLevel educationLevel = _repository.EducationLevels.FirstOrDefault(el => el.EducationId == id);
            return View("EducationLevels/EditEducationLevel",new PatientStructuredDataViewModel (educationLevel));
        }

        ///<summary>
        /// Edit fields of an Education Level in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditEducationLevel(PatientStructuredDataViewModel model) 
        {
            _repository.EditEducationLevel(model.EducationLevel);

            return RedirectToAction("ViewEducationLevels");
        }

        ///<summary>
        /// Delete an existing Education Level - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteEducationLevel(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            EducationLevel educationLevel = _repository.EducationLevels.FirstOrDefault(el => el.EducationId == id);

            return View("EducationLevels/DeleteEducationLevel",new PatientStructuredDataViewModel (educationLevel));
        }

        ///<summary>
        /// Delete an existing Education Level in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteEducationLevel(PatientStructuredDataViewModel model) 
        {
            EducationLevel educationLevel = _repository.EducationLevels.FirstOrDefault(ip => ip.EducationId == model.EducationLevel.EducationId);

            try
            {
                _repository.DeleteEducationLevel(educationLevel);
                return RedirectToAction("ViewEducationLevels");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Education Level. Delete not available.";
                return View("EducationLevels/DeleteEducationLevel",model);
            }
        }
        #endregion  // end of Education Level section

        #region Ethnicity
        /// <summary>
        /// List view page of Ethnicity
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewEthnicities()
        {
            List<Ethnicity> ethnicities = _repository.Ethnicities.ToList();
            return View("Ethnicities/ViewEthnicities",ethnicities);
        }

        /// <summary>
        /// Create New Ethnicity - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateEthnicity()
        {
            return View("Ethnicities/CreateEthnicity");
        }

        ///<summary>
        /// Add new Ethnicity to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateEthnicity(PatientStructuredDataViewModel model)
        {
            _repository.AddEthnicity(model.Ethnicity);

            return RedirectToAction("ViewEthnicities");
        }

        ///<summary>
        /// Edit fields of a Ethnicity - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditEthnicity(byte id) 
        {
            Ethnicity ethnicity = _repository.Ethnicities.FirstOrDefault(el => el.EthnicityId == id);
            return View("Ethnicities/EditEthnicity",new PatientStructuredDataViewModel (ethnicity));
        }

        ///<summary>
        /// Edit fields of an Ethnicity in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditEthnicity(PatientStructuredDataViewModel model) 
        {
            model.Ethnicity.LastModified = DateTime.Now;
            _repository.EditEthnicity(model.Ethnicity);

            return RedirectToAction("ViewEthnicities");
        }

        ///<summary>
        /// Delete an existing Ethnicity - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteEthnicity(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Ethnicity ethnicity = _repository.Ethnicities.FirstOrDefault(e => e.EthnicityId == id);

            return View("Ethnicities/DeleteEthnicity",new PatientStructuredDataViewModel (ethnicity));
        }

        ///<summary>
        /// Delete an existing Ethnicity in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteEthnicity(PatientStructuredDataViewModel model) 
        {
            Ethnicity ethnicity = _repository.Ethnicities.FirstOrDefault(e => e.EthnicityId == model.Ethnicity.EthnicityId);

            try
            {
                _repository.DeleteEthnicity(ethnicity);
                return RedirectToAction("ViewEthnicities");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Ethnicity. Delete not available.";
                return View("Ethnicities/DeleteEthnicity",model);
            }
        }
        #endregion  // end of Ethnicity section

        #region Genders
        /// <summary>
        /// List view page of Genders
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewGenders()
        {
            List<Gender> genders = _repository.Genders.ToList();
            return View("Genders/ViewGenders",genders);
        }

        /// <summary>
        /// Create New Gender - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateGender()
        {
            return View("Genders/CreateGender");
        }

        ///<summary>
        /// Add new Gender to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateGender(PatientStructuredDataViewModel model)
        {
            model.Gender.LastModified = DateTime.Now;

            _repository.AddGender(model.Gender);

            return RedirectToAction("ViewGenders");
        }

        ///<summary>
        /// Edit fields of a Gender - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditGender(byte id) 
        {
            Gender gender = _repository.Genders.FirstOrDefault(p => p.GenderId == id);
            return View("Genders/EditGender",new PatientStructuredDataViewModel (gender));
        }

        ///<summary>
        /// Edit fields of an Gender in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditGender(PatientStructuredDataViewModel model) 
        {
            model.Gender.LastModified = DateTime.Now;
            _repository.EditGender(model.Gender);

            return RedirectToAction("ViewGenders");
        }

        ///<summary>
        /// Delete an existing Gender - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteGender(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Gender gender = _repository.Genders.FirstOrDefault(p => p.GenderId == id);

            return View("Genders/DeleteGender",new PatientStructuredDataViewModel (gender));
        }

        ///<summary>
        /// Delete an existing Gender in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteGender(PatientStructuredDataViewModel model) 
        {
            Gender gender = _repository.Genders.FirstOrDefault(p => p.GenderId == model.Gender.GenderId);

            try
            {
                _repository.DeleteGender(gender);
                return RedirectToAction("ViewGenders");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Gender. Delete not available.";
                return View("Genders/DeleteGender",model);
            }
        }
        #endregion  // end of Gender section

        #region Gender Pronouns
        /// <summary>
        /// List view page of GenderPronouns
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewGenderPronouns()
        {
            List<GenderPronoun> genderPronouns = _repository.GenderPronouns.ToList();
            return View("GenderPronouns/ViewGenderPronoun",genderPronouns);
        }

        /// <summary>
        /// Create New GenderPronoun - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateGenderPronoun()
        {
            return View("GenderPronouns/CreateGenderPronoun");
        }

        ///<summary>
        /// Add new GenderPronoun to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateGenderPronoun(PatientStructuredDataViewModel model)
        {
            model.GenderPronoun.LastModified = DateTime.Now;

            _repository.AddGenderPronoun(model.GenderPronoun);

            return RedirectToAction("ViewGenderPronouns");
        }

        ///<summary>
        /// Edit fields of a GenderPronoun - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditGenderPronoun(int id) 
        {
            GenderPronoun genderPronoun = _repository.GenderPronouns.FirstOrDefault(p => p.GenderPronounId == id);
            return View("GenderPronouns/EditGenderPronoun",new PatientStructuredDataViewModel (genderPronoun));
        }

        ///<summary>
        /// Edit fields of an GenderPronoun in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditGenderPronoun(PatientStructuredDataViewModel model) 
        {
            model.GenderPronoun.LastModified = DateTime.Now;
            _repository.EditGenderPronoun(model.GenderPronoun);

            return RedirectToAction("ViewGenderPronouns");
        }

        ///<summary>
        /// Delete an existing GenderPronoun - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteGenderPronoun(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            GenderPronoun genderPronoun = _repository.GenderPronouns.FirstOrDefault(p => p.GenderPronounId == id);

            return View("GenderPronouns/DeleteGenderPronoun",new PatientStructuredDataViewModel (genderPronoun));
        }

        ///<summary>
        /// Delete an existing GenderPronoun in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteGenderPronoun(PatientStructuredDataViewModel model) 
        {
            GenderPronoun genderPronoun = _repository.GenderPronouns.FirstOrDefault(p => p.GenderPronounId == model.GenderPronoun.GenderPronounId);

            try
            {
                _repository.DeleteGenderPronoun(genderPronoun);
                return RedirectToAction("ViewGenderPronouns");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this GenderPronoun. Delete not available.";
                return View("GenderPronouns/DeleteGenderPronoun",model);
            }
        }
        #endregion  // end of Gender Pronoun section

        #region Insurance Providers
        /// <summary>
        /// List view page of Insurance Providers
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewInsuranceProviders()
        {
            List<InsuranceProvider> insuranceProviders = _repository.InsuranceProviders.ToList();
            return View("InsuranceProviders/ViewInsuranceProviders",insuranceProviders);
        }

        /// <summary>
        /// Create New Insurance Provider - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateInsuranceProvider()
        {
            return View("InsuranceProviders/CreateInsuranceProvider");
        }

        ///<summary>
        /// Add new Insurance Provider to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateInsuranceProvider(PatientStructuredDataViewModel model)
        {
            _repository.AddInsuranceProvider(model.InsuranceProvider);

            return RedirectToAction("ViewInsuranceProviders");
        }

        ///<summary>
        /// Edit fields of a Insurance Provider - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditInsuranceProvider(short id) 
        {
            InsuranceProvider insuranceProvider = _repository.InsuranceProviders.FirstOrDefault(ip => ip.InsuranceProviderId == id);
            return View("InsuranceProviders/EditInsuranceProvider",new PatientStructuredDataViewModel (insuranceProvider));
        }

        ///<summary>
        /// Edit fields of an Insurance Provider in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditInsuranceProvider(PatientStructuredDataViewModel model) 
        {
            _repository.EditInsuranceProvider(model.InsuranceProvider);

            return RedirectToAction("ViewInsuranceProviders");
        }

        ///<summary>
        /// Delete an existing Insurance Provider - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteInsuranceProvider(short id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            InsuranceProvider insuranceProvider = _repository.InsuranceProviders.FirstOrDefault(ip => ip.InsuranceProviderId == id);

            return View("InsuranceProviders/DeleteInsuranceProvider",new PatientStructuredDataViewModel (insuranceProvider));
        }

        ///<summary>
        /// Delete an existing Insurance Provider in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteInsuranceProvider(PatientStructuredDataViewModel model) 
        {
            InsuranceProvider insuranceProvider = _repository.InsuranceProviders.FirstOrDefault(ip => ip.InsuranceProviderId == model.InsuranceProvider.InsuranceProviderId);

            try
            {
                _repository.DeleteInsuranceProvider(insuranceProvider);
                return RedirectToAction("ViewInsuranceProviders");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Insurance Provider. Delete not available.";
                return View("InsuranceProviders/DeleteInsuranceProvider",model);
            }
        }
        #endregion  // end of Insurance Provider section

        #region Languages
        /// <summary>
        /// List view page of Language
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewLanguages()
        {
            List<Language> languages = _repository.Languages.ToList();
            return View("Languages/ViewLanguages",languages);
        }

        /// <summary>
        /// Create New Language - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateLanguage()
        {
            return View("Languages/CreateLanguage");
        }

        ///<summary>
        /// Add new Language to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateLanguage(PatientStructuredDataViewModel model)
        {
            _repository.AddLanguage(model.Language);

            return RedirectToAction("ViewLanguages");
        }

        ///<summary>
        /// Edit fields of a Language - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditLanguage(short id) 
        {
            Language language = _repository.Languages.FirstOrDefault(el => el.LanguageId == id);
            return View("Languages/EditLanguage",new PatientStructuredDataViewModel (language));
        }

        ///<summary>
        /// Edit fields of an Language in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditLanguage(PatientStructuredDataViewModel model) 
        {
            model.Language.LastModified = DateTime.Now;
            _repository.EditLanguage(model.Language);

            return RedirectToAction("ViewLanguages");
        }

        ///<summary>
        /// Delete an existing Language - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteLanguage(short id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Language language = _repository.Languages.FirstOrDefault(e => e.LanguageId == id);

            return View("Languages/DeleteLanguage",new PatientStructuredDataViewModel (language));
        }

        ///<summary>
        /// Delete an existing Language in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteLanguage(PatientStructuredDataViewModel model) 
        {
            Language language = _repository.Languages.FirstOrDefault(e => e.LanguageId == model.Language.LanguageId);

            try
            {
                _repository.DeleteLanguage(language);
                return RedirectToAction("ViewLanguages");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Language. Delete not available.";
                return View("Languages/DeleteLanguage",model);
            }
        }
        #endregion  // end of Language section

        #region Legal Status
        /// <summary>
        /// List view page of LegalStatus
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewLegalStatuses()
        {
            List<LegalStatus> legalStatuses = _repository.LegalStatuses.ToList();
            return View("LegalStatuses/ViewLegalStatuses",legalStatuses);
        }

        /// <summary>
        /// Create New LegalStatus - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateLegalStatus()
        {
            return View("LegalStatuses/CreateLegalStatus");
        }

        ///<summary>
        /// Add new LegalStatus to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateLegalStatus(PatientStructuredDataViewModel model)
        {
            _repository.AddLegalStatus(model.LegalStatus);

            return RedirectToAction("ViewLegalStatuses");
        }

        ///<summary>
        /// Edit fields of a LegalStatus - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditLegalStatus(byte id) 
        {
            LegalStatus legalStatus = _repository.LegalStatuses.FirstOrDefault(el => el.LegalStatusId == id);
            return View("LegalStatuses/EditLegalStatus",new PatientStructuredDataViewModel (legalStatus));
        }

        ///<summary>
        /// Edit fields of an LegalStatus in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditLegalStatus(PatientStructuredDataViewModel model) 
        {
            model.LegalStatus.LastModified = DateTime.Now;
            _repository.EditLegalStatus(model.LegalStatus);

            return RedirectToAction("ViewLegalStatuses");
        }

        ///<summary>
        /// Delete an existing LegalStatus - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteLegalStatus(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            LegalStatus legalStatus = _repository.LegalStatuses.FirstOrDefault(e => e.LegalStatusId == id);

            return View("LegalStatuses/DeleteLegalStatus",new PatientStructuredDataViewModel (legalStatus));
        }

        ///<summary>
        /// Delete an existing LegalStatus in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteLegalStatus(PatientStructuredDataViewModel model) 
        {
            LegalStatus legalStatus = _repository.LegalStatuses.FirstOrDefault(e => e.LegalStatusId == model.LegalStatus.LegalStatusId);

            try
            {
                _repository.DeleteLegalStatus(legalStatus);
                return RedirectToAction("ViewLegalStatuses");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Legal Status. Delete not available.";
                return View("LegalStatuses/DeleteLegalStatus",model);
            }
        }
        #endregion  // end of Legal Status section

        #region Marital Status
        /// <summary>
        /// List view page of Marital Status
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewMaritalStatuses()
        {
            List<MaritalStatus> maritalStatus = _repository.MaritalStatuses.ToList();
            return View("MaritalStatuses/ViewMaritalStatuses",maritalStatus);
        }

        /// <summary>
        /// Create New Marital Status - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateMaritalStatus()
        {
            return View("MaritalStatuses/CreateMaritalStatus");
        }

        ///<summary>
        /// Add new Marital Status to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateMaritalStatus(PatientStructuredDataViewModel model)
        {
            _repository.AddMaritalStatus(model.MaritalStatus);

            return RedirectToAction("ViewMaritalStatuses");
        }

        ///<summary>
        /// Edit fields of a Marital Status - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditMaritalStatus(byte id) 
        {
            MaritalStatus maritalStatus = _repository.MaritalStatuses.FirstOrDefault(el => el.MaritalStatusId == id);
            return View("MaritalStatuses/EditMaritalStatus",new PatientStructuredDataViewModel (maritalStatus));
        }

        ///<summary>
        /// Edit fields of an Marital Status in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditMaritalStatus(PatientStructuredDataViewModel model) 
        {
            model.MaritalStatus.LastModified = DateTime.Now;
            _repository.EditMaritalStatus(model.MaritalStatus);

            return RedirectToAction("ViewMaritalStatuses");
        }

        ///<summary>
        /// Delete an existing Marital Status - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteMaritalStatus(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            MaritalStatus maritalStatus = _repository.MaritalStatuses.FirstOrDefault(e => e.MaritalStatusId == id);

            return View("MaritalStatuses/DeleteMaritalStatus",new PatientStructuredDataViewModel (maritalStatus));
        }

        ///<summary>
        /// Delete an existing Marital Status in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteMaritalStatus(PatientStructuredDataViewModel model) 
        {
            MaritalStatus maritalStatus = _repository.MaritalStatuses.FirstOrDefault(e => e.MaritalStatusId == model.MaritalStatus.MaritalStatusId);

            try
            {
                _repository.DeleteMaritalStatus(maritalStatus);
                return RedirectToAction("ViewMaritalStatuses");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Marital Status. Delete not available.";
                return View("MaritalStatuses/DeleteMaritalStatus",model);
            }
        }
        #endregion  // end of Marital Status section

        #region Preferred Contact Time
        /// <summary>
        /// List view page of Preferred Contact Time
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPreferredContactTimes()
        {
            List<PreferredContactTime> preferredContactTime = _repository.PreferredContactTimes.ToList();
            return View("PreferredContactTimes/ViewPreferredContactTimes",preferredContactTime);
        }

        /// <summary>
        /// Create New Preferred Contact Time - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePreferredContactTime()
        {
            return View("PreferredContactTimes/CreatePreferredContactTime");
        }

        ///<summary>
        /// Add new Preferred Contact Time to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreatePreferredContactTime(PatientStructuredDataViewModel model)
        {
            _repository.AddPreferredContactTime(model.PreferredContactTime);

            return RedirectToAction("ViewPreferredContactTimes");
        }

        ///<summary>
        /// Edit fields of a Preferred Contact Time - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPreferredContactTime(int id) 
        {
            PreferredContactTime preferredContactTime = _repository.PreferredContactTimes.FirstOrDefault(el => el.ContactTimeId == id);
            return View("PreferredContactTimes/EditPreferredContactTime",new PatientStructuredDataViewModel (preferredContactTime));
        }

        ///<summary>
        /// Edit fields of an Preferred Contact Time in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPreferredContactTime(PatientStructuredDataViewModel model) 
        {
            model.PreferredContactTime.LastModified = DateTime.Now;
            _repository.EditPreferredContactTime(model.PreferredContactTime);

            return RedirectToAction("ViewPreferredContactTimes");
        }

        ///<summary>
        /// Delete an existing Preferred Contact Time - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePreferredContactTime(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            PreferredContactTime preferredContactTime = _repository.PreferredContactTimes.FirstOrDefault(e => e.ContactTimeId == id);

            return View("PreferredContactTimes/DeletePreferredContactTime",new PatientStructuredDataViewModel (preferredContactTime));
        }

        ///<summary>
        /// Delete an existing Preferred Contact Time in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePreferredContactTime(PatientStructuredDataViewModel model) 
        {
            PreferredContactTime preferredContactTime = _repository.PreferredContactTimes.FirstOrDefault(e => e.ContactTimeId == model.PreferredContactTime.ContactTimeId);

            try
            {
                _repository.DeletePreferredContactTime(preferredContactTime);
                return RedirectToAction("ViewPreferredContactTimes");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Preferred Contact Time. Delete not available.";
                return View("PreferredContactTimes/DeletePreferredContactTime",model);
            }
        }
        #endregion  // end of Preferred Contact Time section

        #region Preferred Mode of Contact
        /// <summary>
        /// List view page of Preferred Mode Of Contact
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewPreferredModeOfContacts()
        {
            List<PreferredModeOfContact> preferredModeOfContacts = _repository.PreferredModesOfContact.ToList();
            return View("PreferredModeOfContacts/ViewPreferredModeOfContacts",preferredModeOfContacts);
        }

        /// <summary>
        /// Create New Preferred Mode Of Contact - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreatePreferredModeOfContact()
        {
            return View("PreferredModeOfContacts/CreatePreferredModeOfContact");
        }

        ///<summary>
        /// Add new Preferred Mode Of Contact to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreatePreferredModeOfContact(PatientStructuredDataViewModel model)
        {
            _repository.AddPreferredModeOfContact(model.PreferredModeOfContact);

            return RedirectToAction("ViewPreferredModeOfContacts");
        }

        ///<summary>
        /// Edit fields of a Preferred Mode Of Contact - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPreferredModeOfContact(int id) 
        {
            PreferredModeOfContact preferredModeOfContact = _repository.PreferredModesOfContact.FirstOrDefault(el => el.ModeOfContactId == id);
            return View("PreferredModeOfContacts/EditPreferredModeOfContact",new PatientStructuredDataViewModel (preferredModeOfContact));
        }

        ///<summary>
        /// Edit fields of an Preferred Mode Of Contact in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditPreferredModeOfContact(PatientStructuredDataViewModel model) 
        {
            model.PreferredModeOfContact.LastModified = DateTime.Now;
            _repository.EditPreferredModeOfContact(model.PreferredModeOfContact);

            return RedirectToAction("ViewPreferredModeOfContacts");
        }

        ///<summary>
        /// Delete an existing Preferred Mode Of Contact - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePreferredModeOfContact(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            PreferredModeOfContact preferredModeOfContact = _repository.PreferredModesOfContact.FirstOrDefault(e => e.ModeOfContactId == id);

            return View("PreferredModeOfContacts/DeletePreferredModeOfContact",new PatientStructuredDataViewModel (preferredModeOfContact));
        }

        ///<summary>
        /// Delete an existing Preferred Mode Of Contact in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeletePreferredModeOfContact(PatientStructuredDataViewModel model) 
        {
            PreferredModeOfContact preferredModeOfContact = _repository.PreferredModesOfContact.FirstOrDefault(e => e.ModeOfContactId == model.PreferredModeOfContact.ModeOfContactId);

            try
            {
                _repository.DeletePreferredModeOfContact(preferredModeOfContact);
                return RedirectToAction("ViewPreferredModeOfContacts");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Preferred Mode of Contact. Delete not available.";
                return View("PreferredModeOfContacts/DeletePreferredModeOfContact",model);
            }
        }
        #endregion  // end of Preferred Mode of Contact section

        #region Race
        /// <summary>
        /// List view page of Race
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewRaces()
        {
            List<Race> races = _repository.Races.ToList();
            return View("Race/ViewRaces",races);
        }

        /// <summary>
        /// Create New Race - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateRace()
        {
            return View("Race/CreateRace");
        }

        ///<summary>
        /// Add new Race to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateRace(PatientStructuredDataViewModel model)
        {
            model.Race.LastModified = DateTime.Now;

            _repository.AddRace(model.Race);

            return RedirectToAction("ViewRaces");
        }

        ///<summary>
        /// Edit fields of a Race - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRace(int id) 
        {
            Race race = _repository.Races.FirstOrDefault(p => p.RaceId == id);
            return View("Race/EditRace",new PatientStructuredDataViewModel (race));
        }

        ///<summary>
        /// Edit fields of an Race in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditRace(PatientStructuredDataViewModel model) 
        {
            model.Race.LastModified = DateTime.Now;
            _repository.EditRace(model.Race);

            return RedirectToAction("ViewRaces");
        }

        ///<summary>
        /// Delete an existing Race - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRace(int id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Race race = _repository.Races.FirstOrDefault(p => p.RaceId == id);

            return View("Race/DeleteRace",new PatientStructuredDataViewModel (race));
        }

        ///<summary>
        /// Delete an existing Race in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteRace(PatientStructuredDataViewModel model) 
        {
            Race race = _repository.Races.FirstOrDefault(p => p.RaceId == model.Race.RaceId);

            try
            {
                _repository.DeleteRace(race);
                return RedirectToAction("ViewRaces");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Race. Delete not available.";
                return View("Race/DeleteRace",model);
            }
        }
        #endregion  // end of Race section

        #region Religion
        /// <summary>
        /// List view page of Religion
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewReligions()
        {
            List<Religion> religions = _repository.Religions.ToList();
            return View("Religions/ViewReligions",religions);
        }

        /// <summary>
        /// Create New Religion - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateReligion()
        {
            return View("Religions/CreateReligion");
        }

        ///<summary>
        /// Add new Religion to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]  
        public IActionResult CreateReligion(PatientStructuredDataViewModel model)
        {
            _repository.AddReligion(model.Religion);

            return RedirectToAction("ViewReligions");
        }

        ///<summary>
        /// Edit fields of a Religion - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditReligion(short id) 
        {
            Religion religion = _repository.Religions.FirstOrDefault(r => r.ReligionId == id);
            return View("Religions/EditReligion",new PatientStructuredDataViewModel (religion));
        }

        ///<summary>
        /// Edit fields of an Religion in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditReligion(PatientStructuredDataViewModel model) 
        {
            model.Religion.LastModified = DateTime.Now;
            _repository.EditReligion(model.Religion);

            return RedirectToAction("ViewReligions");
        }

        ///<summary>
        /// Delete an existing Religion - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteReligion(short id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Religion religion = _repository.Religions.FirstOrDefault(r => r.ReligionId == id);

            return View("Religions/DeleteReligion",new PatientStructuredDataViewModel (religion));
        }

        ///<summary>
        /// Delete an existing Religion in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteReligion(PatientStructuredDataViewModel model) 
        {
            Religion religion = _repository.Religions.FirstOrDefault(r => r.ReligionId == model.Religion.ReligionId);

            try
            {
                _repository.DeleteReligion(religion);
                return RedirectToAction("ViewReligions");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Religion. Delete not available.";
                return View("Religions/DeleteReligion",model);
            }
        }
        #endregion  // end of Religion section

        #region Sex
        /// <summary>
        /// List view page of Sex
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataView")]
        public IActionResult ViewSexes()
        {
            List<Sex> sexes = _repository.Sexes.ToList();
            return View("Sexes/ViewSexes",sexes);
        }

        /// <summary>
        /// Create New Sex - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")]
        public IActionResult CreateSex()
        {
            return View("Sexes/CreateSex");
        }

        ///<summary>
        /// Add new Sex to database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd")] 
        public IActionResult CreateSex(PatientStructuredDataViewModel model)
        {
            _repository.AddSex(model.Sex);

            return RedirectToAction("ViewSexes");
        }

        ///<summary>
        /// Edit fields of a Sex - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditSex(byte id) 
        {
            Sex sex = _repository.Sexes.FirstOrDefault(s => s.SexId == id);
            return View("Sexes/EditSex",new PatientStructuredDataViewModel (sex));
        }

        ///<summary>
        /// Edit fields of an Sex in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataEdit")]
        public IActionResult EditSex(PatientStructuredDataViewModel model) 
        {
            model.Sex.LastModified = DateTime.Now;
            _repository.EditSex(model.Sex);

            return RedirectToAction("ViewSexes");
        }

        ///<summary>
        /// Delete an existing Sex - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteSex(byte id) 
        {
            ViewData["RegularMessage"] = "Are you sure you want to delete this?";
            ViewData["ErrorMessage"] = "";

            Sex sex = _repository.Sexes.FirstOrDefault(s => s.SexId == id);

            return View("Sexes/DeleteSex",new PatientStructuredDataViewModel (sex));
        }

        ///<summary>
        /// Delete an existing Sex in the database - Setter
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("StructuredDataDelete")]
        public IActionResult DeleteSex(PatientStructuredDataViewModel model) 
        {
            Sex sex = _repository.Sexes.FirstOrDefault(s => s.SexId == model.Sex.SexId);

            try
            {
                _repository.DeleteSex(sex);
                return RedirectToAction("ViewSexes");
            }
            catch (Exception)
            {
                ViewData["RegularMessage"] = "";
                ViewData["ErrorMessage"] = "Records exist using this Sex. Delete not available.";
                return View("Sexes/DeleteSex",model);
            }
        }
        #endregion  // end Sex section
    }
}