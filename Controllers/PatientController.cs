using IS_Proj_HIT.ViewModels.Alert;
using IS_Proj_HIT.ViewModels.Allergy;
using IS_Proj_HIT.ViewModels.PatientVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Helpers;
using IS_Proj_HIT.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
    public class PatientController : BaseController
    {
        private IWCTCHealthSystemRepository repository;
        private readonly ILogger<PatientController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPatientService _patientService;
        private readonly IAllergenService _allergenService;
        public int PageSize = 10;
        public PatientController(IWCTCHealthSystemRepository repo, ILogger<PatientController> logger, UserManager<IdentityUser> userManager, IPatientService patientService, IAllergenService allergenService)
        {
            repository = repo;
            _logger = logger;
            _userManager = userManager;
            _patientService = patientService;
            _allergenService = allergenService;
        }

        #region Patient
        /// <summary>
        /// Displays list of patients found via patient search
        /// </summary>
        /// <param name="searchLast">Last name to search for</param>
        /// <param name="searchFirst">First name to search for</param>
        /// <param name="searchSSN">SSN...</param>
        /// <param name="searchMRN">MRN...</param>
        /// <param name="searchDOB">Date of birth...</param>
        /// <param name="searchDOBBefore">Date before date of birth...</param>
        // Used in: PatientSearch
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public ActionResult Index(string searchLast, string searchFirst, string searchSSN,
            string searchMRN, DateTime searchDOB, DateTime searchDOBBefore)
        {
            // Retrieve FacilityId from the session
            var facilityIdString = HttpContext.Session.GetString("Facility");
            if (string.IsNullOrEmpty(facilityIdString))
            {
                // Handle the case where Facility is not set in the session
                return Unauthorized("Facility not set for the session.");
            }
            int facilityId = int.Parse(facilityIdString);

            // Clear ModelState
            ViewData.ModelState.Clear();

            // Normalize the search input
            searchLast = string.IsNullOrEmpty(searchLast) ? "" : searchLast.ToUpper().Trim();

            IQueryable<Patient> patientsQuery = repository.Patients
                .Include(p => p.Facility) // still on Patient
                .Include(p => p.Person) // include the Person navigation
                    .ThenInclude(person => person.Religion) // don't believe these are needed except for Facility, but keeping for initial refactor
                .Include(p => p.Person)
                    .ThenInclude(person => person.Gender)
                .Include(p => p.Person)
                    .ThenInclude(person => person.Ethnicity)
                .Include(p => p.Person)
                    .ThenInclude(person => person.MaritalStatus)
                .AsNoTracking()
                .Where(p => p.Person.LastName.ToUpper().Contains(searchLast));

            if (!searchFirst.IsNullOrEmpty())
            {
                patientsQuery = patientsQuery.Where(p => p.Person.FirstName.ToUpper().Contains(searchFirst));
            }

            if (!searchSSN.IsNullOrEmpty())
            {
                patientsQuery = patientsQuery.Where(p => p.Person.Ssn.Contains(searchSSN));
            }

            if (!searchMRN.IsNullOrEmpty())
            {
                patientsQuery = patientsQuery.Where(p => p.Mrn.Contains(searchMRN));
            }

            if (!(searchDOB.GetHashCode() == 0))
            {
                patientsQuery = patientsQuery.Where(p => p.Person.Dob >= searchDOB || p.Person.Dob == null);
            }

            if (!(searchDOBBefore.GetHashCode() == 0))
            {
                patientsQuery = patientsQuery.Where(p => p.Person.Dob <= searchDOBBefore || p.Person.Dob == null);
            }

            // Execute the query to fetch the data, waiting until the end to do the ToList
            var patients = patientsQuery
                            .OrderBy(p => p.Person.LastName)
                            .ToList();

            var patientsAllFacilities = patientsQuery
                            .OrderBy(p => p.Facility.Name)
                            .ThenBy(p => p.Person.LastName)
                            .ToList();

            var patientsSessionFacility = patientsQuery
                            .Where(p => p.FacilityId == facilityId)
                            .OrderBy(p => p.Person.LastName)
                            .ToList();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var vm = new PatientListViewModel
            {
                Patients = patients,
                PatientsAllFacilities = patientsAllFacilities,
                PatientsSessionFacility = patientsSessionFacility,
                SessionFacilityName = patientsSessionFacility.FirstOrDefault()?.Facility?.Name ?? "Unknown Facility",

            };

            return View(vm);
        }

        /// <summary>
        /// Return PatientSearch view
        /// </summary>
        // Used in: Navbar (_Layout), Home Page, PatientSearchIndex, PatientIndex, AddPatient
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public IActionResult PatientSearch() => View();

        /// <summary>
        /// Create a new Patient - Getter
        /// </summary>
        [Authorize]
        [PermissionAuthorize("PatientAdd")]
        public IActionResult CreatePatient()
        {
            Patient patient;
            //Run stored procedure from SQL database to generate the MRN number
            using (var context = new WCTCHealthSystemContext())
            {
                var data = context.Patients.FromSqlRaw("EXECUTE dbo.GetNextMRN").ToList();
                ViewBag.MRN = data.FirstOrDefault()?.Mrn;
                patient = data.FirstOrDefault();

            }
            // this boolean toggles the display of certain UI things between the creating a patient and editing a patient
            //  as both views employ CreatePatient.cshtml; it is true so the Create Patient UI features are rendered
            ViewBag.CreatingPatient = true;

            // this boolean toggles the display of certain UI things which are specific to the Details view and is also
            //  employed within the CreatePatient.cshtml as well as within all partial.cshtml views;
            //  it is false so the details UI will not be rendered
            ViewBag.ViewPatientDetails = false;

            // this boolean is used to suppress certain UI things when the view is opened in a modal;  it should always be false here because the initial CreatePatient should not occur inside a modal
            ViewBag.IsModal = false;

            // this statement creates a list of the facilities associated to the User who is creating the Patient
            ViewBag.UserFacilities = GetUserFacilities();

            var model = new PatientViewModel { };
            AddDropdowns();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View(model);
        }

        ///<summary>
        ///     Add new Patient to database OR update an existing Patient in the database - Setter
        /// It is named CreatePatient as it started out that way, but with only a few more lines of
        /// code it can Create or Edit a patient record
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientEdit")]
        public IActionResult CreatePatient(PatientViewModel model)
        {
            // set a boolean for whether Person exists
            bool personExists = false;

            // check for PersonId presence
            if(model.Person.PersonId > 0)
            {
                model.Patient.PersonId = model.Person.PersonId;
                personExists = true;
                // update Patient mandatory fields until Patient entity has been refactored to remove them
                model.Patient.FirstName = model.Person.FirstName;
                model.Patient.LastName = model.Person.LastName;
            }
            
            // strip masks from ssn
            model.Person.Ssn = SsnHelper.FormatSsn(model.Person.Ssn);

            // populate the EditedBy field
            var editedBy = UserHelper.GetEditedBy(repository, _userManager, User);

            model.Patient.EditedBy = editedBy;
            model.Person.EditedBy = editedBy;

            // check for user entries in Employment
            //  if no entries, the EmploymentId = null and neither an Employment record
            //  nor an Employer Address record is created
            if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.Employment, _logger,
                ("LastModified", DateTime.MinValue)))
            {
                // UtilityHelper-Employment = true, there is an entry in an Employment field,
                //  now must check for an entry in Address
                if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.EmploymentAddress, _logger,
                    ("CountryId", 1),
                    ("AddressStateID", 50),
                    ("LastModified", DateTime.MinValue)))
                {
                    // UtilityHelper-Address = true, there is an entry in the Employment Address, so
                    //  create an address record
                    var employmentAddress = MapAddress(model.EmploymentAddress);

                    // tie the new Address record to the Employment record
                    model.Employment.AddressId = employmentAddress.AddressId;
                }
                else
                {
                    // UtilityHelper - Address = false, no need for an Address record
                    model.Employment.Address = null;  // ensure the Address entity is null
                    model.Employment.AddressId = null;
                }
                // end of the Address conditional

                // strip the mask from any Employment-related phone numbers
                model.Employment.PhoneNumber = PhoneNumberHelper.FormatPhoneNumber(model.Employment.PhoneNumber);

                // add the new employment record to the database to generate the EmploymentId
                // or update the record if it already exists
                if (model.Employment.EmploymentId != 0)
                {
                    model.Employment.LastModified = DateTime.Now; // set the LastModified
                    repository.EditEmployment(model.Employment);
                }
                else
                {
                    model.Employment.LastModified = DateTime.Now; // set the LastModified
                    repository.AddEmployment(model.Employment);
                }

                // tie the Employment record to the Person record
                model.Person.EmploymentId = model.Employment.EmploymentId;
            }
            else
            {
                // UtilityHelper-Employment = false, so no need for an Employment record (or an address record)
                model.Person.EmploymentId = null;
            }

            if(personExists)
            {
                // Person record exists, so update it
                model.Person.LastModified = DateTime.Now;
                repository.EditPerson(model.Person);
            }
            else
            {
                // Person record does not exist, so create it
                model.Person.LastModified = DateTime.Now;
                repository.AddPerson(model.Person);

                // associate with Patient
                model.Patient.PersonId = model.Person.PersonId;
                // temporarily assign Patient mandatory fields until Patient has been refactored to remove them
                model.Patient.FirstName = model.Person.FirstName;
                model.Patient.LastName = model.Person.LastName;
            }
            // successfully ceated a Person record

            // Check to see if the Patient record exists
            var existingPatient = repository.Patients.FirstOrDefault(p => p.Mrn == model.Patient.Mrn);

            if (existingPatient != null)
            {
                // Detach the existing tracked entity to avoid tracking conflict
                repository.Detach(existingPatient);

                // Patient record exists, so update it
                model.Patient.LastModified = DateTime.Now;
                repository.EditPatient(model.Patient);
            }
            else
            {
                // Patient record does not exist, so create it
                model.Patient.LastModified = DateTime.Now;
                repository.AddPatient(model.Patient);
            }
            // successfully created a Patient record

            // Handle any PatientAliases
            foreach (var alias in model.PatientAliases)
            {
                // Check for User entries in any of the Alias fields
                if (!UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(alias, _logger,
                    (nameof(PatientAlias.AliasFirstName), string.Empty),
                    (nameof(PatientAlias.AliasMiddleName), string.Empty),
                    (nameof(PatientAlias.AliasLastName), string.Empty),
                    (nameof(PatientAlias.AliasPriority), 0),
                    ("LastModified", DateTime.MinValue)))
                {
                    // skip processing this alias if no relevant fields are set
                    continue;
                }

                // UtilityHelper-Alias is true, so create or update the record
                alias.PatientMRN = model.Patient.Mrn;

                // see it the record exists, if it does, just update it, if it doesn't, create it
                var existingAlias = repository.PatientAliases.FirstOrDefault(pa => pa.PatientAliasID == alias.PatientAliasID);
                if (existingAlias != null)
                {
                    // Detach the existing tracked entity to avoid tracking conflict
                    repository.Detach(existingAlias);
                    alias.LastModified = DateTime.Now;
                    repository.EditPatientAlias(alias);
                }
                else
                {
                    alias.LastModified = DateTime.Now;
                    repository.AddPatientAlias(alias);
                }
            }
            // end of Patient Aliases section

            // iterate over the list of Emergency Contacts
            foreach (var contact in model.EmergencyContact)
            {
                // check if Emergency contact has data
                if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(contact, _logger,
                    ("EmergencyContactId", 0),
                    ("LastModified", DateTime.MinValue)))
                {
                    // UtilityHelper-EmergencyContact = true, there is an entry in an EmergencyContact field,                     
                    // strip the mask from any EmergencyContact-related phone numbers
                    contact.ContactPhone = PhoneNumberHelper.FormatPhoneNumber(contact.ContactPhone);

                    // add the new EmergencyContact record to the database to generate the EmergencyContactId
                    contact.Mrn = model.Patient.Mrn;

                    if (contact.EmergencyContactId != 0) // true means an existing EmCon
                    {
                        if (contact.ContactAddressId != null)    // true means an existing ContactAddressId
                        {
                            contact.ContactAddress.LastModified = DateTime.Now; // needs updated because it comes through as null
                        }
                        else    // means there wasn't an existing ContactAddress with this existing EmCon, but they could have just created it
                        {
                            if (!UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(contact.ContactAddress, _logger,
                            ("CountryId", 1),
                            ("AddressStateID", 50),
                            ("LastModified", DateTime.MinValue)))
                            {
                                contact.ContactAddress = null;
                                contact.ContactAddressId = null;
                            }
                        }
                        contact.LastModified = DateTime.Now; // set the LastModified
                        repository.EditPatientEmergencyContact(contact);
                    }
                    else     // for a new EmCon, check for an address
                    {
                        if (!UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(contact.ContactAddress, _logger,
                            ("CountryId", 1),
                            ("AddressStateID", 50),
                            ("LastModified", DateTime.MinValue)))
                        {
                            contact.ContactAddress = null;
                            contact.ContactAddressId = null;
                        }
                        contact.LastModified = DateTime.Now; // set the LastModified
                        repository.AddPatientEmergencyContact(contact);
                    }
                }
            }
            // End of the Emergency Contact section

            // check if any Primary Insurance information was provided
            CreateOrEditPatientInsurance(model.PrimaryInsurance, model.PrimaryInsurance.InsuranceOrder, model.Patient.Mrn);

            // check if any Secondary Insurance information was provided
            CreateOrEditPatientInsurance(model.SecondaryInsurance, model.SecondaryInsurance.InsuranceOrder, model.Patient.Mrn);

            // check if any Tertiary Insurance information was provided
            CreateOrEditPatientInsurance(model.TertiaryInsurance, model.TertiaryInsurance.InsuranceOrder, model.Patient.Mrn);

            // Now deal with PersonContactDetail

            // strip the mask from any PersonContactDetail-related phone numbers
            model.PersonContactDetail.CellPhone = PhoneNumberHelper.FormatPhoneNumber(model.PersonContactDetail.CellPhone);
            model.PersonContactDetail.HomePhone = PhoneNumberHelper.FormatPhoneNumber(model.PersonContactDetail.HomePhone);
            model.PersonContactDetail.WorkPhone = PhoneNumberHelper.FormatPhoneNumber(model.PersonContactDetail.WorkPhone);

            //  Check first for any info in ResidenceAddress (because a person might not have any phones or email)
            if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.ResidenceAddress, _logger,
                ("CountryId", 1),
                ("AddressStateID", 50),
                ("LastModified", DateTime.MinValue)))
            {
                // UtilityHelper-Address = true, there is an entry in the Residence Address, so update or
                //  create an address record
                var residenceAddress = MapAddress(model.ResidenceAddress);

                // tie the Address record to the PatientContactDetails record
                model.PersonContactDetail.ResidenceAddressId = residenceAddress.AddressId;

                // check the boolean for true
                if (model.ResidenceSameAsMailing)
                {
                    // ResidenceSameAsMailing = true so assign the same AddressId to MailingAddressId
                    model.PersonContactDetail.MailingAddressId = residenceAddress.AddressId;

                    // Now tie in the Person.PersonId and update or create a PersonContactDetail record
                    model.PersonContactDetail.PersonId = model.Person.PersonId;

                    if (model.PersonContactDetail.PersonContactDetailId != 0)
                    {
                        model.PersonContactDetail.LastModified = DateTime.Now;
                        repository.EditPersonContactDetail(model.PersonContactDetail);
                    }
                    else
                    {
                        model.PersonContactDetail.LastModified = DateTime.Now;
                        repository.AddPersonContactDetail(model.PersonContactDetail);
                    }
                }
                // ResidenceSameAsMailing = False so check for entries in MailingAddress object
                else if (!model.ResidenceSameAsMailing)
                {
                    // It is possible the ResidenceAddress.AddressId and MailingAddress.AddressId were created originally as the same, and then an edit happened, say to change the MailingAddress to a PO Box, so now the two AddressIds need to be different and a new MailingAddress record created.  For this to happen, the MailingAddress.AddressId needs to be set to 0;  if they are already different then leave it alone and process as a regular edit
                    if (model.MailingAddress.AddressId == model.ResidenceAddress.AddressId)
                    {
                        model.MailingAddress.AddressId = 0;
                    }

                    if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.MailingAddress, _logger,
                        ("CountryId", 0),
                        ("AddressStateID", 0),
                        ("LastModified", DateTime.MinValue)))
                    {
                        // UtilityHelper-Address = true, there is an entry in the Mailing Address, so
                        //  create an address record
                        var mailingAddress = MapAddress(model.MailingAddress);

                        // tie the new Address record to the PersonContactDetail record
                        model.PersonContactDetail.MailingAddressId = mailingAddress.AddressId;

                        // since there is a PersonContactDetail.MailingAddress, finish creating a PCD record
                        model.PersonContactDetail.PersonId = model.Person.PersonId;

                        if (model.PersonContactDetail.PersonContactDetailId != 0)
                        {
                            model.PersonContactDetail.LastModified = DateTime.Now;
                            repository.EditPersonContactDetail(model.PersonContactDetail);
                        }
                        else
                        {
                            model.PersonContactDetail.LastModified = DateTime.Now;
                            repository.AddPersonContactDetail(model.PersonContactDetail);
                        }
                    }
                    // this finishes creating a PersonContactDetail with a ResidenceAddress and unique MailingAddress
                }
                else
                {
                    // to get here:  ResidenceAddress is created, ResidenceSameAsMailing = false, no unique MailingAddress so finish creating the PersonContactDetail 
                    model.PersonContactDetail.PersonId = model.Person.PersonId;

                    if (model.PersonContactDetail.PersonContactDetailId != 0)
                    {
                        model.PersonContactDetail.LastModified = DateTime.Now;
                        repository.EditPersonContactDetail(model.PersonContactDetail);
                    }
                    else
                    {
                        model.PersonContactDetail.LastModified = DateTime.Now;
                        repository.AddPersonContactDetail(model.PersonContactDetail);
                    }
                }
                // this completes the conditional for ResidenceAddress = true
            }

            // It is possible for ResidenceAddress to be empty yet still have entries in MailingAddress, so check it here
            // Note:  for whatever reason, it was necessary to create a MailingAddress in the constructor, 
            // so these default values are set to 0 unlike any of the other Address instances where this is used
            else if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.MailingAddress, _logger,
                    ("CountryId", 0),
                    ("AddressStateID", 0),
                    ("LastModified", DateTime.MinValue)))
            {
                // UtilityHelper-Address = true, there is an entry in the Mailing Address, so
                //  create an address record
                var mailingAddress2 = MapAddress(model.MailingAddress);

                // tie the new Address record to the PatientContactDetail record
                model.PersonContactDetail.MailingAddressId = mailingAddress2.AddressId;

                // since there is a PatientContactDetail.MailingAddress, finish creating a PCD record
                model.PersonContactDetail.PersonId = model.Person.PersonId;

                if (model.PersonContactDetail.PersonContactDetailId != 0)
                {
                    model.PersonContactDetail.LastModified = DateTime.Now;
                    repository.EditPersonContactDetail(model.PersonContactDetail);
                }
                else
                {
                    model.PersonContactDetail.LastModified = DateTime.Now;
                    repository.AddPersonContactDetail(model.PersonContactDetail);
                }
            }
            // this completes the conditional for ResidenceAddress = false, MailingAddress = true
            else
            {
                // UtilityHelper - Addresses = false, no need for any Address records
                model.PersonContactDetail.ResidenceAddress = null;  // ensure the Address entity is null
                model.PersonContactDetail.ResidenceAddressId = null;
                model.PersonContactDetail.MailingAddress = null;  // ensure the Address entity is null
                model.PersonContactDetail.MailingAddressId = null;

                // check for any other non-empty fields in PCD
                if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(model.PersonContactDetail, _logger, ("PersonContactDetailId", 0)))
                {
                    // UtilityHelper-PCD is true, so create the record
                    model.PersonContactDetail.PersonId = model.Person.PersonId;

                    if (model.PersonContactDetail.PersonContactDetailId != 0)
                    {
                        model.PersonContactDetail.LastModified = DateTime.Now;
                        repository.EditPersonContactDetail(model.PersonContactDetail);
                    }
                    else
                    {
                        model.PersonContactDetail.LastModified = DateTime.Now;
                        repository.AddPersonContactDetail(model.PersonContactDetail);
                    }
                }
            }
            // successfully completed the PersonContactDetail portion

            // Now check for PersonModeOfContacts
            if (model.SelectedPreferredModeOfContactIds != null && model.SelectedPreferredModeOfContactIds.Any())
            {
                // conditional result was true, something was selected
                // if a PCD was NOT created above, create it now because the PersonContactDetails.PersonContactDetailId is a foreign key needed in PersonModeOfContacts
                if (model.PersonContactDetail.PersonContactDetailId == 0)
                {
                    model.PersonContactDetail.PersonId = model.Person.PersonId;
                    model.PersonContactDetail.LastModified = DateTime.Now;
                    repository.AddPersonContactDetail(model.PersonContactDetail);
                }

                // Retrieve the current PersonModeOfContacts from the database
                var currentPersonModeOfContacts = GetPersonModeOfContacts(model.PersonContactDetail.PersonContactDetailId) ?? new List<PersonModeOfContact>();

                // Create a list from the model of new and updated PersonModeOfContacts based on the selected IDs
                var newPersonModeOfContacts = model.SelectedPreferredModeOfContactIds.Select(id => new PersonModeOfContact
                {
                    PersonContactDetailId = model.PersonContactDetail.PersonContactDetailId,
                    ModeOfContactId = id,
                    LastModified = DateTime.Now
                }).ToList();

                // Determine the PersonModeOfContacts to be added
                foreach (var newPersonModeOfContact in newPersonModeOfContacts)
                {
                    var existingContact = currentPersonModeOfContacts.FirstOrDefault(c => c.ModeOfContactId == newPersonModeOfContact.ModeOfContactId);

                    if (existingContact == null)
                    {
                        // Add new contact if it doesn't exist in the current contacts
                        repository.AddPersonModeOfContact(newPersonModeOfContact);
                    }
                }

                // Determine the PersonModeOfContacts to be removed
                var contactsToRemove = currentPersonModeOfContacts.Where(c => !model.SelectedPreferredModeOfContactIds.Contains(c.ModeOfContactId)).ToList();
                foreach (var contactToRemove in contactsToRemove)
                {
                    repository.DeletePersonModeOfContact(contactToRemove);
                }
            }

            // Now check for PersonContactTimes
            if (model.SelectedPreferredContactTimeIds != null && model.SelectedPreferredContactTimeIds.Any())
            {
                // conditional result was true, something was selected
                // if a PCD was NOT created above, create it now because the PersonContactDetails.PersonContactDetailId is a foreign key needed in PersonContactTime
                if (model.PersonContactDetail.PersonContactDetailId == 0)
                {
                    model.PersonContactDetail.PersonId = model.Person.PersonId;
                    model.PersonContactDetail.LastModified = DateTime.Now;
                    repository.AddPersonContactDetail(model.PersonContactDetail);
                }

                // Retrieve the current PersonModeOfContacts from the database
                var currentPersonContactTimes = GetPersonContactTimes(model.PersonContactDetail.PersonContactDetailId) ?? new List<PersonContactTime>();

                // Create a list of new and updated PersonModeOfContacts based on the selected IDs
                var newPersonContactTimes = model.SelectedPreferredContactTimeIds.Select(id => new PersonContactTime
                {
                    PersonContactDetailId = model.PersonContactDetail.PersonContactDetailId,
                    ContactTimeId = id,
                    LastModified = DateTime.Now
                }).ToList();

                // Determine the PersonModeOfContacts to be added or updated
                foreach (var newPersonContactTime in newPersonContactTimes)
                {
                    var existingContact = currentPersonContactTimes.FirstOrDefault(c => c.ContactTimeId == newPersonContactTime.ContactTimeId);

                    if (existingContact == null)
                    {
                        // Add new contact if it doesn't exist in the current contacts
                        repository.AddPersonContactTime(newPersonContactTime);
                    }
                }

                // Determine the PersonModeOfContacts to be removed
                var contactsToRemove = currentPersonContactTimes.Where(c => !model.SelectedPreferredContactTimeIds.Contains(c.ContactTimeId)).ToList();
                foreach (var contactToRemove in contactsToRemove)
                {
                    repository.DeletePersonContactTime(contactToRemove);
                }

            }
            // end of the PersonContactTimes section

            // Now check for PersonRaces
            if (model.SelectedRaceIds != null && model.SelectedRaceIds.Any())
            {
                // conditional result was true, something was selected
                // Retrieve the current PersonRaces from the database
                var currentPersonRaces = repository.PersonRaces
                    .Include(pr => pr.Race)
                    .Where(pr => pr.PersonId == model.Person.PersonId)
                    .ToList() ?? new List<PersonRace>();

                // Create a list from the model of new and updated PersonRacess based on the selected IDs
                var newPersonRaces = model.SelectedRaceIds.Select(id => new PersonRace
                {
                    RaceId = id,
                    PersonId = model.Person.PersonId,
                    LastModified = DateTime.Now
                }).ToList();

                // Determine the PersonRacess to be added
                foreach (var newPersonRace in newPersonRaces)
                {
                    var existingRace = currentPersonRaces.FirstOrDefault(c => c.RaceId == newPersonRace.RaceId);
                    if (existingRace == null)
                    {
                        // Add new contact if it doesn't exist in the current contacts
                        repository.AddPersonRace(newPersonRace);
                    }
                }
                // Determine the PersonRaces to be removed
                var racesToRemove = currentPersonRaces.Where(c => !model.SelectedRaceIds.Contains(c.RaceId)).ToList();
                foreach (var raceToRemove in racesToRemove)
                {
                    repository.DeletePersonRace(raceToRemove);
                }
            }
            else
            {
                // the model doesn't have any SelectedRaceIds, so check for existing and remove them
                var currentPersonRacesToRemove = repository.PersonRaces
                    .Include(pr => pr.Race)
                    .Where(pr => pr.PersonId == model.Person.PersonId)
                    .ToList() ?? new List<PersonRace>();
                if(currentPersonRacesToRemove != null && currentPersonRacesToRemove.Any())
                {
                    repository.DeletePersonRace(currentPersonRacesToRemove);
                }
            }

            // Now check for PersonLanguages            
            if(model.SelectedLanguageIds != null && model.SelectedLanguageIds.Any())
            {
                // conditional result was true, something was selected
                // Retrieve the current PersonLanguages from the database
                var currentPersonLanguages = repository.PersonLanguages
                    .Include(pl => pl.Language)
                    .Where(pl => pl.PersonId == model.Person.PersonId)
                    .ToList() ?? new List<PersonLanguage>();

                // Create a list from the model of new and updated PersonLanguages based on the selected IDs
                var newPersonLanguages = model.SelectedLanguageIds.Select(id => new PersonLanguage
                {
                    LanguageId = id,
                    PersonId = model.Person.PersonId,
                    IsPrimary = 1,
                    LastModified = DateTime.Now
                }).ToList();

                // Determine the PersonLanguages to be added
                foreach (var newPersonLanguage in newPersonLanguages)
                {
                    var existingPersonLanguage = currentPersonLanguages.FirstOrDefault(x => x.LanguageId == newPersonLanguage.LanguageId);
                    if(existingPersonLanguage == null)
                    {
                        // the object doesn't exist so create it
                        repository.AddPersonLanguage(newPersonLanguage);
                    }
                }

                // Determine the PersonLanguages to be removed
                var personLanguagesToRemove = currentPersonLanguages.Where(x => !model.SelectedLanguageIds.Contains(x.LanguageId)).ToList();
                repository.DeletePersonLanguage(personLanguagesToRemove);
            }
            else
            {
                // the model doesn't have any SelectedLanguageIds, so check for existing and remove them
                var currentPersonLanguagesToRemove = repository.PersonLanguages
                    .Include(pl => pl.Language)
                    .Where(pl => pl.PersonId == model.Person.PersonId)
                    .ToList() ?? new List<PersonLanguage>();
                if(currentPersonLanguagesToRemove != null && currentPersonLanguagesToRemove.Any())
                {
                    repository.DeletePersonLanguage(currentPersonLanguagesToRemove);
                }
            }

            // now take the user to the details view
            // the requestId is needed if the user was routed here from a Request
            return RedirectToAction("ViewPatientDetails", new { id = model.Patient.Mrn, requestId = model.RequestId });

        }
        // End of CreatePatient() setter method

        ///<summary
        /// Edit Patient - Getter
        ///</summary>
        [Authorize]
        [PermissionAuthorize("PatientEdit")]
        public async Task<IActionResult>EditPatient(string id, int? requestId)
        {
            // this boolean toggles the display of certain UI things between the creating a patient and editing a patient
            //  as both views employ CreatePatient.cshtml;  it is false so the Edit Patient UI features are rendered
            ViewBag.CreatingPatient = false;

            // this boolean toggles the display of certain UI things which are specific to the Details view and is also
            //  employed within the CreatePatient.cshtml as well as within all partial.cshtml views;
            //  it is false so the details UI will not be rendered
            ViewBag.ViewPatientDetails = false;

            // this boolean is used to suppress certain UI things when the view is opened in a modal;  it should always be false here because the EditPatient should not occur inside a modal
            ViewBag.IsModal = false;

            // this list is used only in Edit mode, only if the User is an Administrator (controlled in the cshtml), to assign any Facility
            ViewBag.Facilities = new SelectList(repository.Facilities, "FacilityId", "Name");

            // initializing the PatientViewModel utilizing a separate method
            var model = await InitializePatientViewModel(id, requestId);

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // returning the CreatePatient.cshtml along with the model because the user interface
            // for both is the same except for a couple display things near the top of the page
            //   if the "CreatePatient" portion is omitted, the controller looks for a view with the
            //   same name as this method, which does not exist and will throw an exception
            return View("CreatePatient", model);
        }
        // End of EditPatient getter


        /// <summary>
        ///     View the details of a Patient - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PatientView")]
        public async Task<IActionResult> ViewPatientDetails(string id, bool isModal, int? requestId)
        {
            // this boolean toggles the display of certain UI things between the creating a patient and editing a patient
            //  as both views employ CreatePatient.cshtml;  it is false so the Edit Patient UI features are rendered
            ViewBag.CreatingPatient = false;

            // this boolean toggles the display of certain UI things which are specific to the Details view and is also
            //  employed within the CreatePatient.cshtml as well as within all partial.cshtml views;
            //  it is false so the details UI will not be rendered
            ViewBag.ViewPatientDetails = true;

            // this boolean is used to suppress certain UI things when the view is opened in a modal;  in this method it is being passed in from an ajax call
            ViewBag.IsModal = isModal;

            // identify the User's Facility to enable showing only those Encounters
            var currentUser = repository.UserTables.FirstOrDefault(u => u.Email == User.Identity.Name);
            var currentUserFacility = repository.UserFacilities.FirstOrDefault(e => e.UserId == currentUser.UserId);
            ViewBag.CurrentUserFacilityId = currentUserFacility.FacilityId;

            // initializing the PatientViewModel utilizing a separate method
            var model = await InitializePatientViewModel(id, requestId);

            // Get the list of facilities for the given userId
            var facilities = await repository.UserFacilities
                                    .Where(uf => uf.UserId == currentUser.UserId)
                                    .Select(uf => uf.FacilityId)
                                    .ToListAsync();

            // Get the list of encounters filtered by the facilities for the given userId
            var patientEncounters = await repository.Encounters
                             .Where(e => e.Mrn == model.Patient.Mrn && facilities.Contains(e.FacilityId))
                             .Include(e => e.Facility)
                             .ToListAsync();

            ViewBag.PatientEncounters = patientEncounters;

            // set a boolean to indicate if this patient has an open Encounter
            bool openEncounter = repository.Encounters.Where(e => e.DischargeDateTime == null && e.Mrn == model.Patient.Mrn).Any();
            ViewBag.OpenEncounter = openEncounter;

            // set a boolean to indicate if this patient is deceased
            bool patientDeceased = model.Person.DeceasedLiving;
            ViewBag.PatientDeceased = patientDeceased;

            // An Admin person should see all of the encounters for a patient regardless of which facility
            var adminPatientEncounters = repository.Encounters
                            .Where(e => e.Mrn == model.Patient.Mrn)
                            .Include(e => e.Facility)
                            .ToList();
            ViewBag.AdminPatientEncounters = adminPatientEncounters;

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // returning the CreatePatient.cshtml along with the model because the user interface for both is the same except for a couple display things near the top of the page
            //   if the "CreatePatient" portion is omitted, the controller looks for a view with the same name as this method, which does not exist and will throw an exception
            return View("CreatePatient", model);
        }

        /// <summary>
        /// Deletes selected Patient and related records, either through MrnNavigation or via OnDelete behavior set in the Context.
        /// </summary>
        /// <param name="id">Id of unique patient to delete</param>
        // Used in: PatientDetails
        [Authorize]
        [PermissionAuthorize("PatientDelete")]
        public IActionResult DeletePatient(string id)
        {
            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            using var tran = new TransactionScope();
                
            var patient = repository.Patients
                    .Include(p => p.PatientAliases)
                    .Include(p => p.PatientAlerts)
                    .Include(p => p.PatientEmergencyContacts)
                    .Include(p => p.PatientInsurances)
                    .Include(p => p.PatientMedicationLists)
                    .Include(p => p.Person)
                        .ThenInclude(p => p.Employment)
                            .ThenInclude(ea => ea.Address)
                    .Include(p => p.Person)
                        .ThenInclude(p => p.PersonContactDetail)
                    .Include(p => p.Person)
                        .ThenInclude(p => p.PersonLanguages)
                    .Include(p => p.Person)
                        .ThenInclude(p => p.PersonRaces)
                    .FirstOrDefault(p => p.Mrn == id);

            var personId = patient.PersonId;

            if (patient == null)
            {
                return NotFound();
            }

            try
            {
                // delete Emergency Contact addresses;  Emergency contact will delete when patient deletes
                List<PatientEmergencyContact> emergencyContacts = repository.PatientEmergencyContacts
                    .Where(c => c.Mrn == patient.Mrn)
                    .Include(c => c.ContactAddress)
                    .ToList();

                if (emergencyContacts != null)
                {
                    foreach (var emCon in emergencyContacts)
                    {
                        var emConAddress = repository.Addresses.FirstOrDefault(eca => eca.AddressId == emCon.ContactAddressId);
                        if (emConAddress != null)
                        {
                            repository.DeleteAddress(emConAddress);
                        }
                    }
                }

                // delete residence, mailing addresses;  pcd will delete when person deletes
                var personContactDetail = repository.PersonContactDetails
                        .Include(pcd => pcd.PersonModeOfContacts)
                        .Include(pcd => pcd.PersonContactTimes)
                        .FirstOrDefault(pcd => pcd.PersonId == personId);
                if (personContactDetail != null && personContactDetail.ResidenceAddressId != null)
                {
                    var residenceAddress = repository.Addresses.FirstOrDefault(ra => ra.AddressId == personContactDetail.ResidenceAddressId);
                    repository.DeleteAddress(residenceAddress);
                }
                if (personContactDetail != null && personContactDetail.MailingAddressId != null)
                {
                    var mailingAddress = repository.Addresses.FirstOrDefault(ma => ma.AddressId == personContactDetail.MailingAddressId);
                    repository.DeleteAddress(mailingAddress);
                }

                // Delete related PersonModeOfContacts
                if (personContactDetail != null)
                {
                    foreach (var pmc in personContactDetail.PersonModeOfContacts)
                    {
                        repository.DeletePersonModeOfContact(pmc);
                    }

                    // Delete related PersonContactTimes
                    foreach (var pct in personContactDetail.PersonContactTimes)
                    {
                        repository.DeletePersonContactTime(pct);
                    }
                }
                // end of person contact details section

                // delete Patient and these relatives:  Person, PatientInsurance, PatientAlias, PatientAlerts, PersonContactDetail, PersonLanguage, PatientMedicationList, PersonRace, Encounter (and all of its relatives), and Request (and all of its relatives), via OnDelete Behavior settings in the database.
                // As of 6/2/2025, not supporting only Newborn, Birth.
                repository.DeletePatient(patient);
                repository.DeletePerson(patient.Person);

                // Employment and its address
                var employment = repository.Employments.FirstOrDefault(e => e.EmploymentId == patient.Person.EmploymentId);

                if (employment != null && employment.AddressId != null)
                {
                    var employmentAddress = repository.Addresses.FirstOrDefault(a => a.AddressId == employment.AddressId);
                    repository.DeleteEmployment(employment);
                    repository.DeleteAddress(employmentAddress);
                }
                else if (employment != null)
                {
                    repository.DeleteEmployment(employment);
                }
                // End of Employment section



                // Commit transaction
                tran.Complete();
                TempData["PatientSuccess"] = "Patient successfully deleted.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Roll back transaction in case of errors
                tran.Dispose();
                TempData["PatientError"] = $"An exception occurred:  {ex.Message}";
                return RedirectToAction("ViewPatientDetails", "Patient", new { id = patient.Mrn, isModal = false });
            }
        }

        // This next section of code is private methods called by Patient methods   

        /// <summary>
        ///     Initialize the PatientViewModel 
        ///     Setting this up as a separate method allows it to be called by any other method.
        ///     Without this, all of the code here would be repeated in the EditPatient getter and ViewPatientDetails getter methods.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<PatientViewModel> InitializePatientViewModel(string id, int? requestId)
        {
            var patient = await _patientService.GetPatientAndPersonForPatientViewModelByMrnAsync(id);

            var person = patient.Person;

            Employment employment = (person.EmploymentId != null) ? repository.Employments.FirstOrDefault(e => e.EmploymentId == person.EmploymentId) : null;
            Address employmentAddress = (employment != null) ? repository.Addresses.FirstOrDefault(ea => ea.AddressId == employment.AddressId) : null;

            List<PersonLanguage> personLanguages = repository.PersonLanguages
                .Include(r => r.Language)
                .Where(r => r.PersonId == person.PersonId)
                .ToList();

            List<PatientEmergencyContact> emergencyContacts = await repository.PatientEmergencyContacts
                .Where(c => c.Mrn == patient.Mrn)
                .Include(c => c.ContactRelationship)
                .Include(c => c.ContactAddress)
                .ToListAsync();

            PatientInsurance primaryInsurance = repository.PatientInsurances.Where(pi => pi.InsuranceOrder == 0).FirstOrDefault(pi => pi.MRN == patient.Mrn);
            PatientInsurance secondaryInsurance = repository.PatientInsurances.Where(pi => pi.InsuranceOrder == 1).FirstOrDefault(pi => pi.MRN == patient.Mrn);
            PatientInsurance tertiaryInsurance = repository.PatientInsurances.Where(pi => pi.InsuranceOrder == 2).FirstOrDefault(pi => pi.MRN == patient.Mrn);

            PersonContactDetail personContactDetail = repository.PersonContactDetails
                .Include(pcd => pcd.PersonModeOfContacts)
                    .ThenInclude(pmc => pmc.PreferredModeOfContact)
                .Include(pcd => pcd.PersonContactTimes)
                    .ThenInclude(pct => pct.PreferredContactTime)
                .FirstOrDefault(pcd => pcd.PersonId == person.PersonId);

            Address residenceAddress = (personContactDetail != null) ? repository.Addresses.FirstOrDefault(ra => ra.AddressId == personContactDetail.ResidenceAddressId) : null;
            Address mailingAddress = (personContactDetail != null) ? repository.Addresses.FirstOrDefault(ma => ma.AddressId == personContactDetail.MailingAddressId) : null;

            // Set a boolean for all of the circumstances of the PatientViewModel property 'ResidenceSameAsMailing':
            // The order of these checks is important:  trying to check an id on an entity which is null will throw an exception
            bool IsSame = true;
            if (mailingAddress == null && residenceAddress == null) { IsSame = true; }
            else if (mailingAddress != null && residenceAddress == null) { IsSame = false; }
            else if (mailingAddress != null && mailingAddress.AddressId != residenceAddress.AddressId) { IsSame = false; }

            List<PatientAlias> patientAliases = repository.PatientAliases
                .Where(pa => pa.PatientMRN == patient.Mrn)
                .ToList();

            List<PersonRace> personRaces = repository.PersonRaces
                .Include(pr => pr.Race)
                .Where(pr => pr.PersonId == person.PersonId)
                .ToList();

            AddDropdowns();

            // initialize the view model
            var model = new PatientViewModel
            (
                patient, person, employment, employmentAddress, primaryInsurance, secondaryInsurance, tertiaryInsurance, personContactDetail, residenceAddress, mailingAddress, emergencyContacts, patientAliases, personLanguages, personRaces, personRaces?.Select(pr => pr.RaceId).ToList() ?? new List<byte>(),personLanguages?.Select(pr => pr.LanguageId).ToList() ?? new List<short>())
            {
                SelectedPreferredModeOfContactIds = personContactDetail?.PersonModeOfContacts?.Select(pmc => pmc.ModeOfContactId).ToList() ?? new List<int>(),
                SelectedPreferredContactTimeIds = personContactDetail?.PersonContactTimes?.Select(pct => pct.ContactTimeId).ToList() ?? new List<int>()
            };

            // set the boolean after the view model is created
            model.ResidenceSameAsMailing = IsSame;
            // if routed here from a request, identify it and initialize the property in the view model
            model.RequestId = (requestId != null) ? repository.Requests.FirstOrDefault(r => r.RequestId == requestId)?.RequestId : (int?)null;

            return model;
        }

        /// <summary>
        ///     Patient:  Private method for checking for user entries in any Insurance field
        /// </summary>
        /// <param name="currentInstance"></param>
        /// <param name="mrn"></param>
        /// <returns></returns>
        private PatientInsurance CreateOrEditPatientInsurance(PatientInsurance currentInstance, byte index, string mrn)
        {
            if (UtilityHelper.IsAnyPropertyNotNullOrNonZeroExcludingDefaults(currentInstance, _logger,
                ("InsuranceOrder", index),
                ("PatientInsuranceId", 0),
                ("EffectiveDate", DateTime.MinValue)))
            {
                // UtilityHelper-currentInstance is true, so create or update the record
                currentInstance.MRN = mrn;

                // see if the record exists, if it does, just update it, if it doesn't, create it
                if (currentInstance.PatientInsuranceId != 0)
                {
                    repository.EditPatientInsurance(currentInstance);
                }
                else
                {
                    repository.AddPatientInsurance(currentInstance);
                }

            }
            return null;
        }

        /// <summary>
        ///     Patient: private method for mapping an Address object
        /// </summary>
        /// <param name="currentInstance"></param>
        /// <returns></returns>
        private Address MapAddress(Address currentInstance)
        {
            // If there is an AddressId, just edit the instance, otherwise create one
            if (currentInstance.AddressId != 0)
            {
                currentInstance.LastModified = DateTime.Now;
                repository.EditAddress(currentInstance);
                return currentInstance;
            }
            else
            {
                var newAddress = new Address
                {
                    Address1 = currentInstance.Address1,
                    Address2 = (currentInstance.Address2 == null) ? null : currentInstance.Address2,
                    PostalCode = currentInstance.PostalCode,
                    City = currentInstance.City,
                    County = (currentInstance.County == null) ? null : currentInstance.County,
                    AddressStateID = currentInstance.AddressStateID,
                    CountryId = (currentInstance.CountryId == 0) ? 1 : currentInstance.CountryId,
                    LastModified = DateTime.Now
                };
                repository.AddAddress(newAddress);
                return newAddress;
            }
        }

        /// <summary>
        ///     Retrieve a list of PersonModeOfContacts from the database
        /// </summary>
        /// <param name="personContactDetailId"></param>
        /// <returns>list of PersonModeOfContacts</returns>
        private List<PersonModeOfContact> GetPersonModeOfContacts(long personContactDetailId)
        {
            return repository.PersonModeOfContacts
                .Where(p => p.PersonContactDetailId == personContactDetailId)
                .ToList();
        }

        /// <summary>
        ///     Retrieve a list of PersonContactTimes from the database
        /// </summary>
        /// <param name="personContactDetailId"></param>
        /// <returns>list of PersonContactTimes</returns>
        private List<PersonContactTime> GetPersonContactTimes(long personContactDetailId)
        {
            return repository.PersonContactTimes
                .Where(p => p.PersonContactDetailId == personContactDetailId)
                .ToList();
        }

        private List<SelectListItem> GetUserFacilities()
        {
            var aspNetUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = repository.UserTables.SingleOrDefault(u => u.AspNetUsersId == aspNetUsersId);

            // Retrieve only the Facilities associated with the user
            var userFacilities = repository.UserFacilities
                .Where(f => f.UserId == user.UserId)
                .Select(f => new SelectListItem
                {
                    Value = f.FacilityId.ToString(), // FacilityID as the value
                    Text = f.Facility.Name           // Facility Name as the display text
                })
                .ToList();
            return userFacilities;
        }

        /// <summary>
        ///     Renders the Patient Alias partial via an ajax call initiated by a button click within the _DemographicsPartial.cshtml
        /// </summary>
        /// <param name="index"></param>
        /// <param name="viewPatientDetails"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public IActionResult GetAliasPartial(int indexPa, bool viewPatientDetails)
        {
            var viewModel = new PatientAlias();
            ViewData["indexPA"] = indexPa;
            ViewBag.ViewPatientDetails = viewPatientDetails;
            return PartialView("_AliasPartial", viewModel);
        }

        /// <summary>
        ///     Renders the Emergency Contact Partial via an ajax call initiated by a button click within the _EmergencyContactPartial.cshtml
        /// </summary>
        /// <param name="index"></param>
        /// <param name="viewPatientDetails"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public IActionResult AddEmergencyContact(int index, bool viewPatientDetails)
        {
            // create a new instance of PatientEmergencyContact
            var newContact = new PatientEmergencyContact();

            // Fetch the state and countries lists and make them available to the added instances
            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(repository);
            ViewData["Countries"] = AddressHelper.GetCountries(repository);

            // recreate the Relationships dropdown and make it available to the add instances
            var otherOptionNames = new[] { "Other" };
            var queryRelationships = repository.Relationships
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.RelationshipId, r.Name })
                .ToList();
            ViewBag.Relationships = new SelectList(queryRelationships, "RelationshipId", "Name");

            // carry through our view data
            ViewData["indexEC"] = index;
            ViewBag.ViewPatientDetails = viewPatientDetails;
            return PartialView("_EmergencyContactPartial", newContact);
        }

        // End of section for Patient private methods
        #endregion // end of entire Patient region

        #region Medications
        /// <summary>
        /// Lists the medications of the patient matching the parameter id.
        /// </summary>
        /// <param name="id">Id of patient whose medications are displayed</param>
        /// <param name="encounterId">Id of the encounter that the user came from. Used to allow the user to return to the original encounter.</param>
        // Used in: AddPatient, AddPatientButton
        [Authorize]
        [PermissionAuthorize("MedicationListView")]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task<IActionResult> ListMedications(string id, string? encounterId)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            ViewBag.returnToEncouterId = encounterId;
            
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(id);

            var PatientMedicationList = repository.PatientMedicationLists
                .Include(p => p.Medications).ThenInclude(p => p.MedicationGenericName)
                .Include(p => p.Medications).ThenInclude(p => p.MedicationBrandName)
                .Include(p => p.MedicationFrequencies).Include(p => p.Physicians)
                .Include(p => p.Medications).ThenInclude(p => p.MedicationDeliveryRoute)
                .Where(p => p.Mrn == id);

            var vm = new PatientMedicationListViewModel
            {
                Mrn = id,
                ActiveMeds = PatientMedicationList
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.Medications.MedicationGenericName.GenericName)
                    .ToList(),
                InactiveMeds = PatientMedicationList
                    .Where(i => !i.IsActive)
                    .OrderBy(i => i.Medications.MedicationGenericName.GenericName)
                    .ToList()
            };
            
            // Detect AJAX request ie modal
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Medication/_ListMedicationsPartial", vm); // partial contains only the fragment for modal
            }

            return View("Medication/ListMedications", vm);
        }

        /// <summary>
        ///     Create a new Patient Medication - Getter
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("MedicationListAdd")]
        public IActionResult CreatePatientMedication(string mrn)
        {
            var model = new PatientMedicationViewModel(mrn);
            model.PatientMedication.StartDate = DateTime.Now;
            PopulatePatientMedicationVm(model);

            // Detect AJAX request ie modal
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Medication/_CreatePatientMedicationPartial", model); // partial contains only the fragment for modal
            }

            return View("Medication/CreatePatientMedication", model);
        }

        /// <summary>
        ///     Create a new Patient Medication - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("MedicationListAdd")]
        public IActionResult CreatePatientMedication(PatientMedicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // restore lists and CurrentPatient so validation messages and selects render correctly
                model.PatientMedication.StartDate = DateTime.Now;
                PopulatePatientMedicationVm(model);

                // If this was an AJAX request, return the partial so the modal gets updated HTML
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // return the partial view HTML that the modal will inject
                    return PartialView("Medication/_CreatePatientMedicationPartial", model);
                }

                // non-AJAX: return the full view so validation messages show
                return View("Medication/CreatePatientMedication", model);
            }

            PatientMedicationList patientMedication = model.PatientMedication;
            if (patientMedication.EndDate.HasValue && patientMedication.StartDate > patientMedication.EndDate)
            {
                patientMedication.EndDate = patientMedication.StartDate;
            }
            patientMedication.IsActive = true;
            patientMedication.LastModified = DateTime.Now;
            repository.AddPatientMedicationList(patientMedication);

            // Success: different responses for AJAX vs normal post
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // return redirectUrl — your modal script will fetch that HTML into modal
                return Json(new { success = true, redirectUrl = Url.Action("ListMedications", new { id = patientMedication.Mrn }) });
            }

            // Non-AJAX: redirect to ListAlerts in the normal way
            return RedirectToAction("ListMedications", new { id = patientMedication.Mrn });
        }

        private void PopulatePatientMedicationVm(PatientMedicationViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // Ensure PatientMedication exists so model binding updates it on POST
            model.PatientMedication ??= new PatientMedicationList();

            // Current patient
            model.CurrentPatient = repository.Patients
                .Where(p => p.Mrn == model.PatientMedication.Mrn)
                .Include(p => p.PatientAlerts)
                .FirstOrDefault();

            // Select lists
            model.MedicationsAvailable = repository.Medications
                .Where(m => m.IsActive)
                .Include(m => m.MedicationGenericName)
                .Include(m => m.MedicationDosageForm)
                .Include(m => m.MedicationBrandName)
                .OrderBy(m => m.MedicationGenericName.GenericName)
                .Select(m => new SelectListItem
                {
                    Value = m.MedicationId.ToString(),
                    Text = m.MedicationGenericName.GenericName + " " + m.ActiveStrengthUnits + " " + m.MedicationDosageForm.DosageForm + " => " + m.MedicationBrandName.BrandName
                })
                .ToList();

            model.MedicationFrequencies = repository.MedicationFrequencies
                .Select(a => new SelectListItem
                {
                    Value = a.MedicationFrequencyId.ToString(),
                    Text = a.FrequencyDescription
                })
                .ToList();

            model.Providers = repository.Physicians
                .Select(a => new SelectListItem
                {
                    Value = a.PhysicianId.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToList();
        }

        /// <summary>
        /// Displays details of selected patient medication
        /// </summary>
        /// <param name="id">Id of patient medication to display</param>
        [Authorize]
        [PermissionAuthorize("MedicationListView")]
        public IActionResult PatientMedicationDetails(long id)
        {
            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            PatientMedicationList patientMedication = repository.PatientMedicationLists
                .Include(pml => pml.Medications).ThenInclude(med => med.MedicationDosageForm)
                .Include(pml => pml.Medications).ThenInclude(med => med.MedicationBrandName)
                .Include(pml => pml.Medications).ThenInclude(med => med.MedicationGenericName)
                .Include(pml => pml.Patients).ThenInclude(patient => patient.PatientAlerts)
                .Include(pml => pml.Physicians)
                .Include(pml => pml.MedicationFrequencies)
                .FirstOrDefault(pml => pml.PatientMedicationListID == id);

            // Detect AJAX request ie modal
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("Medication/_DetailsPatientMedicationPartial", patientMedication);
            }

            return View("Medication/DetailsPatientMedication", patientMedication);
        }

        /// <summary>
        ///     Edit a current Patient Medication - Getter
        /// </summary>
        /// <param name="id">Id of patient medication to edit</param>
        [Authorize]
        [PermissionAuthorize("MedicationListEdit")]
        public IActionResult EditPatientMedication(long id)
        {
            PatientMedicationList patientMedication = repository.PatientMedicationLists
                .Include(pml => pml.Patients).ThenInclude(patient => patient.PatientAlerts)
                .Include(pml => pml.Medications)
                .Include(pml => pml.MedicationFrequencies)
                .FirstOrDefault(pml => pml.PatientMedicationListID == id);

            PatientMedicationViewModel model = new(patientMedication)
            {
                CurrentPatient = repository.Patients
                    .Include(pa => pa.PatientAlerts)
                    .FirstOrDefault(p => p.Mrn == patientMedication.Mrn),
                MedicationsAvailable = repository.Medications
                    .Where(med => med.IsActive)
                    .Include(med => med.MedicationGenericName)
                    .Include(med => med.MedicationDosageForm)
                    .Include(med => med.MedicationBrandName)
                    .OrderBy(med => med.MedicationGenericName.GenericName)
                    .Select(med =>
                        new SelectListItem
                        {
                            Value = med.MedicationId.ToString(),
                            Text = med.MedicationGenericName.GenericName + " " + med.ActiveStrengthUnits + " " + med.MedicationDosageForm.DosageForm + " => " + med.MedicationBrandName.BrandName
                        }).ToList(),
                MedicationFrequencies = repository.MedicationFrequencies
                    .Select(a =>
                        new SelectListItem
                        {
                            Value = a.MedicationFrequencyId.ToString(),
                            Text = a.FrequencyDescription
                        }).ToList(),
                Providers = repository.Physicians
                    .Select(a =>
                        new SelectListItem
                        {
                            Value = a.PhysicianId.ToString(),
                            Text = a.FirstName + " " + a.LastName
                        }).ToList()
            };

            // Detect AJAX request ie modal
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("Medication/_EditPatientMedicationPartial", model);
            }
            return View("Medication/EditPatientMedication", model);
        }

        /// <summary>
        ///     Edit a current Patient Medication - Setter
        /// </summary>
        /// <param name="id">Id of patient medication to edit</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("MedicationListEdit")]
        public IActionResult EditPatientMedication(PatientMedicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("_EditPatientMedicationPartial", model);
                return View("EditPatientMedication", model);
            }
            
            PatientMedicationList patientMedication = model.PatientMedication;

            // check if EndDate exists and if the EndDate has passed
            if (patientMedication.EndDate.HasValue && patientMedication.EndDate.Value.Date <= DateTime.Today)
            {
                // guard against StartDate occurring after EndDate
                if (patientMedication.StartDate > patientMedication.EndDate)
                {
                    patientMedication.EndDate = patientMedication.StartDate;
                }
                patientMedication.IsActive = false;
            }
            else
            {
                // EndDate is either null or in the future
                patientMedication.IsActive = true;
            }
            patientMedication.LastModified = DateTime.Now;
            repository.EditPatientMedicationList(patientMedication);

            // If AJAX (modal) return the details partial HTML so modal shows updated details
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Build PatientMedicationList from repository/entity data
                PatientMedicationList patientMed = repository.PatientMedicationLists
                .Include(pml => pml.Medications).ThenInclude(med => med.MedicationDosageForm)
                .Include(pml => pml.Medications).ThenInclude(med => med.MedicationBrandName)
                .Include(pml => pml.Medications).ThenInclude(med => med.MedicationGenericName)
                .Include(pml => pml.Patients).ThenInclude(patient => patient.PatientAlerts)
                .Include(pml => pml.Physicians)
                .Include(pml => pml.MedicationFrequencies)
                .FirstOrDefault(pml => pml.PatientMedicationListID == model.PatientMedication.PatientMedicationListID);

                return PartialView("Medication/_DetailsPatientMedicationPartial", patientMed);
            }

            // Non-AJAX flow: full redirect to details page
            return RedirectToAction("PatientMedicationDetails", new { id = patientMedication.PatientMedicationListID });
        }

        /// <summary>
        ///     Delete a Patient Medication
        /// </summary>
        /// <param name="id">Id of patient medication to delete</param>
        /// <param name="mrn">Id of the patient the specified medication belongs to</param>
        // Used in: ListMedications.cshtml
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("MedicationListDelete")]
        public IActionResult DeletePatientMedication(long id)
        {
            PatientMedicationList patientMedicationList = repository.PatientMedicationLists
                .Include(pml => pml.Patients)
                .FirstOrDefault(pml => pml.PatientMedicationListID == id);

            string mrn = patientMedicationList.Patients.Mrn;

            repository.DeletePatientMedicationList(patientMedicationList);

            // If called via AJAX from modal, return JSON that tells client to re-fetch the ListAlerts partial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var redirectUrl = Url.Action("ListMedications", "Patient", new { id= mrn });
                return Json(new { success = true, redirectUrl = redirectUrl });
            }

            // Fallback for full-page POST
            return RedirectToAction("ListMedications", new { id = mrn });
        }
        #endregion // end of Patient Medications section

        #region Alerts
        /// <summary>
        ///     View ListAlerts for selected MRN - Getter
        /// </summary>
        /// <param name="id">Id of unique patient</param>
        // Used in: PatientAlertDetails, ListAlerts, PatientBanner
        [Authorize]
        [PermissionAuthorize("AlertAdd", "AlertDelete", "AlertEdit", "AlertView")]
        public async Task<IActionResult> ListAlerts(string mrn, string encounterId = null)
        {
            if (string.IsNullOrWhiteSpace(mrn))
                return BadRequest();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // this toggles a route link in the _PatientMenu.cshtml
            ViewBag.returnToEncounterId = encounterId;

            // Pass the Mrn for the banners and partials
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            var alerts = await repository.PatientAlerts
                .Where(pa => pa.Mrn == mrn)
                .Include(pa => pa.AlertType)
                .Include(pa => pa.PatientAllergies)
                    .ThenInclude(paAll => paAll.MedicationGenericName)
                .Include(pa => pa.PatientAllergies)
                    .ThenInclude(paAll => paAll.Allergen)
                .Include(pa => pa.PatientAllergies)
                    .ThenInclude(paAll => paAll.Reaction)
                .Include(pa => pa.PatientFallRisks)
                    .ThenInclude(pfr => pfr.FallRisk)
                .Include(pa => pa.PatientRestrictions)
                    .ThenInclude(pr => pr.RestrictionType)
                .OrderByDescending(pa => pa.LastModified)
                .AsNoTracking()
                .ToListAsync();

            var items = alerts.Select(pa =>
            {
                string alertTypeName = pa.AlertType?.Name ?? "Unknown";
                string description = string.Empty;

                if (string.Equals(alertTypeName, "Allergy", StringComparison.OrdinalIgnoreCase))
                {
                    var allergy = pa.PatientAllergies.FirstOrDefault();
                    if (allergy != null)
                    {
                        if (allergy.MedicationGenericName != null && !string.IsNullOrWhiteSpace(allergy.MedicationGenericName.GenericName))
                        {
                            var name = allergy.MedicationGenericName.GenericName;
                            var idx = name.IndexOf(',');
                            var truncated = idx >= 0 ? name.Substring(0, idx).Trim() : name.Trim();
                            description = $"{truncated} => {allergy.Reaction?.Name}";
                        }
                        else
                        {
                            description = $"{allergy.Allergen?.AllergenName} => {allergy.Reaction?.Name}";
                        }
                    }
                }
                else if (string.Equals(alertTypeName, "Fall Risk", StringComparison.OrdinalIgnoreCase))
                {
                    description = pa.PatientFallRisks.FirstOrDefault()?.FallRisk?.Name ?? string.Empty;
                }
                else if (string.Equals(alertTypeName, "Restricted Access", StringComparison.OrdinalIgnoreCase))
                {
                    description = pa.PatientRestrictions.FirstOrDefault()?.RestrictionType?.Name ?? string.Empty;
                }
                else
                {
                    // Default description fallback: use comments or alert type description if present
                    description = pa.Comments ?? pa.AlertType?.Description ?? string.Empty;
                }

                return new AlertListItemViewModel
                {
                    PatientAlertId = pa.PatientAlertId,
                    AlertTypeName = alertTypeName,
                    Description = description,
                    StartDate = pa.StartDate,
                    EndDate = pa.EndDate,
                    IsActive = pa.IsActive ?? false
                };
            }).ToList();

            var vm = new AlertListViewModel
            {
                Mrn = mrn,
                ActiveAlerts = items.Where(i => i.IsActive).ToList(),
                InactiveAlerts = items.Where(i => !i.IsActive).ToList()
            };

            ViewData["Mrn"] = mrn;
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Alert/_ListAlertsPartial", vm); // partial contains only the fragment for modal
            }

            return View("Alert/ListAlerts", vm);

        }

        /// <summary>
        ///     View CreateAlert page - Getter
        /// </summary>
        /// <param name="id">Id of unique patient</param>
        // Used in: ListAlerts
        [Authorize]
        [PermissionAuthorize("AlertAdd")]
        public IActionResult CreateAlert(string id)
        {
            PopulateAlertSelectLists(id);
            ViewBag.myMrn = id;
            
            //ViewBags for Patient Banner at top of page
            ViewBag.Patient = _patientService.GetPatientBannerDataByMrnAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Alert/_AlertCreatePartial"); // partial contains only the fragment for modal
            }

            return View("Alert/AlertCreate");
        }

        /// <summary>
        ///     Create alert - Setter
        /// </summary>
        /// <param name="model">Alert model to be added to database</param>
        // Used in: CreateAlert
        [HttpPost]
        [ActionName("CreateAlert")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("AlertAdd")]
        public IActionResult CreateAlert(AlertCreateViewModel model)
        {
            // repopulate lists needed by the view in case we need to return (on error)
            PopulateAlertSelectLists(model.Mrn);

            if (!ModelState.IsValid)
            {
                // If this was an AJAX request, return the partial so the modal gets updated HTML
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // return the partial view HTML that the modal will inject
                    return PartialView("Alert/_AlertCreatePartial", model);
                }

                // non-AJAX: return the full view so validation messages show
                return View("AlertCreate", model);
            }

            // Model is valid: create entities and save
            string mrn = model.Mrn;

            PatientAlert pa = new PatientAlert
            {
                AlertTypeId = model.AlertTypeId,
                Mrn = model.Mrn,
                IsActive = model.IsActive ?? true,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Comments = model.Comments,
                LastModified = DateTime.Now
            };

            repository.AddAlert(pa);

            if (model.AlertTypeId == 5) // Fall Risk
            {
                PatientFallRisk pfr = new PatientFallRisk
                {
                    FallRiskId = model.FallRiskId,
                    PatientAlertId = pa.PatientAlertId,
                    LastModified = DateTime.Now
                };
                repository.AddPatientFallRisk(pfr);
            }
            else if (model.AlertTypeId == 3) // Restricted Access
            {
                PatientRestriction pr = new PatientRestriction
                {
                    RestrictionTypeId = model.RestrictionTypeId,
                    PatientAlertId = pa.PatientAlertId,
                    LastModified = DateTime.Now
                };
                repository.AddPatientRestriction(pr);
            }

            // Success: different responses for AJAX vs normal post
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // return redirectUrl — your modal script will fetch that HTML into modal
                return Json(new { success = true, redirectUrl = Url.Action("ListAlerts", new { mrn }) });
            }

            // Non-AJAX: redirect to ListAlerts in the normal way
            return RedirectToAction("ListAlerts", new { mrn });
        }

        // Helper method to populate ViewBag or model select lists used by the CreateAlert methods
        private void PopulateAlertSelectLists(string mrn)
        {
            ViewBag.AlertType = repository.AlertTypes
                .Where(a => a.AlertId != 4) // do not want 'Allergy' in this dropdown
                .OrderBy(a => a.Name).Select(a =>
                new SelectListItem
                {
                    Value = a.AlertId.ToString(),
                    Text = a.Name
                }).ToList();
            
            ViewBag.PatientFallRisk = repository.FallRisks.OrderBy(r => r.Name).Include(r => r.PatientFallRisks).Select(
                r =>
                    new SelectListItem
                    {
                        Value = r.FallRiskId.ToString(),
                        Text = r.Name
                    }).ToList();
            
            ViewBag.Restriction = repository.Restrictions.OrderBy(r => r.Name).Include(r => r.PatientRestrictions)
                .Select(r =>
                    new SelectListItem
                    {
                        Value = r.RestrictionId.ToString(),
                        Text = r.Name
                    }).ToList();
            ViewBag.Today = DateTime.Now;
            
            if (repository.PatientAlerts.FirstOrDefault(b => b.Mrn == mrn) == null)
            {
                ViewBag.MRN = mrn;
            }
            else
            {
                ViewBag.MRN = repository.PatientAlerts.FirstOrDefault(b => b.Mrn == mrn).Mrn;
            }
        }


        /// <summary>
        ///     View PatientAlertDetails page - Getter
        /// </summary>
        /// <param name="id">Id of unique alert</param>
        /// <param name="mrn">Mrn of unique patient</param>
        // Used in: ListAlerts
        [Authorize]
        [PermissionAuthorize("AlertView")]
        public async Task<IActionResult> AlertDetails(long id, string mrn)
        {
            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // Pass the Mrn for the banners and partials
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            var pa = await repository.PatientAlerts
                .Where(x => x.PatientAlertId == id)
                .Include(x => x.AlertType)
                .Include(x => x.PatientAllergies)
                    .ThenInclude(a => a.MedicationGenericName)
                .Include(x => x.PatientAllergies)
                    .ThenInclude(a => a.Allergen)
                .Include(x => x.PatientAllergies)
                    .ThenInclude(a => a.Reaction)
                .Include(x => x.PatientFallRisks)
                    .ThenInclude(fr => fr.FallRisk)
                .Include(x => x.PatientRestrictions)
                    .ThenInclude(r => r.RestrictionType)
                .Include(x => x.PatientAdvancedDirectives)
                    .ThenInclude(ad => ad.AdvancedDirective)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (pa == null) return NotFound();

            var vm = new AlertDetailsViewModel
            {
                PatientAlertId = pa.PatientAlertId,
                Mrn = pa.Mrn,
                AlertTypeName = pa.AlertType?.Name ?? "Unknown",
                AlertTypeDescription = pa.AlertType?.Description,
                StartDate = pa.StartDate,
                EndDate = pa.EndDate,
                IsActive = pa.IsActive ?? false,
                Comments = pa.Comments,
                LastModified = pa.LastModified
            };
            
            switch (vm.AlertTypeName)
            {
                case "Allergy":
                    var allergy = pa.PatientAllergies.FirstOrDefault();
                    if (allergy != null)
                    {
                        vm.AllergenName = allergy.MedicationGenericName?.GenericName ?? allergy.Allergen?.AllergenName;
                        vm.ReactionName = allergy.Reaction?.Name;
                    }
                    break;

                case "Fall Risk":
                    vm.FallRiskName = pa.PatientFallRisks.FirstOrDefault()?.FallRisk?.Name;
                    break;

                case "Restricted Access":
                    vm.RestrictionName = pa.PatientRestrictions.FirstOrDefault()?.RestrictionType?.Name;
                    break;
            }

            // Detect AJAX request ie modal
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("Alert/_AlertDetailsPartial", vm);
            }

            return View("Alert/AlertDetails", vm);
        }

        /// <summary>
        ///     Edit an existing PatientAlert - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("AlertEdit")]
        public async Task<IActionResult> Edit(long id, string mrn)
        {
            // Pass the Mrn for the banners and partials
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            var pa = await repository.PatientAlerts
                .Include(x => x.AlertType)
                .Include(x => x.PatientAllergies)
                    .ThenInclude(a => a.MedicationGenericName)
                .Include(x => x.PatientAllergies)
                    .ThenInclude(a => a.Allergen)
                .Include(x => x.PatientAllergies)
                    .ThenInclude(a => a.Reaction)
                .Include(x => x.PatientFallRisks)
                    .ThenInclude(fr => fr.FallRisk)
                .Include(x => x.PatientRestrictions)
                    .ThenInclude(r => r.RestrictionType)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PatientAlertId == id && x.Mrn == mrn);

            if (pa == null) return NotFound();

            var vm = new AlertEditViewModel
            {
                PatientAlertId = pa.PatientAlertId,
                Mrn = pa.Mrn,
                AlertTypeName = pa.AlertType?.Name ?? "Unknown",
                StartDate = pa.StartDate,
                EndDate = pa.EndDate,
                IsActive = pa.IsActive ?? false,
                LastModified = pa.LastModified,
                Comments = pa.Comments
            };

            if (vm.AlertTypeName == "Allergy")
            {
                var allergy = pa.PatientAllergies.FirstOrDefault();
                if (allergy != null)
                {
                    vm.AllergenName = allergy.MedicationGenericName?.GenericName ?? allergy.Allergen?.AllergenName;
                    vm.ReactionName = allergy.Reaction?.Name;
                }
            }
            else if (vm.AlertTypeName == "Fall Risk")
            {
                vm.FallRiskName = pa.PatientFallRisks.FirstOrDefault()?.FallRisk?.Name;
            }
            else if (vm.AlertTypeName == "Restricted Access")
            {
                vm.RestrictionName = pa.PatientRestrictions.FirstOrDefault()?.RestrictionType?.Name;
            }

            // Detect AJAX request ie modal
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("Alert/_AlertEditPartial", vm);
            }
            return View("Alert/AlertEdit", vm);
        }
       
        /// <summary>
        ///     Edit an existing PatientAlert - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("AlertEdit")]
        public async Task<IActionResult> Edit(AlertEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("_AlertEditPartial", model);
                return View("AlertEdit", model);
            }
                
            var pa = await repository.PatientAlerts.FirstOrDefaultAsync(x => x.PatientAlertId == model.PatientAlertId && x.Mrn == model.Mrn);
            if (pa == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return NotFound();
                return NotFound();
            }    

            pa.EndDate = model.EndDate;
            pa.Comments = model.Comments;
            pa.IsActive = model.EndDate.HasValue ? false : true;
            pa.LastModified = DateTime.Now;

            repository.EditAlert(pa);

            // If AJAX (modal) return the details partial HTML so modal shows updated details
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Build AlertDetailsViewModel manually from repository/entity data
                var detailsModel = new AlertDetailsViewModel
                {
                    PatientAlertId = pa.PatientAlertId,
                    Mrn = pa.Mrn,
                    AlertTypeName = pa.AlertType?.Name ?? "Unknown",
                    AlertTypeDescription = pa.AlertType?.Description,
                    StartDate = pa.StartDate,
                    EndDate = pa.EndDate,
                    IsActive = pa.IsActive ?? false,
                    Comments = pa.Comments,
                    LastModified = pa.LastModified
                };

                // Pass the Mrn for the banners and partials
                await _patientService.GetPatientBannerDataByMrnAsync(model.Mrn);

                return PartialView("Alert/_AlertDetailsPartial", detailsModel);
            }

            // Non-AJAX flow: full redirect to details page
            return RedirectToAction("AlertDetails", new { id = model.PatientAlertId, mrn = model.Mrn });
        }

        /// <summary>
        ///     Deletes Alert - Setter
        /// </summary>
        /// <param name="id">Id of unique alert</param>
        /// <param name="mrn">Mrn of unique patient</param>
        /// Used in: ListAlerts
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("AlertDelete")]
        public IActionResult DeleteAlert(long id, string mrn)
        {
            var alert = repository.PatientAlerts.FirstOrDefault(b => b.PatientAlertId == id);
            if (alert == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Alert not found." });
                return NotFound();
            }
            repository.DeleteAlert(alert);

            // If called via AJAX from modal, return JSON that tells client to re-fetch the ListAlerts partial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var redirectUrl = Url.Action("ListAlerts", "Patient", new { mrn = mrn });
                return Json(new { success = true, redirectUrl = redirectUrl });
            }

            // Fallback for full-page POST
            return RedirectToAction("ListAlerts", new { mrn = mrn });
        }

        /// <summary>
        ///     Getter for refreshing the ALERT indicator in the PatientBannerPartial when the alerts are modified via a modal
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("PatientView", "AlertView")]
        public IActionResult GetAlertSummary(string mrn)
        {
            if (string.IsNullOrWhiteSpace(mrn)) return BadRequest();

            // Load only what you need for the banner: active alert count
            var patient = repository.Patients.Include(p => p.PatientAlerts).FirstOrDefault(b => b.Mrn == mrn);
            if (patient == null) return NotFound();

            int totalAlerts = patient.PatientAlerts?.Count ?? 0;
            int activeAlerts = 0;
            if (patient.PatientAlerts != null)
            {
                foreach (var a in patient.PatientAlerts)
                {
                    if (!a.EndDate.HasValue) activeAlerts++;
                }
            }

            return Json(new
            {
                mrn = mrn,
                totalAlerts = totalAlerts,
                activeAlerts = activeAlerts,
                hasActive = activeAlerts > 0
            });
        }

        #endregion // end of Alerts region

        #region Allergies 
        /// <summary>
        ///     List the Patient Allergies - Getter
        /// </summary>
        /// <param name="mrn"></param>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("AllergyAdd", "AllergyDelete", "AllergyEdit", "AllergyView")]
        public async Task<IActionResult> ListAllergies(string mrn, string encounterId = null)
        {
            if (string.IsNullOrWhiteSpace(mrn))
                return BadRequest();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // this toggles a route link in the _PatientMenu.cshtml
            ViewBag.returnToEncounterId = encounterId;

            // Load patient for banner/menu
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            ViewData["Mrn"] = mrn;

            // Build list: only AllergenName and concatenated Description as requested
            var allergies = await repository.PatientAllergy
                .AsNoTracking()
                .Include(pa => pa.Allergen)
                .Include(pa => pa.Reaction)
                .Include(pa => pa.MedicationGenericName)
                .Include(pa => pa.PatientAlert)
                .Where(pa => pa.Mrn == mrn || (pa.PatientAlert != null && pa.PatientAlert.Mrn == mrn))
                .OrderBy(pa => pa.Allergen.AllergenName)
                .Select(pa => new AllergyListItemViewModel
                {
                    PatientAllergyId = pa.PatientAllergyId,
                    AllergenName = pa.Allergen != null ? pa.Allergen.AllergenName : "Unknown",
                    // Concatenate parts into a single description field, truncating the Medication Generic Name at the first comma
                    Description = (
                        pa.GenericMedicationId.HasValue && pa.MedicationGenericName != null
                            ? (pa.MedicationGenericName.GenericName.Contains(",")
                                ? pa.MedicationGenericName.GenericName.Substring(
                                    0,
                                    pa.MedicationGenericName.GenericName.IndexOf(",")
                                )
                                : pa.MedicationGenericName.GenericName)
                            + (pa.Reaction != null ? " => " + pa.Reaction.Name : "")
                            : (pa.Reaction != null ? pa.Reaction.Name : "")
                    ).Trim(),
                    IsActive = pa.IsActive,
                    StartDate = pa.StartDate,
                    EndDate = pa.EndDate != null ? pa.EndDate : null
                })
                .ToListAsync();

            var vm = new AllergyListViewModel
            {
                Mrn = mrn,
                ActiveAllergies = allergies.Where(i => i.IsActive).ToList(),
                InactiveAllergies = allergies.Where(i => !i.IsActive).ToList()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Allergy/_ListAllergiesPartial", vm); // partial contains only the fragment for modal
            }

            return View("Allergy/ListAllergies", vm);
        }

        /// <summary>
        ///     Create a new Patient Allergy - Getter
        /// </summary>
        /// <param name="mrn"></param>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("AllergyAdd")]
        public async Task<IActionResult> CreateAllergy(string mrn, string encounterId = null)
        {
            if (string.IsNullOrWhiteSpace(mrn))
                return BadRequest();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // this toggles a route link in the _PatientMenu.cshtml
            ViewBag.returnToEncounterId = encounterId;

            // Load patient for banner/menu
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            ViewData["Mrn"] = mrn;

            var vm = new CreateAllergyViewModel
            {
                Mrn = mrn,
                Allergens = await _allergenService.GetAllAllergensAsync(),
                Reactions = await repository.Reactions.OrderBy(r => r.Name).ToListAsync(),
                MedicationGenericNames = await repository.MedicationGenericNames.OrderBy(m => m.GenericName).ToListAsync()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Allergy/_CreateAllergyPartial", vm); // partial contains only the fragment for modal
            }

            return View("Allergy/CreateAllergy", vm);
        }

        /// <summary>
        ///     Create a new Patient Allergy - Setter
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("AllergyAdd")]
        public async Task<IActionResult> CreateAllergy(CreateAllergyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Allergens = await _allergenService.GetAllAllergensAsync();
                vm.Reactions = await repository.Reactions.OrderBy(r => r.Name).ToListAsync();
                vm.MedicationGenericNames = await repository.MedicationGenericNames.OrderBy(m => m.GenericName).ToListAsync();

                // If this was an AJAX request, return the partial so the modal gets updated HTML
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // return the partial view HTML that the modal will inject
                    return PartialView("Allergy/_CreateAllergyPartial", vm);
                }
                return View(vm);
            }

            // load reaction to check AlertRequired
            var reaction = await repository.Reactions
                .Where(r => r.ReactionId == vm.ReactionId)
                .FirstOrDefaultAsync();

            // Build PatientAllergy entity
            var allergy = new PatientAllergy
            {
                AllergenId = vm.AllergenId,
                ReactionId = vm.ReactionId,
                GenericMedicationId = vm.GenericMedicationId,
                StartDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsActive = true,
                // PatientAlertId left null by default; we will set it only if alert is required
            };

            using var tran = new TransactionScope();
            try
            {
                // check if the reaction requires a PatientAlert
                if (reaction?.AlertRequired == true)
                {
                    var alert = new PatientAlert
                    {
                        Mrn = vm.Mrn,
                        AlertTypeId = 4, // AlertTypeId for allergies
                        LastModified = DateTime.UtcNow,
                        IsActive = true,
                        StartDate = DateTime.UtcNow,
                        Comments = "Auto-created allergy alert"
                    };

                    repository.AddAlert(alert); // add a PatientAlert

                    allergy.PatientAlertId = alert.PatientAlertId;  // tie the PatientAlert to the PatientAllergy
                    repository.AddPatientAllergy(allergy);
                }
                else
                {
                    // if PatientAlert is not needed, directly post the new PatientAllergy
                    allergy.Mrn = vm.Mrn; // tie the Patient to the Allergy
                    repository.AddPatientAllergy(allergy);
                }

                // Commit transaction
                tran.Complete();

                // Success: different responses for AJAX vs normal post
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // return redirectUrl — your modal script will fetch that HTML into modal
                    return Json(new { success = true, redirectUrl = Url.Action("ListAllergies", new { vm.Mrn }) });
                }

                // Non-AJAX: redirect to ListAllergies in the normal way
                return RedirectToAction("ListAllergies", new { mrn = vm.Mrn });
            }
            catch
            {
                // Roll back transaction in case of errors
                tran.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Provide a Details view of existing Patient Allergy - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("AllergyView")]
        public async Task<IActionResult> DetailsAllergy(long id, string mrn)
        {
            if (string.IsNullOrWhiteSpace(mrn))
                return BadRequest();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // Pass the Mrn for the banners and partials
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            var pa = await repository.PatientAllergy
                .Where(x => x.PatientAllergyId == id)
                .Include(a => a.MedicationGenericName)
                .Include(a => a.Allergen)
                .Include(a => a.Reaction)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (pa == null) return NotFound();

            // create the truncated Generic Name for display purposes
            var genericTruncated = string.IsNullOrEmpty(pa.MedicationGenericName?.GenericName)
                ? pa.MedicationGenericName?.GenericName
                : (pa.MedicationGenericName.GenericName.Split(',', 2)[0].Trim());

            var model = new AllergyDetailsViewModel
            {
                // Ids and booleans (use pa values directly)
                PatientAllergyId = pa.PatientAllergyId,
                Mrn = mrn,
                IsActive = pa.IsActive,

                // Non-nullable DateTime properties on the view model:
                // if entity has nullable dates, provide a fallback (choose sensible default)
                StartDate = pa.StartDate,
                LastModified = pa.LastModified,

                // Nullable view-model properties mapped directly from entity (use null propagation)
                AllergenId = pa.AllergenId,
                ReactionId = pa.ReactionId,
                EndDate = pa.EndDate,
                Comments = pa.Comments,
                GenericMedicationId = pa.GenericMedicationId,
                PatientAlertId = pa.PatientAlertId,

                // Navigation properties may be null so use safe access
                AllergenName = pa.Allergen?.AllergenName,
                ReactionName = pa.Reaction?.Name,
                // build the display value depending upon the AllergenName
                AllergenDisplay = (pa.Allergen?.AllergenName != null
                        && pa.Allergen.AllergenName.Equals("Medication", StringComparison.OrdinalIgnoreCase)
                        && pa.MedicationGenericName != null)
                        ? $"{pa.Allergen.AllergenName} => {genericTruncated}"
                        : pa.Allergen?.AllergenName

            };


            // Detect AJAX request ie modal
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("Allergy/_DetailsAllergyPartial", model);
            }

            return View("Allergy/DetailsAllergy", model);
        }

        /// <summary>
        ///     Edit an existing Patient Allergy - getter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("AllergyEdit")]
        public async Task<IActionResult> EditAllergy(long id, string mrn)
        {
            if (string.IsNullOrWhiteSpace(mrn))
                return BadRequest();

            // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            // Pass the Mrn for the banners and partials
            ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(mrn);

            var pa = await repository.PatientAllergy
                .Where(x => x.PatientAllergyId == id)
                .Include(a => a.MedicationGenericName)
                .Include(a => a.Allergen)
                .Include(a => a.Reaction)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (pa == null) return NotFound();

            // create the truncated Generic Name for display purposes
            var genericTruncated = string.IsNullOrEmpty(pa.MedicationGenericName?.GenericName)
                ? pa.MedicationGenericName?.GenericName
                : (pa.MedicationGenericName.GenericName.Split(',', 2)[0].Trim());

            var model = new AllergyEditViewModel
            {
                // Ids and booleans (use pa values directly)
                PatientAllergyId = pa.PatientAllergyId,
                Mrn = mrn,
                IsActive = pa.IsActive,

                // Non-nullable DateTime properties on the view model:
                // if entity has nullable dates, provide a fallback (choose sensible default)
                StartDate = pa.StartDate,
                LastModified = pa.LastModified,

                // Nullable view-model properties mapped directly from entity (use null propagation)
                AllergenId = pa.AllergenId,
                ReactionId = pa.ReactionId,
                EndDate = pa.EndDate,
                Comments = pa.Comments,
                GenericMedicationId = pa.GenericMedicationId,
                PatientAlertId = pa.PatientAlertId,

                // Navigation properties may be null so use safe access
                AllergenName = pa.Allergen?.AllergenName,
                ReactionName = pa.Reaction?.Name,
                // build the display value depending upon the AllergenName
                AllergenDisplay = (pa.Allergen?.AllergenName != null
                        && pa.Allergen.AllergenName.Equals("Medication", StringComparison.OrdinalIgnoreCase)
                        && pa.MedicationGenericName != null)
                        ? $"{pa.Allergen.AllergenName} => {genericTruncated}"
                        : pa.Allergen?.AllergenName
            };


            // Detect AJAX request ie modal
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("Allergy/_EditAllergyPartial", model);
            }

            return View("Allergy/EditAllergy", model);
        }

        /// <summary>
        ///     Edit an existing Patient Allergy - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("AllergyEdit")]
        public async Task<IActionResult> EditAllergy(AllergyEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("_EditAllergyPartial", model);
                return View("EditAllergy", model);
            }

            var pa = await repository.PatientAllergy
                .Where(x => x.PatientAllergyId == model.PatientAllergyId)
                .Include(a => a.MedicationGenericName)
                .Include(a => a.Allergen)
                .Include(a => a.Reaction)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (pa == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return NotFound();
                return NotFound();
            }    

            pa.EndDate = model.EndDate;
            pa.Comments = model.Comments;

            if (model.EndDate.HasValue && model.EndDate.Value <= DateTime.Now)
            {
                pa.IsActive = false;
            }
            else
            {
                pa.IsActive = true;
            }

            pa.LastModified = DateTime.Now;
            repository.EditPatientAllergy(pa);

            // any associated Patient Alert needs updated as well
            if(pa.PatientAlertId.HasValue)
                {
                    var alertToUpdate = repository.PatientAlerts.FirstOrDefault(a => a.PatientAlertId == pa.PatientAlertId);
                    alertToUpdate.IsActive = pa.IsActive;
                    alertToUpdate.EndDate = pa.EndDate;
                    alertToUpdate.LastModified = pa.LastModified;
                    repository.EditAlert(alertToUpdate);
                }


            // If AJAX (modal) return the details partial HTML so modal shows updated details
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // create the truncated Generic Name for display purposes
                var genericTruncated = string.IsNullOrEmpty(pa.MedicationGenericName?.GenericName)
                    ? pa.MedicationGenericName?.GenericName
                    : (pa.MedicationGenericName.GenericName.Split(',', 2)[0].Trim());

                // Build AllergyDetailsViewModel manually from repository/entity data
                var detailsModel = new AllergyDetailsViewModel
                {
                    // Ids and booleans (use pa values directly)
                    PatientAllergyId = pa.PatientAllergyId,
                    Mrn = model.Mrn,
                    IsActive = pa.IsActive,

                    // Non-nullable DateTime properties on the view model:
                    // if entity has nullable dates, provide a fallback (choose sensible default)
                    StartDate = pa.StartDate,
                    LastModified = pa.LastModified,

                    // Nullable view-model properties mapped directly from entity (use null propagation)
                    AllergenId = pa.AllergenId,
                    ReactionId = pa.ReactionId,
                    EndDate = pa.EndDate,
                    Comments = pa.Comments,
                    GenericMedicationId = pa.GenericMedicationId,
                    PatientAlertId = pa.PatientAlertId,

                    // Navigation properties may be null so use safe access
                    AllergenName = pa.Allergen?.AllergenName,
                    ReactionName = pa.Reaction?.Name,
                    // build the display value depending upon the AllergenName
                    AllergenDisplay = (pa.Allergen?.AllergenName != null
                        && pa.Allergen.AllergenName.Equals("Medication", StringComparison.OrdinalIgnoreCase)
                        && pa.MedicationGenericName != null)
                        ? $"{pa.Allergen.AllergenName} => {genericTruncated}"
                        : pa.Allergen?.AllergenName
                };

                // Pass the Mrn for the banners and partials
                ViewBag.Patient = await _patientService.GetPatientBannerDataByMrnAsync(model.Mrn);

                return PartialView("Allergy/_DetailsAllergyPartial", detailsModel);
            }

            // Non-AJAX flow: full redirect to details page
            return RedirectToAction("DetailsAllergy", new { id = model.PatientAllergyId, mrn = model.Mrn });
        }

        /// <summary>
        ///     Delete a Patient Allergy from the database - setter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mrn"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("AllergyDelete")]
        public IActionResult DeleteAllergy(long id, string mrn)
        {
            var allergy = repository.PatientAllergy.FirstOrDefault(pa => pa.PatientAllergyId == id);

            if (allergy.PatientAlertId != null)
            {
                PatientAlert patientAlert = repository.PatientAlerts.FirstOrDefault(p => p.PatientAlertId == allergy.PatientAlertId);
                repository.DeleteAlert(patientAlert);
                repository.DeletePatientAllergy(allergy);
            }
            else
            {
                repository.DeletePatientAllergy(allergy);
            }

            // If called via AJAX from modal, return JSON that tells client to re-fetch the ListAllergies partial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var redirectUrl = Url.Action("ListAllergies", "Patient", new { mrn = mrn });
                return Json(new { success = true, redirectUrl = redirectUrl });
            }

            // Fallback for full-page POST
            return RedirectToAction("ListAllergies", new { mrn = mrn });
        }
        #endregion

        #region Dropdowns
        /// <summary>
        /// Add dropdowns to encounter views. Controller method to display dropdowns
        /// </summary>
        // [Authorize(Roles = "Administrator, HIT Faculty, HIMS Technician, HIT Clerk, Read Only, Registrar, Physician, Med Assist Faculty, Med Assist Student, Nursing Faculty, Nursing Student")]
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public void AddDropdowns(Patient model = null)
        {

            /*
             * Explanation:
             * otherOptionNames is the names of the fields that are along the lines of "Other", "Did not Disclose",
             * "Unknown", etc.
             * It was requested that these fields be at the bottom of the dropdown
             * The .OrderBy() filter takes the name of the record, and checks if it is in the otherOptionNames array
             * If it is, it is given a value of "2", which means it goes to the last half of the list
             * Otherwise, it is given a one, and put in the first half of the list
             * Within that constraint, .ThenBy() then puts all of the values in each half of the list in alphabetical
             * order
             *
             * Names are being compared instead of ids because, between the test, dev, and live database, the ids of
             * these records may change. This also gives a better idea of what is being moved to the bottom of
             * the list.
             */
            string[] otherOptionNames = {"Non-Denominational", "None/No Preference", "Not Asked",
                "Not Listed", "Other", "Unknown"};
            var queryReligion = repository.Religions
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.ReligionId, r.Name })
                .ToList();
            ViewBag.Religions = new SelectList(queryReligion, "ReligionId", "Name", 0);

            otherOptionNames = new[] { "Choose Not to Disclose", "Unreported" };
            var querySex = repository.Sexes
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.SexId, r.Name })
                .ToList();
            ViewBag.Sexes = new SelectList(querySex, "SexId", "Name", 0);

            otherOptionNames = new[] { "Other", "Choose Not to Disclose", "Unreported" };
            var queryGender = repository.Genders
                .OrderBy(r => otherOptionNames.Contains(r.Name))
                .ThenBy(r => r.Name)
                .Select(r => new { r.GenderId, r.Name })
                .ToList();
            ViewBag.Gender = new SelectList(queryGender, "GenderId", "Name", 0);

            otherOptionNames = new[] { "Other", "Choose Not to Disclose", "Unreported" };
            var queryGenderPronouns = repository.GenderPronouns
                .OrderBy(r => otherOptionNames.Contains(r.GenderPronouns))
                .ThenBy(r => r.GenderPronouns)
                .Select(r => new { r.GenderPronounId, r.GenderPronouns })
                .ToList();
            ViewBag.GenderPronouns = new SelectList(queryGenderPronouns, "GenderPronounId", "GenderPronouns", 0);

            otherOptionNames = new[] { "Choose Not to Disclose", "Unreported" };
            var queryEthnicity = repository.Ethnicities
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.EthnicityId, r.Name })
                .ToList();
            ViewBag.Ethnicity = new SelectList(queryEthnicity, "EthnicityId", "Name", 0);

            otherOptionNames = new[] { "Other", "Choose Not to Disclose", "Unreported" };
            var queryMaritalStatus = repository.MaritalStatuses
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.MaritalStatusId, r.Name })
                .ToList();
            ViewBag.MaritalStatus = new SelectList(queryMaritalStatus, "MaritalStatusId", "Name", 0);

            /*
             * This works similarly to the other queries, however in reverse
             * If it is the desired element, it is put at the front of the list no matter what
             */
            var english = "English";
            var queryLanguages = repository.Languages
                .OrderBy(r => r.Name == english ? 1 : 2)
                .ThenBy(r => r.Name)
                .Select(r => new { r.LanguageId, r.Name })
                .ToList();
            ViewBag.Languages = new SelectList(queryLanguages, "LanguageId", "Name", 0);

            otherOptionNames = new[] { "Choose Not to Disclose", "Unreported", "Other" };
            var queryRaces = repository.Races
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.RaceId, r.Name })
                .ToList();
            ViewBag.Races = new SelectList(queryRaces, "RaceId", "Name", 0);

            otherOptionNames = new[] { "Other" };
            var queryRelationships = repository.Relationships
                .OrderBy(r => otherOptionNames.Contains(r.Name) ? 2 : 1)
                .ThenBy(r => r.Name)
                .Select(r => new { r.RelationshipId, r.Name })
                .ToList();
            ViewBag.Relationships = new SelectList(queryRelationships, "RelationshipId", "Name");

            otherOptionNames = new[] { "Select Patient Status" };
            var queryLegalStatuses = repository.LegalStatuses
                .OrderBy(l => otherOptionNames.Contains(l.LegalStatusName) ? 2 : 1)
                .ThenBy(l => l.LegalStatusName)
                .Select(l => new { l.LegalStatusId, l.LegalStatusName })
                .ToList();
            ViewBag.LegalStatuses = new SelectList(queryLegalStatuses, "LegalStatusId", "LegalStatusName");

            ViewData["EducationId"] = new SelectList(repository.EducationLevels, "EducationId", "EducationLevelName");
            ViewData["InsuranceProviders"] = new SelectList(repository.InsuranceProviders, "InsuranceProviderId", "ProviderName");

            ViewData["StateId"] = AddressHelper.GetStatesWithWisconsinFirst(repository);
            ViewData["Countries"] = AddressHelper.GetCountries(repository);

            var queryPreferredModesOfContact = repository.PreferredModesOfContact
                .OrderBy(m => m.Name)
                .Select(m => new { m.ModeOfContactId, m.Name });
            ViewBag.PreferredModesOfContact = new SelectList(queryPreferredModesOfContact, "ModeOfContactId", "Name");

            var queryPreferredContactTimes = repository.PreferredContactTimes
                .OrderBy(t => t.Name).Select(t => new { t.ContactTimeId, t.Name });
            ViewBag.PreferredContactTimes = new SelectList(queryPreferredContactTimes, "ContactTimeId", "Name", 0);

        }
        #endregion // end of Dropdowns

        #region Patient Banner - Directives
        // returns 'true' is there is at least one directive;  used to toggle the 'Directives' button between enabled/disabled
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public JsonResult HasDirectives(string mrn)
        {
            bool hasAny = repository.Documents
                .Any(d => d.Mrn == mrn && d.DocumentTypeID == 2);

            // return true/false as JSON
            return Json(hasAny);
        }

        // Returns the list HTML for the modal body
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public IActionResult ListDirectives(string id)
        {
            var directives = repository.Documents
                .Include(dt => dt.DocumentType)
                .Where(d => d.Mrn == id && d.DocumentTypeID == 2)
                .OrderByDescending(d => d.CreatedAt)
                .ToList();

            return PartialView("_PatientDirectivesList", directives);
        }

        // Streams the document (e.g. PDF) for viewing
        [HttpGet]
        [Authorize]
        [PermissionAuthorize("PatientAdd", "PatientDelete", "PatientEdit", "PatientView")]
        public IActionResult ViewDirective(int docId)
        {
            var doc = repository.Documents.FirstOrDefault(d => d.DocumentID == docId);
            if (doc == null) return NotFound();

            string contentType = doc.FileType.ToLower() switch
            {
                "pdf" => "application/pdf",
                "jpeg" => "image/jpeg",
                "jpg" => "image/jpeg",
                "png" => "image/png",
                _ => "application/octet-stream"
            };

            return File(doc.DocumentContent, contentType);
        }
        #endregion

        #region Search for Patient
        // GET: /Patient/SearchPatients?q=smith
        [HttpGet]
        public async Task<IActionResult> SearchPatients(string q, int? take)
        {
            // determine facility from session
            var facilityIdString = HttpContext.Session.GetString("Facility");
            if (string.IsNullOrEmpty(facilityIdString) || !int.TryParse(facilityIdString, out var facilityId))
                return Unauthorized();

            var max = take ?? 25;
            var results = await _patientService.SearchPatientsAsync(facilityId, q ?? "", max);

            // return in Select2-friendly shape { id, text }
            var shaped = results.Select(r => new
            {
                id = r.Mrn,
                text = r.Display
            });

            return Json(new { results = shaped });
        }


        #endregion
    }
}