using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS_Proj_HIT.ViewModels.BirthRegistry;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;



namespace IS_Proj_HIT.Controllers
{

    public class BirthRegistryController(IWCTCHealthSystemRepository repository, IBirthRegistrySearchService searchService, IBirthRegistryDropdownDataService dropdownService, IBirthRegistryValidationService validationService) : Controller
    {
        private readonly IWCTCHealthSystemRepository _repository = repository;
        private readonly IBirthRegistrySearchService _searchService = searchService;
        private readonly IBirthRegistryDropdownDataService _dropdownService = dropdownService;
        private readonly IBirthRegistryValidationService _validationService = validationService;

        #region Main Actions

        public IActionResult Index()
        {

            return RedirectToAction("BirthRegistryArchive");
        }


        [HttpGet]
        public ViewResult Create()
        {
            var viewModel = new BirthRegistryViewModel
            {
                FacilityViewModel = new BirthFacilityViewModel(),
                MothersInformationViewModel = new MothersInformationViewModel(),
                FatherInformationViewModel = new FatherInformationViewModel(),
                PrenatalCareViewModel = new PrenatalCareViewModel(),
                LaborAndDeliveryViewModel = new LaborAndDeliveryViewModel(),
                NewbornViewModel = new NewbornViewModel(),
                FinalizeViewModel = new FinalizeViewModel()
            };

            LoadAllDropdownData(viewModel, null);

            ViewBag.Mode = "create";
            ViewBag.ActiveTab = "mother";

            return View("BirthRegistry", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id, string tab = "mother")
        {
            var birthRecord = GetBirthRegistryWithDetails(id);

            if (birthRecord == null)
            {
                TempData["ErrorMessage"] = $"Birth registry with ID {id} could not be found.";
                return RedirectToAction(nameof(BirthRegistryArchive));
            }

            var firstNewborn = birthRecord.Newborns?.FirstOrDefault();
            NewbornViewModel newbornViewModel;

            if (firstNewborn != null)
            {
                var fullNewborn = _repository.Newborns
                    .Include(n => n.NewbornMrnNavigation)
                    .Include(n => n.Birth)
                    .Include(n => n.AbnormalConditions)
                    .Include(n => n.CongenitalAnomalies)
                    .FirstOrDefault(n => n.NewbornId == firstNewborn.NewbornId);

                if (fullNewborn != null)
                {
                    newbornViewModel = new NewbornViewModel(fullNewborn);
                    newbornViewModel.IsExistingPatient = true;
                    newbornViewModel.NewbornMrn = fullNewborn.NewbornMrn;
                }
                else
                {
                    newbornViewModel = new NewbornViewModel();
                }
            }
            else
            {
                newbornViewModel = new NewbornViewModel();
            }

            var viewModel = new BirthRegistryViewModel
            {
                BirthId = id,
                FacilityViewModel = new BirthFacilityViewModel(birthRecord),
                FatherInformationViewModel = new FatherInformationViewModel(birthRecord.FatherPerson, birthRecord),
                MothersInformationViewModel = new MothersInformationViewModel(birthRecord.MotherMrnNavigation, birthRecord),
                PrenatalCareViewModel = birthRecord.Prenatals?.FirstOrDefault() != null
                    ? new PrenatalCareViewModel(birthRecord.Prenatals.FirstOrDefault())
                    : new PrenatalCareViewModel(),
                LaborAndDeliveryViewModel = birthRecord.Newborns?.FirstOrDefault()?.LaborAndDeliveries?.FirstOrDefault() != null
                    ? new LaborAndDeliveryViewModel(birthRecord.Newborns.FirstOrDefault().LaborAndDeliveries.FirstOrDefault())
                    : new LaborAndDeliveryViewModel(),
                NewbornViewModel = newbornViewModel,
                FinalizeViewModel = new FinalizeViewModel(birthRecord)

            };

            LoadAllDropdownData(viewModel, birthRecord);

            LoadLaborTooltipData();

            ViewBag.Mode = "edit";
            ViewBag.EditMode = true;
            ViewBag.PreselectedMotherMrn = birthRecord.MotherMrn;
            ViewBag.BirthId = id;
            ViewBag.ActiveTab = tab;

            return View("BirthRegistry", viewModel);
        }

        [HttpGet]
        public IActionResult ViewBirthRegistry(int id, string tab = "mother")
        {
            var birthRecord = GetBirthRegistryWithDetails(id);

            if (birthRecord == null)
            {
                TempData["ErrorMessage"] = "Birth registry not found.";
                return RedirectToAction(nameof(BirthRegistryArchive));
            }

            var viewModel = new BirthRegistryViewModel
            {
                BirthId = id,
                IsComplete = _validationService.IsRegistryComplete(birthRecord),
                FacilityViewModel = new BirthFacilityViewModel(birthRecord),
                FatherInformationViewModel = new FatherInformationViewModel(birthRecord.FatherPerson, birthRecord),
                MothersInformationViewModel = new MothersInformationViewModel(birthRecord.MotherMrnNavigation, birthRecord),
                PrenatalCareViewModel = birthRecord.Prenatals?.FirstOrDefault() != null
                    ? new PrenatalCareViewModel(birthRecord.Prenatals.FirstOrDefault())
                    : new PrenatalCareViewModel(),
                LaborAndDeliveryViewModel = birthRecord.Newborns?.FirstOrDefault()?.LaborAndDeliveries?.FirstOrDefault() != null
                    ? new LaborAndDeliveryViewModel(birthRecord.Newborns.FirstOrDefault().LaborAndDeliveries.FirstOrDefault())
                    : new LaborAndDeliveryViewModel(),
                NewbornViewModel = birthRecord.Newborns?.FirstOrDefault() != null
                    ? new NewbornViewModel(birthRecord.Newborns.FirstOrDefault())
                    : new NewbornViewModel(),
                FinalizeViewModel = new FinalizeViewModel(birthRecord)
            };

            if (!string.IsNullOrWhiteSpace(birthRecord.FacilityTransferredFromName))
            {
                var fromFacility = _repository.Facilities
                    .Include(f => f.Address)
                        .ThenInclude(a => a.AddressState)
                    .AsNoTracking()
                    .FirstOrDefault(f => f.Name == birthRecord.FacilityTransferredFromName);

                if (fromFacility != null)
                {
                    viewModel.FacilityViewModel.FacilityTransferredFromId = fromFacility.FacilityId;
                    viewModel.FacilityViewModel.TransferFacilityCity = fromFacility.Address?.City;
                    viewModel.FacilityViewModel.TransferFacilityZip = fromFacility.Address?.PostalCode;
                    viewModel.FacilityViewModel.TransferFacilityState = fromFacility.Address?.AddressState?.StateName;
                }
            }

            LoadAllDropdownData(viewModel, birthRecord);



            ViewBag.Mode = "view";
            ViewBag.ViewMode = true;
            ViewBag.BirthId = id;
            ViewBag.MotherMrn = birthRecord.MotherMrn;
            ViewBag.ActiveTab = tab;

            return View("BirthRegistry", viewModel);
        }

    // =======================================================
    // Birth Registry - Mother Lookup
    // =======================================================

    [Authorize]
    [PermissionAuthorize("BirthRegistryAdd", "BirthRegistryEdit", "BirthRegistryView")] // <-- adjust to match your project if needed
    [HttpGet]
        public IActionResult SearchMothers(
        string searchLast,
        string searchFirst,
        string searchSSN,
        string searchMRN,
        DateTime? searchDOB,
        DateTime? searchDOBBefore)
            {
            // Retrieve FacilityId from the session (mirrors PatientController)
            var facilityIdString = HttpContext.Session.GetString("Facility");
            if (string.IsNullOrEmpty(facilityIdString))
            {
                return Unauthorized("Facility not set for the session.");
            }
            int facilityId = int.Parse(facilityIdString);

            // Normalize inputs
            searchLast = string.IsNullOrWhiteSpace(searchLast) ? "" : searchLast.Trim().ToUpper();
            searchFirst = string.IsNullOrWhiteSpace(searchFirst) ? "" : searchFirst.Trim().ToUpper();

            IQueryable<Patient> q = repository.Patients
                .Include(p => p.Facility)
                .Include(p => p.Person)
                    .ThenInclude(person => person.Gender)
                .AsNoTracking()
                .Where(p => p.Person.LastName.ToUpper().Contains(searchLast));

            if (!string.IsNullOrWhiteSpace(searchFirst))
                q = q.Where(p => p.Person.FirstName.ToUpper().Contains(searchFirst));

            if (!string.IsNullOrWhiteSpace(searchSSN))
                q = q.Where(p => p.Person.Ssn != null && p.Person.Ssn.Contains(searchSSN));

            if (!string.IsNullOrWhiteSpace(searchMRN))
                q = q.Where(p => p.Mrn != null && p.Mrn.Contains(searchMRN));

            if (searchDOB.HasValue)
                q = q.Where(p => p.Person.Dob >= searchDOB || p.Person.Dob == null);

            if (searchDOBBefore.HasValue)
                q = q.Where(p => p.Person.Dob <= searchDOBBefore || p.Person.Dob == null);

            // FEMALE-only filter (null allowed)
            q = q.Where(p => p.Person.Gender == null || p.Person.Gender.Name.ToUpper() == "FEMALE");

            // Session facility only (matches your Patient list default behavior)
            q = q.Where(p => p.FacilityId == facilityId);

            var results = q
                .OrderBy(p => p.Person.LastName)
                .ThenBy(p => p.Person.FirstName)
                .Take(500)
                .ToList();

            return PartialView("Mother/_MotherLookupResults", results);
            }


        [HttpGet]
        public IActionResult BirthRegistryArchive(
            string motherFirstName, string motherLastName, string motherMrn,
            string motherSsn, DateTime? motherDateOfBirth,
            string newbornFirstName, string newbornLastName, string newbornMrn,
            DateTime? birthDateRangeStart, DateTime? birthDateRangeEnd,
            string registryStatus = "all",
            string sortBy = "motherLastName",
            string sortOrder = "asc",
            int page = 1,
            int pageSize = 25)
        {
            List<BirthRegistryArchiveViewModel> searchResults;

            var hasSearchCriteria = !string.IsNullOrEmpty(motherFirstName) ||
                                !string.IsNullOrEmpty(motherLastName) ||
                                !string.IsNullOrEmpty(motherMrn) ||
                                !string.IsNullOrEmpty(motherSsn) ||
                                motherDateOfBirth.HasValue ||
                                !string.IsNullOrEmpty(newbornFirstName) ||
                                !string.IsNullOrEmpty(newbornLastName) ||
                                !string.IsNullOrEmpty(newbornMrn) ||
                                birthDateRangeStart.HasValue ||
                                birthDateRangeEnd.HasValue ||
                                registryStatus != "all";

            if (hasSearchCriteria)
            {
                searchResults = _searchService.SearchBirthRegistries(
                    motherFirstName, motherLastName, motherMrn, motherSsn,
                    motherDateOfBirth, newbornFirstName, newbornLastName,
                    newbornMrn, birthDateRangeStart, birthDateRangeEnd,
                    registryStatus);
            }
            else
            {
                searchResults = GetAllArchiveData();
            }

            searchResults = SortArchiveResults(searchResults, sortBy, sortOrder);

            var totalRecordCount = searchResults.Count;
            var pagedResults = searchResults
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalRecordCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecordCount / pageSize);
            ViewBag.HasSearchCriteria = hasSearchCriteria;

            var searchParams = BuildArchiveSearchParams(
                motherFirstName, motherLastName, motherMrn, motherSsn, motherDateOfBirth,
                newbornFirstName, newbornLastName, newbornMrn,
                birthDateRangeStart, birthDateRangeEnd,
                registryStatus, sortBy, sortOrder, pageSize);

            ViewBag.SearchParams = searchParams;

            return View(pagedResults);
        }

  private List<BirthRegistryArchiveViewModel> GetAllArchiveData()
{
    return _repository.Births
        .Include(b => b.MotherPerson)        
        .Include(b => b.MotherMrnNavigation)
        .Include(b => b.Facility)
        .Include(b => b.Newborns)
            .ThenInclude(n => n.NewbornMrnNavigation)
        .Include(b => b.Newborns)
            .ThenInclude(n => n.Person)
        .Include(b => b.Newborns)
            .ThenInclude(n => n.LaborAndDeliveries)
        .Include(b => b.Prenatals)
        .Select(b => new BirthRegistryArchiveViewModel
        {
            BirthId = b.BirthId,
            MotherMrn = b.MotherMrn,
            
            MotherFirstName = b.MotherPerson != null ? b.MotherPerson.FirstName : (b.MotherMrnNavigation != null ? b.MotherMrnNavigation.FirstName : "Unknown"),
            MotherMiddleName = b.MotherPerson != null ? b.MotherPerson.MiddleName : (b.MotherMrnNavigation != null ? b.MotherMrnNavigation.MiddleName : "Unknown"),
            MotherLastName = b.MotherPerson != null ? b.MotherPerson.LastName : (b.MotherMrnNavigation != null ? b.MotherMrnNavigation.LastName : "Unknown"),
            
            MotherDob = b.MotherPerson != null ? b.MotherPerson.Dob : (b.MotherMrnNavigation != null ? b.MotherMrnNavigation.Dob : null),
            
            NewbornMrn = b.Newborns.FirstOrDefault() != null ? b.Newborns.FirstOrDefault().NewbornMrn : null,
            
         
            NewbornFirstName = b.Newborns.FirstOrDefault() != null && b.Newborns.FirstOrDefault().Person != null ? b.Newborns.FirstOrDefault().Person.FirstName : (b.Newborns.FirstOrDefault() != null && b.Newborns.FirstOrDefault().NewbornMrnNavigation != null ? b.Newborns.FirstOrDefault().NewbornMrnNavigation.FirstName : "Unknown"),
            NewbornMiddleName = b.Newborns.FirstOrDefault() != null && b.Newborns.FirstOrDefault().Person != null ? b.Newborns.FirstOrDefault().Person.MiddleName : (b.Newborns.FirstOrDefault() != null && b.Newborns.FirstOrDefault().NewbornMrnNavigation != null ? b.Newborns.FirstOrDefault().NewbornMrnNavigation.MiddleName : "Unknown"),
            NewbornLastName = b.Newborns.FirstOrDefault() != null && b.Newborns.FirstOrDefault().Person != null ? b.Newborns.FirstOrDefault().Person.LastName : (b.Newborns.FirstOrDefault() != null && b.Newborns.FirstOrDefault().NewbornMrnNavigation != null ? b.Newborns.FirstOrDefault().NewbornMrnNavigation.LastName : "Unknown"),
        
            NewbornDob = b.Newborns.FirstOrDefault() != null ? b.Newborns.FirstOrDefault().DateAndTimeOfBirth : null,
            FacilityName = b.Facility != null ? b.Facility.Name : "Unknown",
            BirthDate = b.Newborns.FirstOrDefault() != null ? b.Newborns.FirstOrDefault().DateAndTimeOfBirth : null,
            CertifierSignature = b.CertifierSignature,
            
            BirthCount = b.MotherPerson != null ? b.MotherPerson.BirthsAsMother.Count : (b.MotherMrnNavigation != null && b.MotherMrnNavigation.Births != null ? b.MotherMrnNavigation.Births.Count : 0),
            
            IsComplete = _validationService.IsRegistryComplete(b)
        })
        .OrderBy(r => r.MotherLastName)
        .ThenBy(r => r.MotherFirstName)
        .ThenByDescending(r => r.BirthDate)
        .ToList();
}

        private List<BirthRegistryArchiveViewModel> SortArchiveResults(
            List<BirthRegistryArchiveViewModel> results,
            string sortBy,
            string sortOrder)
        {
            var isAscending = sortOrder?.ToLower() == "asc";

            return sortBy?.ToLower() switch
            {
                "motherlastname" or "mothername" => isAscending
                    ? results.OrderBy(r => r.MotherLastName).ThenBy(r => r.MotherFirstName).ToList()
                    : results.OrderByDescending(r => r.MotherLastName).ThenByDescending(r => r.MotherFirstName).ToList(),
                "newbornname" => isAscending
                    ? results.OrderBy(r => r.NewbornLastName).ThenBy(r => r.NewbornFirstName).ToList()
                    : results.OrderByDescending(r => r.NewbornLastName).ThenByDescending(r => r.NewbornFirstName).ToList(),
                "facility" => isAscending
                    ? results.OrderBy(r => r.FacilityName).ToList()
                    : results.OrderByDescending(r => r.FacilityName).ToList(),
                "certifier" => isAscending
                    ? results.OrderBy(r => r.CertifierSignature).ToList()
                    : results.OrderByDescending(r => r.CertifierSignature).ToList(),
                "status" => isAscending
                    ? results.OrderBy(r => r.IsComplete).ToList()
                    : results.OrderByDescending(r => r.IsComplete).ToList(),
                _ => results.OrderBy(r => r.MotherLastName).ThenBy(r => r.MotherFirstName).ToList()
            };
        }

        private Dictionary<string, object> BuildArchiveSearchParams(
            string motherFirstName, string motherLastName, string motherMrn, string motherSsn, DateTime? motherDateOfBirth,
            string newbornFirstName, string newbornLastName, string newbornMrn,
            DateTime? birthDateRangeStart, DateTime? birthDateRangeEnd,
            string registryStatus, string sortBy, string sortOrder, int pageSize)
        {
            var searchParams = new Dictionary<string, object>
            {
                {"sortBy", sortBy},
                {"sortOrder", sortOrder},
                {"pageSize", pageSize},
                {"registryStatus", registryStatus}
            };

            if (!string.IsNullOrEmpty(motherFirstName))
                searchParams["motherFirstName"] = motherFirstName;

            if (!string.IsNullOrEmpty(motherLastName))
                searchParams["motherLastName"] = motherLastName;

            if (!string.IsNullOrEmpty(motherMrn))
                searchParams["motherMrn"] = motherMrn;

            if (!string.IsNullOrEmpty(motherSsn))
                searchParams["motherSsn"] = motherSsn;

            if (motherDateOfBirth.HasValue)
                searchParams["motherDateOfBirth"] = motherDateOfBirth.Value.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(newbornFirstName))
                searchParams["newbornFirstName"] = newbornFirstName;

            if (!string.IsNullOrEmpty(newbornLastName))
                searchParams["newbornLastName"] = newbornLastName;

            if (!string.IsNullOrEmpty(newbornMrn))
                searchParams["newbornMrn"] = newbornMrn;

            if (birthDateRangeStart.HasValue)
                searchParams["birthDateRangeStart"] = birthDateRangeStart.Value.ToString("yyyy-MM-dd");

            if (birthDateRangeEnd.HasValue)
                searchParams["birthDateRangeEnd"] = birthDateRangeEnd.Value.ToString("yyyy-MM-dd");

            return searchParams;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBirthRegistry(int id)
        {
            try
            {
                var birth = await _repository.Births
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                            .ThenInclude(ld => ld.Characteristics)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                            .ThenInclude(ld => ld.MaternalMorbidities)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                            .ThenInclude(ld => ld.OnsetOfLabors)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.AbnormalConditions)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.CongenitalAnomalies)
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Education)
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Ethnicity)
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Races)
                    .Include(b => b.Prenatals)
                        .ThenInclude(p => p.Infections)
                    .Include(b => b.Prenatals)
                        .ThenInclude(p => p.RiskFactors)
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Races)
                    .FirstOrDefaultAsync(b => b.BirthId == id);

                if (birth == null)
                {
                    TempData["ErrorMessage"] = "Birth registry not found.";
                    return RedirectToAction("BirthRegistryArchive");
                }

                // Delete in correct order to respect foreign key constraints

                // 1. Delete Prenatal data
                if (birth.Prenatals != null && birth.Prenatals.Any())
                {
                    foreach (var prenatal in birth.Prenatals.ToList())
                    {
                        // Clear many-to-many relationships
                        prenatal.Infections?.Clear();
                        prenatal.RiskFactors?.Clear();

                        _repository.DeletePrenatal(prenatal);
                    }
                }

                // 2. Delete Labor & Delivery data for each newborn
                if (birth.Newborns != null && birth.Newborns.Any())
                {
                    foreach (var newborn in birth.Newborns.ToList())
                    {
                        if (newborn.LaborAndDeliveries != null && newborn.LaborAndDeliveries.Any())
                        {
                            foreach (var laborDelivery in newborn.LaborAndDeliveries.ToList())
                            {
                                // Clear many-to-many relationships
                                laborDelivery.Characteristics?.Clear();
                                laborDelivery.MaternalMorbidities?.Clear();
                                laborDelivery.OnsetOfLabors?.Clear();

                                _repository.DeleteLaborAndDelivery(laborDelivery);
                            }
                        }

                        // Clear newborn many-to-many relationships
                        newborn.AbnormalConditions?.Clear();
                        newborn.CongenitalAnomalies?.Clear();

                        // Delete the newborn record
                        _repository.DeleteNewborn(newborn);
                    }
                }

                // 3. Delete Birth Father (if exists and not shared)
                if (birth.BirthFatherId.HasValue && birth.BirthFather != null)
                {
                    // Clear race associations
                    birth.BirthFather.Races?.Clear();

                    // Check if this father is used by other births
                    var otherBirthsWithSameFather = await _repository.Births
                        .Where(b => b.BirthFatherId == birth.BirthFatherId && b.BirthId != id)
                        .AnyAsync();

                    // Only delete if not used elsewhere
                    if (!otherBirthsWithSameFather)
                    {
                        _repository.DeleteBirthFather(birth.BirthFather);
                    }
                }

                // 4. Finally, delete the Birth record
                _repository.DeleteBirth(birth);

                TempData["SuccessMessage"] = $"Birth registry for {birth.MotherMrn} deleted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting registry: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                TempData["ErrorMessage"] = $"Error deleting registry: {ex.Message}";
            }

            return RedirectToAction("BirthRegistryArchive");
        }

        #endregion

        #region Main Helper Methods

        private Birth GetBirthRegistryWithDetails(int id)
        {
            return _repository.Births
                .Include(b => b.MotherPerson)
                .Include(b => b.FatherPerson)
                .ThenInclude(fp => fp.EducationLevel)
                .Include(b => b.FatherPerson)
                .ThenInclude(fp => fp.Ethnicity)
                .Include(b => b.FatherPerson)
                .ThenInclude(fp => fp.PersonRaces)
                .Include(b => b.MotherMrnNavigation)
                    .ThenInclude(m => m.Religion)
                .Include(b => b.MotherMrnNavigation)
                    .ThenInclude(m => m.Ethnicity)
                .Include(b => b.MotherMrnNavigation)
                    .ThenInclude(m => m.EducationLevel)
                .Include(b => b.MotherMrnNavigation)
                    .ThenInclude(m => m.PatientRaces)
                .Include(b => b.Facility)
                    .ThenInclude(f => f.Address)
                    .ThenInclude(a => a.AddressState)
                .Include(b => b.Address)
                    .ThenInclude(a => a.AddressState)
                .Include(b => b.BirthPlaceType)
                .Include(b => b.DeliveringAttendant)
                .Include(b => b.CertifierOfBirth)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.NewbornMrnNavigation)
                .Include(b => b.Newborns)
                .ThenInclude(n => n.Person)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.LaborAndDeliveries)
                        .ThenInclude(ld => ld.FetalPresentationAtBirth)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.LaborAndDeliveries)
                        .ThenInclude(ld => ld.FinalRouteAndMethod)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.LaborAndDeliveries)
                        .ThenInclude(ld => ld.Characteristics)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.LaborAndDeliveries)
                        .ThenInclude(ld => ld.MaternalMorbidities)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.AbnormalConditions)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.CongenitalAnomalies)
                /*.Include(b => b.BirthFather)
                    .ThenInclude(bf => bf.Education)
                .Include(b => b.BirthFather)
                    .ThenInclude(bf => bf.Ethnicity)
                .Include(b => b.BirthFather)
                    .ThenInclude(bf => bf.Races)*/
                .Include(b => b.Prenatals)
                    .ThenInclude(p => p.Infections)
                .Include(b => b.Prenatals)
                    .ThenInclude(p => p.RiskFactors)
                .FirstOrDefault(b => b.BirthId == id);
        }

        private void LoadAllDropdownData(BirthRegistryViewModel viewModel, Birth birthRecord = null)
        {
            _dropdownService.PopulateFacilityDropdowns(viewModel.FacilityViewModel);
            _dropdownService.PopulateFatherDropdowns(viewModel.FatherInformationViewModel);
            _dropdownService.PopulatePrenatalDropdowns(viewModel.PrenatalCareViewModel);
            _dropdownService.PopulateLaborDropdowns(viewModel.LaborAndDeliveryViewModel);
            _dropdownService.PopulateNewbornDropdowns(viewModel.NewbornViewModel);

            // Always populate mother dropdowns, regardless of database status
            _dropdownService.PopulateMotherDropdowns(viewModel.MothersInformationViewModel);
            if (birthRecord?.MotherMrnNavigation != null)
            {
                LoadMotherAddressData(viewModel.MothersInformationViewModel, birthRecord.MotherMrn);
            }
        }
        #endregion

        #region Facility Section

        [HttpGet]
        public IActionResult GetPhysicianById(int id)
        {
            var physician = _repository.Physicians.FirstOrDefault(p => p.PhysicianId == id);

            if (physician == null)
                return NotFound();

            return Json(new
            {
                fullName = $"{physician.FirstName} {physician.LastName}",
                npi = physician.License,
                titles = physician.Credentials
            });
        }

        [HttpGet]
        public IActionResult SearchPhysicians(string term, int take = 10)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(Array.Empty<object>());

            term = term.Trim();

            var results = _repository.Physicians
                .Where(p => p.ProviderStatusId == 1 &&
                    (
                        p.FirstName.Contains(term) ||
                        p.LastName.Contains(term) ||
                        p.License.Contains(term) ||
                        p.Credentials.Contains(term)
                    ))
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                .Take(take)
                .Select(p => new
                {
                    id = p.PhysicianId,
                    fullName = p.FirstName + " " + p.LastName,
                    npi = p.License,
                    titles = p.Credentials
                })
                .ToList();

            return Json(results);
        }

        [HttpGet]
        public IActionResult SearchFacilities(string term)
        {
            term = term?.Trim();

            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(new
                {
                    success = true,
                    results = Array.Empty<BirthFacilityViewModel.FacilitySearchResultVm>()
                });
            }

            var query = _repository.Facilities
                .Include(f => f.Address)
                    .ThenInclude(a => a.AddressState)
                .AsNoTracking()
                .Where(f =>
                    f.Name.Contains(term) ||
                    (!string.IsNullOrEmpty(f.Description) && f.Description.Contains(term)) ||
                    (!string.IsNullOrEmpty(f.Phone) && f.Phone.Contains(term)) ||
                    (f.Address != null && (
                        (!string.IsNullOrEmpty(f.Address.Address1) && f.Address.Address1.Contains(term)) ||
                        (!string.IsNullOrEmpty(f.Address.Address2) && f.Address.Address2.Contains(term)) ||
                        (!string.IsNullOrEmpty(f.Address.City) && f.Address.City.Contains(term)) ||
                        (!string.IsNullOrEmpty(f.Address.County) && f.Address.County.Contains(term)) ||
                        (!string.IsNullOrEmpty(f.Address.PostalCode) && f.Address.PostalCode.Contains(term)) ||
                        (f.Address.AddressState != null &&
                         !string.IsNullOrEmpty(f.Address.AddressState.StateName) &&
                         f.Address.AddressState.StateName.Contains(term))
                    ))
                );

            var results = query
                .OrderBy(f => f.Name)
                .Take(10)
                .Select(f => new BirthFacilityViewModel.FacilitySearchResultVm
                {
                    Id = f.FacilityId,
                    Name = f.Name,
                    Description = f.Description,
                    Phone = f.Phone,

                    Street = f.Address != null ? f.Address.Address1 : string.Empty,
                    Street2 = f.Address != null ? f.Address.Address2 : string.Empty,
                    City = f.Address != null ? f.Address.City : string.Empty,
                    County = f.Address != null ? f.Address.County : string.Empty,
                    Postal = f.Address != null ? f.Address.PostalCode : string.Empty,
                    State = (f.Address != null && f.Address.AddressState != null)
                                    ? f.Address.AddressState.StateName
                                    : string.Empty
                })
                .ToList();

            return Json(new { success = true, results });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveFacilityData([FromBody] BirthFacilityViewModel model)
        {
            try
            {
                if (model == null || !model.BirthId.HasValue || model.BirthId.Value <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid facility data submitted."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.PlaceOfBirth))
                    return Json(new { success = false, message = "Place of birth is required." });

                if (!model.BirthPlaceTypeId.HasValue)
                    return Json(new { success = false, message = "Birth place type is required." });

                if (model.PlaceOfBirth == "NonFacility" && !model.NonWctmcFacilityId.HasValue)
                    return Json(new { success = false, message = "Please select the non-WCTMC facility where the birth occurred." });

                if (model.IsMotherTransferred == true && !model.FacilityTransferredFromId.HasValue)
                    return Json(new { success = false, message = "Please select the facility the mother was transferred from." });

                if (!model.DeliveringAttendantId.HasValue)
                    return Json(new { success = false, message = "Please search for and select a delivering attendant." });

                if (model.IsCertifierAttendant == false && !model.CertifierOfBirthId.HasValue)
                    return Json(new { success = false, message = "Please search for and select a certifier." });

                if (string.IsNullOrWhiteSpace(model.CertifierSignature))
                    return Json(new { success = false, message = "Certifier signature is required." });

                if (!model.DateCertified.HasValue)
                    return Json(new { success = false, message = "Date certified is required." });

                var today = DateOnly.FromDateTime(DateTime.Today);
                if (model.DateCertified.Value > today)
                    return Json(new { success = false, message = "Date certified cannot be in the future." });

                Birth birth;

                if (model.BirthId.HasValue && model.BirthId.Value > 0)
                {
                    birth = _repository.Births
                        .Include(b => b.Facility)
                        .Include(b => b.BirthPlaceType)
                        .Include(b => b.DeliveringAttendant)
                        .Include(b => b.CertifierOfBirth)
                        .FirstOrDefault(b => b.BirthId == model.BirthId.Value);

                    if (birth == null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Birth record not found."
                        });
                    }
                }
                else
                {
                    birth = new Birth();
                }

                birth.IsWctmcbirth = model.PlaceOfBirth == "WCTMC";
                birth.BirthPlaceTypeId = model.BirthPlaceTypeId;

                if (model.PlaceOfBirth == "WCTMC")
                {
                    birth.FacilityId = model.FacilityId;
                }
                else if (model.PlaceOfBirth == "NonFacility")
                {
                    birth.FacilityId = model.NonWctmcFacilityId;
                }
                else
                {
                    birth.FacilityId = null;
                }

                birth.IsMotherTransferred = model.IsMotherTransferred;

                if (model.IsMotherTransferred == true && model.FacilityTransferredFromId.HasValue)
                {
                    var fromFacility = _repository.Facilities
                        .FirstOrDefault(f => f.FacilityId == model.FacilityTransferredFromId.Value);

                    birth.FacilityTransferredFromName = fromFacility?.Name;
                }
                else
                {
                    birth.FacilityTransferredFromName = null;
                }

                birth.DeliveringAttendantId = model.DeliveringAttendantId;

                if (model.IsCertifierAttendant == true)
                {
                    birth.CertifierOfBirthId = model.DeliveringAttendantId;
                }
                else
                {
                    birth.CertifierOfBirthId = model.CertifierOfBirthId;
                }

                birth.CertifierSignature = model.CertifierSignature;
                birth.DateCertified = model.DateCertified;

                birth = _repository.SaveOrUpdateBirth(birth);

                return Json(new
                {
                    success = true,
                    message = "Facility data saved successfully.",
                    birthId = birth.BirthId
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "An unexpected error occurred while saving facility data. Please try again."
                });
            }
        }

        #endregion

        #region Mother Actions

        [HttpGet]
        public IActionResult GetMotherDetails(string mrn, string mode = "create")
        {
            if (string.IsNullOrEmpty(mrn))
                return Content("<div class='alert alert-danger'>MRN is required</div>", "text/html");

            var patient = _repository.Patients
                .Include(p => p.Person)
                    .ThenInclude(per => per.PersonRaces)
                .FirstOrDefault(p => p.Mrn == mrn);

            if (patient == null)
                return Content("<div class='alert alert-danger'>Patient not found</div>", "text/html");

            // Force reload of Person and PersonRaces from DB to get latest races
            var person = _repository.Persons
                .Include(per => per.PersonRaces)
                .FirstOrDefault(per => per.PersonId == patient.PersonId);

            // Map to MothersInformationViewModel
            var viewModel = new IS_Proj_HIT.ViewModels.BirthRegistry.MothersInformationViewModel();
            viewModel.IsExistingPatient = true;
            viewModel.Mrn = patient.Mrn;
            viewModel.FirstName = patient.FirstName ?? person?.FirstName;
            viewModel.MiddleName = patient.MiddleName ?? person?.MiddleName;
            viewModel.LastName = patient.LastName ?? person?.LastName;
            viewModel.PersonId = patient.PersonId;
            viewModel.DateOfBirth = patient.Dob ?? person?.Dob;
            viewModel.SSN = patient.Ssn ?? person?.Ssn;
            // Add more mappings as needed for your UI

            if (person != null)
            {
                viewModel.FromPerson(person);
            }
            // Populate dropdowns for dynamic partial loads (must be after FromPerson)
            _dropdownService.PopulateMotherDropdowns(viewModel);

            // Return the partial view for the full accordion
            return PartialView("~/Views/BirthRegistry/Mother/_MotherAccordionSections.cshtml", viewModel);
        }
        [HttpGet]
        public IActionResult SearchMother(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new { success = false, message = "Search term is required" });
            }

            // Only return female patients (SexId == 2)
            var lowerSearchTerm = searchTerm.ToLower();
            var ssnSearchTerm = searchTerm.Replace("-", "").Replace(" ", ""); // Remove dashes and spaces

            var results = _repository.Patients
                .Include(p => p.Person)
                .Where(p =>
                    (
                        // Patient or Person must be female
                        (p.SexId == 2) ||
                        (p.Person != null && p.Person.SexId == 2)
                    ) &&
                    (
                        (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.ToLower().Contains(lowerSearchTerm)) ||
                        (!string.IsNullOrEmpty(p.LastName) && p.LastName.ToLower().Contains(lowerSearchTerm)) ||
                        (!string.IsNullOrEmpty(p.Mrn) && p.Mrn.ToLower().Contains(lowerSearchTerm)) ||
                        (!string.IsNullOrEmpty(p.Ssn) && p.Ssn.Replace("-", "").Replace(" ", "").Contains(ssnSearchTerm)) ||
                        (p.Person != null && (
                            (!string.IsNullOrEmpty(p.Person.FirstName) && p.Person.FirstName.ToLower().Contains(lowerSearchTerm)) ||
                            (!string.IsNullOrEmpty(p.Person.LastName) && p.Person.LastName.ToLower().Contains(lowerSearchTerm))
                        ))
                    )
                )
                .Select(p => new {
                    p.Mrn,
                    p.FirstName,
                    p.LastName,
                    p.MiddleName,
                    p.Ssn,
                    p.Dob,
                    p.PersonId,
                    Person = p.Person == null ? null : new {
                        p.Person.PersonId,
                        p.Person.FirstName,
                        p.Person.LastName,
                        p.Person.MiddleName,
                        p.Person.Dob
                    }
                })
                .Take(20)
                .ToList();

            return Json(new { success = true, patients = results });
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult SaveMotherData([FromBody] MothersInformationViewModel viewModel)
{
    try
    {
        Console.WriteLine($"[DEBUG] SaveMotherData called: PersonId={viewModel?.PersonId}, Mrn={viewModel?.Mrn}, IsExistingPatient={viewModel?.IsExistingPatient}");
        
        if (viewModel != null)
        {
            Console.WriteLine($"[DEBUG] Incoming cultural fields: ReligionId={(viewModel.Religion != null ? viewModel.Religion.ReligionId.ToString() : "null")}, EthnicityId={(viewModel.Ethnicity != null ? viewModel.Ethnicity.EthnicityId.ToString() : "null")}, SelectedRaceIdsCount={(viewModel.SelectedRaceIds != null ? viewModel.SelectedRaceIds.Count.ToString() : "0")}");
        }

        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Model validation failed", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        if (viewModel == null)
        {
            return Json(new { success = false, message = "Mother information is required." });
        }

        // Only require FirstName for new mothers (not existing patients)
        if (!viewModel.IsExistingPatient && string.IsNullOrWhiteSpace(viewModel.FirstName))
        {
            return Json(new { success = false, message = "Mother first name is required." });
        }

        if (viewModel.IsExistingPatient && viewModel.PersonId == null && string.IsNullOrEmpty(viewModel.Mrn))
        {
            return Json(new { success = false, message = "Mother identifier is required for existing patients." });
        }

        // Setup our variables using the requested naming convention
        Person motherPerson = null;
        Patient motherPatient = null;

        // 1. Attempt to load by PersonId
        if (viewModel.PersonId.HasValue)
        {
            motherPerson = _repository.Persons
                .Include(p => p.PersonRaces)
                .Include(p => p.PersonContactDetail)
                    .ThenInclude(cd => cd.ResidenceAddress)
                .Include(p => p.PersonContactDetail)
                    .ThenInclude(cd => cd.MailingAddress)
                .FirstOrDefault(p => p.PersonId == viewModel.PersonId.Value);

            motherPatient = _repository.Patients.FirstOrDefault(p => p.PersonId == viewModel.PersonId.Value);
        }
        // 2. Fallback to loading by MRN
        else if (!string.IsNullOrEmpty(viewModel.Mrn))
        {
            motherPatient = _repository.Patients
                .Include(p => p.Person)
                    .ThenInclude(per => per.PersonRaces)
                .Include(p => p.Person)
                    .ThenInclude(per => per.PersonContactDetail)
                        .ThenInclude(cd => cd.ResidenceAddress)
                .Include(p => p.Person)
                    .ThenInclude(per => per.PersonContactDetail)
                        .ThenInclude(cd => cd.MailingAddress)
                .FirstOrDefault(p => p.Mrn == viewModel.Mrn);

            if (motherPatient != null)
            {
                motherPerson = motherPatient.Person;
            }
        }

        // 3. Handle missing Person
        if (motherPerson == null)
        {
            if (viewModel.IsExistingPatient)
            {
                // If they claim to be an existing patient but we found nothing, return an error
                return Json(new { success = false, message = "Mother person record not found in database." });
            }
            else
            {
                // If they are a new mother, instantiate a new Person so we can save it!
                motherPerson = new Person();
                _repository.AddPerson(motherPerson);
            }
        }

        // Update Person entity from ViewModel
        viewModel.UpdatePerson(motherPerson);

        // Keep Patient names in sync with Person (only if a patient record actually exists)
        if (motherPatient != null)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.FirstName))
                motherPatient.FirstName = viewModel.FirstName;
            if (!string.IsNullOrWhiteSpace(viewModel.MiddleName))
                motherPatient.MiddleName = viewModel.MiddleName;
            if (!string.IsNullOrWhiteSpace(viewModel.LastName))
                motherPatient.LastName = viewModel.LastName;
        }

        // --- Synchronize PersonRace ---
        var selectedRaceIds = (viewModel.SelectedRaceIds ?? new List<byte>())
            .Select(id => (byte)Convert.ChangeType(id, typeof(byte))).ToList();
        var existingPersonRaces = motherPerson.PersonRaces?.ToList() ?? new List<PersonRace>();
        
        // Remove unselected races
        foreach (var pr in existingPersonRaces.Where(pr => !selectedRaceIds.Contains(pr.RaceId)).ToList())
        {
            _repository.DeletePersonRace(pr);
        }
        // Add new races
        foreach (var raceId in selectedRaceIds)
        {
            if (!existingPersonRaces.Any(pr => pr.RaceId == raceId))
            {
                var newPr = new PersonRace { PersonId = motherPerson.PersonId, RaceId = raceId, LastModified = DateTime.Now };
                _repository.AddPersonRace(newPr);
            }
        }

        // --- Update PersonContactDetail (addresses) ---
        if (motherPerson.PersonContactDetail == null)
        {
            motherPerson.PersonContactDetail = new PersonContactDetail { PersonId = motherPerson.PersonId, LastModified = DateTime.Now };
            _repository.AddPersonContactDetail(motherPerson.PersonContactDetail);
        }
        var pcd = motherPerson.PersonContactDetail;
        
        // Update residence address only if Address1 is provided
        if (viewModel.ResidentialAddress != null && !string.IsNullOrWhiteSpace(viewModel.ResidentialAddress.Address1))
        {
            if (pcd.ResidenceAddress == null)
            {
                pcd.ResidenceAddress = new Address { LastModified = DateTime.Now };
            }
            pcd.ResidenceAddress.Address1 = viewModel.ResidentialAddress.Address1;
            pcd.ResidenceAddress.Address2 = viewModel.ResidentialAddress.Address2;
            pcd.ResidenceAddress.City = viewModel.ResidentialAddress.City;
            pcd.ResidenceAddress.PostalCode = viewModel.ResidentialAddress.PostalCode;
            pcd.ResidenceAddress.County = viewModel.ResidentialAddress.County;
            pcd.ResidenceAddress.AddressStateID = viewModel.ResidentialAddress.AddressStateID;
            pcd.ResidenceAddress.CountryId = viewModel.ResidentialAddress.CountryId;
            pcd.ResidenceAddress.LastModified = DateTime.Now;
        }
        else
        {
            pcd.ResidenceAddress = null;
        }

        // Update mailing address only if Address1 is provided
        if (!viewModel.MailingAddressSameAsResidential && viewModel.MailingAddress != null && !string.IsNullOrWhiteSpace(viewModel.MailingAddress.Address1))
        {
            if (pcd.MailingAddress == null)
            {
                pcd.MailingAddress = new Address { LastModified = DateTime.Now };
            }
            pcd.MailingAddress.Address1 = viewModel.MailingAddress.Address1;
            pcd.MailingAddress.Address2 = viewModel.MailingAddress.Address2;
            pcd.MailingAddress.City = viewModel.MailingAddress.City;
            pcd.MailingAddress.PostalCode = viewModel.MailingAddress.PostalCode;
            pcd.MailingAddress.County = viewModel.MailingAddress.County;
            pcd.MailingAddress.AddressStateID = viewModel.MailingAddress.AddressStateID;
            pcd.MailingAddress.CountryId = viewModel.MailingAddress.CountryId;
            pcd.MailingAddress.LastModified = DateTime.Now;
        }
        else if (viewModel.MailingAddressSameAsResidential)
        {
            pcd.MailingAddress = null;
        }
        pcd.LastModified = DateTime.Now;
        _repository.EditPersonContactDetail(pcd);

        // Save changes to Person (and related)
        _repository.EditPerson(motherPerson);

        // Get or create the birth record
        Birth birth;
        if (viewModel.BirthId.HasValue && viewModel.BirthId.Value > 0)
        {
            birth = _repository.Births.FirstOrDefault(b => b.BirthId == viewModel.BirthId.Value);
            if (birth == null)
            {
                return Json(new { success = false, message = "Birth record not found." });
            }
        }
        else
        {
            birth = new Birth();
        }

        // Link the mother to the birth record
        viewModel.UpdateBirthRecord(birth);
        
        // Link the PersonId, not the Patient's PersonId, because Patient might be null!
        birth.MotherPersonId = motherPerson.PersonId; 
        
        // Save the birth record
        birth = _repository.SaveOrUpdateBirth(birth);

        return Json(new
        {
            success = true,
            message = "Mother information saved successfully.",
            birthId = birth.BirthId,
            motherMrn = birth.MotherMrn,
            personId = motherPerson.PersonId
        });
    }
    catch (Exception ex)
    {
        string receivedModelJson = string.Empty;
        try {
            receivedModelJson = System.Text.Json.JsonSerializer.Serialize(viewModel);
        } catch {}
        
        return Json(new
        {
            success = false,
            message = $"Error saving mother information: {ex.Message}\nReceived viewModel: {receivedModelJson}"
        });
    }
}

        #endregion

        #region Mother Helper Methods

        private IQueryable<Patient> SearchFemalePatientsQuery(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();
            var ssnSearchTerm = searchTerm.Replace("-", "").Replace(" ", ""); // Remove dashes and spaces
            
            return _repository.Patients
                .Where(p => (p.SexId != 2) && // filter out male
                        (p.FirstName.ToLower().Contains(lowerSearchTerm) ||
                p.LastName.ToLower().Contains(lowerSearchTerm) ||
                p.Mrn.ToLower().Contains(lowerSearchTerm) ||
                p.Ssn.Contains(ssnSearchTerm))); // Use normalized SSN
        }

        private Patient GetFemalePatientWithRelatedData(string mrn)
        {

            return _repository.Patients
                .Include(p => p.Religion)
                .Include(p => p.Ethnicity)
                .Include(p => p.EducationLevel)
                .Include(p => p.PatientRaces)
                .Include(p => p.MaritalStatus)
                .Where(p => p.SexId != 2) // filter out males
                .FirstOrDefault(p => p.Mrn == mrn);
        }


        private void LoadMotherAddressData(MothersInformationViewModel viewModel, string mrn)
        {
            var patientContactDetail = _repository.PatientContactDetails
                .Include(pcd => pcd.ResidenceAddress)
                    .ThenInclude(ra => ra.AddressState)
                .Include(pcd => pcd.MailingAddress)
                    .ThenInclude(ma => ma.AddressState)
                .FirstOrDefault(pcd => pcd.Mrn == mrn);

            if (patientContactDetail == null)
                return;

            // Load residential address
            if (patientContactDetail.ResidenceAddressId.HasValue && patientContactDetail.ResidenceAddress != null)
            {
                viewModel.ResidentialAddress = patientContactDetail.ResidenceAddress;
            }

            // Load mailing address
            if (patientContactDetail.MailingAddressId.HasValue && patientContactDetail.MailingAddress != null)
            {
                viewModel.MailingAddress = patientContactDetail.MailingAddress;
                viewModel.MailingAddressSameAsResidential = false;
            }
            else
            {
                // If no separate mailing address, assume same as residential
                viewModel.MailingAddressSameAsResidential = true;
                if (viewModel.ResidentialAddress != null)
                {
                    viewModel.MailingAddress = viewModel.ResidentialAddress;
                }
            }
        }
        #endregion

        #region Father Section
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveFatherData([FromBody] FatherInformationViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return Json(new { success = false, message = "Father information is required" });
                }

                if (!viewModel.BirthId.HasValue || viewModel.BirthId.Value <= 0)
                {
                    return Json(new { success = false, message = "Birth record not found. Please save mother information first." });
                }

                var birth = _repository.Births
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Races)
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Education)
                    .Include(b => b.BirthFather)
                        .ThenInclude(bf => bf.Ethnicity)
                    .FirstOrDefault(b => b.BirthId == viewModel.BirthId.Value);

                if (birth == null)
                {
                    return Json(new { success = false, message = "Birth record not found" });
                }

                if (!viewModel.HasPaternityAcknowledgement.HasValue)
                {
                    return Json(new { success = false, message = "Paternity acknowledgement status is required" });
                }

                BirthFather birthFather = null;
                string successMessage;

                bool hasAnyFatherData = !string.IsNullOrWhiteSpace(viewModel.FirstName) ||
                                        !string.IsNullOrWhiteSpace(viewModel.LastName);

                if (hasAnyFatherData)
                {
                    if (string.IsNullOrWhiteSpace(viewModel.FirstName) || string.IsNullOrWhiteSpace(viewModel.LastName))
                    {
                        return Json(new { success = false, message = "If providing father information, both first and last name are required" });
                    }

                    var existingFather = birth.BirthFatherId.HasValue ? birth.BirthFather : null;
                    birthFather = viewModel.ToEntity(existingFather);

                    if (birthFather != null)
                    {
                        var selectedRaces = _repository.Races
                            .Where(r => viewModel.SelectedRaceIds.Contains(r.RaceId))
                            .ToList();

                        viewModel.UpdateFatherRaces(birthFather, selectedRaces);
                        birthFather = _repository.SaveOrUpdateBirthFather(birthFather);

                        birth.BirthFatherId = birthFather.BirthFatherId;
                    }
                }

                birth.PaternityAcknowledgementSigned = viewModel.HasPaternityAcknowledgement;
                _repository.EditBirth(birth);

                successMessage = viewModel.HasPaternityAcknowledgement.Value
                    ? "Father information saved successfully"
                    : "Father information saved (will not be included on birth certificate)";

                return Json(new
                {
                    success = true,
                    message = successMessage,
                    birthId = birth.BirthId,
                    birthFatherId = birthFather?.BirthFatherId,
                    paternityAcknowledged = viewModel.HasPaternityAcknowledgement
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving father data: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return Json(new
                {
                    success = false,
                    message = $"Error saving father information: {ex.Message}"
                });
            }
        }
        #endregion

        #region Prenatal Section

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePrenatalData([FromBody] PrenatalCareViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return Json(new { success = false, message = "Prenatal information is required" });
                }

                if (!viewModel.BirthId.HasValue || viewModel.BirthId <= 0)
                {
                    return Json(new { success = false, message = "Birth record not found. Please save mother information first." });
                }

                var birth = _repository.Births
                    .Include(b => b.Prenatals)
                        .ThenInclude(p => p.Infections)
                    .Include(b => b.Prenatals)
                        .ThenInclude(p => p.RiskFactors)
                    .FirstOrDefault(b => b.BirthId == viewModel.BirthId.Value);

                if (birth == null)
                {
                    return Json(new { success = false, message = "Birth record not found." });
                }

                var existingPrenatal = birth.Prenatals?.FirstOrDefault();
                var prenatal = viewModel.ToEntity(birth.BirthId, existingPrenatal);

                prenatal.Infections.Clear();
                if (viewModel.SelectedInfectionIds != null && viewModel.SelectedInfectionIds.Any())
                {
                    var selectedInfections = _repository.PregnancyInfections
                        .Where(i => viewModel.SelectedInfectionIds.Contains(i.InfectionId))
                        .ToList();

                    foreach (var infection in selectedInfections)
                    {
                        prenatal.Infections.Add(infection);
                    }
                }

                prenatal.RiskFactors.Clear();
                if (viewModel.SelectedRiskFactorIds != null && viewModel.SelectedRiskFactorIds.Any())
                {
                    var selectedRiskFactors = _repository.PregnancyRiskFactors
                        .Where(rf => viewModel.SelectedRiskFactorIds.Contains(rf.RiskFactorId))
                        .ToList();

                    foreach (var riskFactor in selectedRiskFactors)
                    {
                        prenatal.RiskFactors.Add(riskFactor);
                    }
                }

                if (existingPrenatal == null)
                {
                    _repository.AddPrenatal(prenatal);
                }
                else
                {
                    _repository.EditPrenatal(prenatal);
                }

                return Json(new
                {
                    success = true,
                    message = "Prenatal care saved successfully",
                    birthId = birth.BirthId,
                    prenatalId = prenatal.PrenatalId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving prenatal data: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = $"Error saving prenatal care: {ex.Message}"
                });
            }
        }


        #endregion

        #region Labor & Delivery Section

        // DB tooltip descriptions (used by edit tooltips)
        private void LoadLaborTooltipData()
        {
            ViewBag.CharacteristicsDescriptions = _repository.CharacteristicOfLabors
                .ToDictionary(c => c.CharacteristicId, c => c.CharacteristicDescription);

            ViewBag.FetalPresentationDescriptions = _repository.FetalPresentationAtBirths
                .ToDictionary(f => f.FetalPresentationAtBirthId, f => f.FetalPresentationDescription);

            ViewBag.FinalRouteAndMethodDescriptions = _repository.FinalRouteAndMethodOfDeliveries
                .ToDictionary(r => r.FinalRouteAndMethodId, r => r.FinalRouteAndMethodDescription);

            ViewBag.MaternalMorbidityDescriptions = _repository.MaternalMorbidities
                .ToDictionary(m => m.MaternalMorbidityId, m => m.MaternalMorbidityDescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveLaborAndDelivery([FromBody] LaborAndDeliveryViewModel vm)
        {
            try
            {
                if (vm == null || !vm.BirthId.HasValue || vm.BirthId.Value <= 0)
                    return Json(new { success = false, message = "BirthId is required." });

                var birth = _repository.Births
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                            .ThenInclude(ld => ld.Characteristics)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                            .ThenInclude(ld => ld.MaternalMorbidities)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                            .ThenInclude(ld => ld.OnsetOfLabors)
                    .FirstOrDefault(b => b.BirthId == vm.BirthId.Value);

                if (birth == null)
                    return Json(new { success = false, message = "Birth record not found." });

                Newborn newborn = null;
                if (vm.NewbornId.HasValue)
                {
                    newborn = birth.Newborns.FirstOrDefault(n => n.NewbornId == vm.NewbornId.Value);
                    if (newborn == null)
                        return Json(new { success = false, message = $"No newborn found for NewbornId {vm.NewbornId.Value}." });
                }
                else
                {
                    newborn = birth.Newborns.FirstOrDefault();
                    if (newborn == null)
                        return Json(new { success = false, message = "No newborn is associated to this birth yet." });
                }

                var labor = newborn.LaborAndDeliveries?.FirstOrDefault();
                var isNew = false;
                if (labor == null)
                {
                    labor = new LaborAndDelivery { NewbornId = newborn.NewbornId };
                    isNew = true;
                }

                byte? fpId = vm.FetalPresentationAtBirthId.HasValue ? (byte?)Convert.ToByte(vm.FetalPresentationAtBirthId.Value) : null;
                byte? frId = vm.FinalRouteAndMethodId.HasValue ? (byte?)Convert.ToByte(vm.FinalRouteAndMethodId.Value) : null;

                labor.FetalPresentationAtBirthId = fpId;
                labor.FinalRouteAndMethodId = frId;
                labor.TrialOfLaborBeforeCesarean = vm.TrialOfLaborBeforeCesarean;

                if (!string.IsNullOrWhiteSpace(vm.Comments))
                {
                    labor.Comments = vm.Comments.Length > 200 ? vm.Comments.Substring(0, 200) : vm.Comments;
                }
                else
                {
                    labor.Comments = null;
                }

                if (vm.NoCharacteristics)
                {
                    labor.Characteristics = new List<CharacteristicOfLabor>();
                }
                else
                {
                    var ids = (vm.SelectedCharacteristicIds ?? Enumerable.Empty<int>())
                              .Distinct()
                              .Select(i => (byte)Convert.ToByte(i))
                              .ToList();

                    var entities = _repository.CharacteristicOfLabors
                        .Where(c => ids.Contains(c.CharacteristicId))
                        .ToList();

                    labor.Characteristics = entities;
                }

                if (vm.NoMaternalMorbidities)
                {
                    labor.MaternalMorbidities = new List<MaternalMorbidity>();
                }
                else
                {
                    var ids = (vm.SelectedMaternalMorbidityIds ?? Enumerable.Empty<int>())
                              .Distinct()
                              .Select(i => (byte)Convert.ToByte(i))
                              .ToList();

                    var entities = _repository.MaternalMorbidities
                        .Where(m => ids.Contains(m.MaternalMorbidityId))
                        .ToList();

                    labor.MaternalMorbidities = entities;
                }

                if (vm.SelectedOnsetOfLaborIds != null)
                {
                    var ids = vm.SelectedOnsetOfLaborIds
                        .Distinct()
                        .Select(i => (byte)Convert.ToByte(i))
                        .ToList();

                    var entities = _repository.OnsetOfLabors
                        .Where(o => ids.Contains(o.OnsetOfLaborId))
                        .ToList();

                    labor.OnsetOfLabors = entities;
                }
                else
                {
                    // labor.OnsetOfLabors = new List<OnsetOfLabor>();

                }

                if (isNew)
                {
                    _repository.AddLaborAndDelivery(labor);
                    if (newborn.LaborAndDeliveries == null) newborn.LaborAndDeliveries = new List<LaborAndDelivery>();
                    if (!newborn.LaborAndDeliveries.Any(ld => ld.LaborAndDeliveryId == labor.LaborAndDeliveryId))
                        newborn.LaborAndDeliveries.Add(labor);
                }
                else
                {
                    _repository.EditLaborAndDelivery(labor);
                }

                return Json(new
                {
                    success = true,
                    message = "Labor & Delivery saved successfully.",
                    birthId = birth.BirthId,
                    newbornId = newborn.NewbornId,
                    laborAndDeliveryId = labor.LaborAndDeliveryId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR in SaveLaborAndDelivery: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null) Console.WriteLine($"Inner: {ex.InnerException.Message}");

                return Json(new { success = false, message = $"Error saving Labor & Delivery: {ex.Message}" });
            }
        }


        #endregion

        #region Newborn Section

        private Birth GetBirthWithNewbornDetails(int? birthId)
        {
            return _repository.Births
                .AsSplitQuery()
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.NewbornMrnNavigation)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.AbnormalConditions)
                .Include(b => b.Newborns)
                    .ThenInclude(n => n.CongenitalAnomalies)
                .FirstOrDefault(b => b.BirthId == birthId);
        }

        private IQueryable<Patient> SearchNewbornQuery(string query)
        {
            string queryNormalized = query.ToLower();
            string querySsnNormalized = query.Replace("-", "").Replace(" ", ""); // Remove dashes and spaces

            return _repository.Patients
                .Where(patient =>
                    // Uncomment for production when recent newborns are added to the system
                    // patient.Dob > DateTime.Now.AddYears(-1) &&
                    (patient.FirstName.Contains(queryNormalized) ||
                    patient.LastName.Contains(queryNormalized) ||
                    patient.Mrn.Contains(queryNormalized) ||
                    patient.Ssn.Contains(querySsnNormalized)) // Use normalized SSN for search
                );
        }

        private Newborn GetNewbornDetailsQuery(string mrn)
        {
            return _repository.Newborns
                .Include(newborn => newborn.NewbornMrnNavigation)
                .Include(newborn => newborn.AbnormalConditions)
                .Include(newborn => newborn.CongenitalAnomalies)
                .FirstOrDefault(newborn => newborn.NewbornMrn == mrn);
        }

       [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult SaveNewbornInformation([FromBody] NewbornViewModel model)
{
    try
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = $"Validation failed: {string.Join(", ", errors)}" });
        }

        if (!model.BirthId.HasValue || model.BirthId.Value <= 0)
        {
            return Json(new { success = false, message = "Birth ID is required." });
        }

        Birth birth = GetBirthWithNewbornDetails(model.BirthId);
        if (birth == null)
        {
            return Json(new { success = false, message = $"Birth record with ID {model.BirthId} not found." });
        }

        // 1. Resolve the existing newborn first
        Newborn existingNewborn = null;
        if (model.NewbornId.HasValue && model.NewbornId.Value > 0)
        {
            existingNewborn = birth.Newborns.FirstOrDefault(n => n.NewbornId == model.NewbornId);
            if (existingNewborn == null)
            {
                return Json(new { success = false, message = $"Newborn record with ID {model.NewbornId} not found." });
            }
        }
        else if (!string.IsNullOrWhiteSpace(model.NewbornMrn))
        {
            existingNewborn = birth.Newborns.FirstOrDefault(n => n.NewbornMrn == model.NewbornMrn);
        }

        // 2. Map ViewModel to Entity
        var newborn = model.ToEntity(existingNewborn);

        // 3. Handle PersonId assignment directly on the NEWBORN entity
        if (!string.IsNullOrWhiteSpace(model.NewbornMrn))
        {
            // Scenario A: Existing patient with MRN
            var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == model.NewbornMrn);
            if (patient == null)
            {
                return Json(new { success = false, message = $"Patient with MRN {model.NewbornMrn} not found." });
            }
            
            // Assign PersonId to the infant, not the birth
            newborn.PersonId = patient.PersonId; 
        }
        else
        {
            // Scenario B: No MRN. Create a Person if they don't have one.
            if (newborn.PersonId == null)
            {
                var newPerson = new Person
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Dob = model.DateAndTimeOfBirth,
                    SexId = model.SexId
                    // Set other default required fields for your Person table here if necessary
                };

                _repository.AddPerson(newPerson);
                
                // Assign newly generated PersonId to the infant
                newborn.PersonId = newPerson.PersonId; 
            }
        }

        // 4. Finalize Add/Edit logic
        if (existingNewborn == null)
        {
            _repository.AddNewborn(newborn);

            if (birth.Newborns == null)
                birth.Newborns = new List<Newborn>();
                
            birth.Newborns.Add(newborn);
        }

        if (model.Plurality.HasValue)
        {
            birth.Plurality = model.Plurality;
        }

        var selectedAbnormalConditions = model.NoAbnormalities
            ? new List<AbnormalCondition>()
            : _repository.AbnormalConditions
                .Where(ac => model.SelectedAbnormalConditionIds.Contains(ac.AbnormalConditionId))
                .ToList();

        var selectedCongenitalAnomalies = model.NoCongenitalAnomalies
            ? new List<CongenitalAnomaly>()
            : _repository.CongenitalAnomalies
                .Where(ca => model.SelectedCongenitalAnomalyIds.Contains(ca.CongenitalAnomalyId))
                .ToList();

        model.UpdateNewbornConditions(newborn, selectedAbnormalConditions, selectedCongenitalAnomalies);

        _repository.EditBirth(birth);
        _repository.EditNewborn(newborn);

        return Json(new
        {
            success = true,
            message = "Newborn information saved successfully",
            newbornId = newborn.NewbornId,
            birthId = birth.BirthId,
            plurality = birth.Plurality,
            dateAndTimeOfBirth = newborn.DateAndTimeOfBirth?.ToString("yyyy-MM-ddTHH:mm:ss")
        });
    }
    catch (Exception ex)
    {
        return Json(new
        {
            success = false,
            message = $"Error saving newborn information: {ex.Message}"
        });
    }
}
        [HttpGet]
        public IActionResult SearchNewborn(string query, int? birthId = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query can not be empty. Please enter a valid query to search for a newborn.");
            }

            // Get MRNs that are already used as newborns in OTHER births
            var existingNewbornMrns = _repository.Newborns
                .Where(n => !birthId.HasValue || n.BirthId != birthId.Value)
                .Select(n => n.NewbornMrn)
                .ToList();

            List<Patient> searchResult = [.. SearchNewbornQuery(query)
                .Where(p => !existingNewbornMrns.Contains(p.Mrn))
                .Take(10)];

            if (searchResult.Count == 0)
            {
                return NotFound("No newborns could be found try adjusting your query.");
            }

            var newborns = searchResult.Select(patient => new
            {
                mrn = patient.Mrn ?? "",
                firstName = patient.FirstName ?? "",
                middleName = patient.MiddleName ?? "",
                lastName = patient.LastName ?? "",
                dob = patient.Dob?.ToString("yyyy-MM-dd") ?? "",
                ssn = patient.Ssn ?? ""
            }).ToList();

            return Json(newborns);
        }

       [HttpGet]
public IActionResult NewbornDetails(int? personId, string mrn, string mode = "edit", int? birthId = null)
{
    // 1. Sanitize frontend JavaScript quirks
    if (mrn == "undefined" || mrn == "null") mrn = null;

    // 2. Validate that we have at least one identifier
    if (!personId.HasValue && string.IsNullOrEmpty(mrn))
        return BadRequest("A Person ID or MRN is required.");

    // 3. Variables to hold our demographic data
    string fName = "", mName = "", lName = "";
    DateTime? dob = null;
    byte? sexId = null;

    // 4. Fetch the existing demographic record (Patient or Person)
    if (!string.IsNullOrEmpty(mrn))
    {
        var patient = _repository.Patients.FirstOrDefault(p => p.Mrn == mrn);
        if (patient == null) return NotFound($"No patient found with MRN {mrn}");
        
        fName = patient.FirstName;
        mName = patient.MiddleName;
        lName = patient.LastName;
        dob = patient.Dob;
        sexId = patient.SexId;
    }
    else if (personId.HasValue)
    {
        // Fallback to Person record if no MRN exists
        var person = _repository.Persons.FirstOrDefault(p => p.PersonId == personId.Value);
        if (person == null) return NotFound($"No person found with ID {personId}");

        fName = person.FirstName;
        mName = person.MiddleName;
        lName = person.LastName;
        dob = person.Dob;
        sexId = person.SexId;
    }

    NewbornViewModel viewModel;

    // 5. Handle existing Birth/Newborn records
    if (birthId.HasValue)
    {
        var birth = _repository.Births
            .Include(b => b.Newborns).ThenInclude(n => n.NewbornMrnNavigation)
            .Include(b => b.Newborns).ThenInclude(n => n.AbnormalConditions)
            .Include(b => b.Newborns).ThenInclude(n => n.CongenitalAnomalies)
            .FirstOrDefault(b => b.BirthId == birthId.Value);

        // Check by MRN first, fallback to PersonId if MRN is null
        var existingNewborn = birth?.Newborns?.FirstOrDefault(n => 
            (!string.IsNullOrEmpty(mrn) && n.NewbornMrn == mrn) || 
            (personId.HasValue && n.PersonId == personId.Value));

        if (existingNewborn != null)
        {
            viewModel = new NewbornViewModel(existingNewborn);
        }
        else
        {
            viewModel = new NewbornViewModel
            {
                PersonId = personId,
                NewbornMrn = mrn,
                FirstName = fName,
                MiddleName = mName,
                LastName = lName,
                DateAndTimeOfBirth = dob,
                SexId = sexId,
                IsExistingPatient = !string.IsNullOrEmpty(mrn),
                BirthId = birthId
            };
        }
    }
    else
    {
        viewModel = new NewbornViewModel
        {
            PersonId = personId,
            NewbornMrn = mrn,
            FirstName = fName,
            MiddleName = mName,
            LastName = lName,
            DateAndTimeOfBirth = dob,
            SexId = sexId,
            IsExistingPatient = !string.IsNullOrEmpty(mrn)
        };
    }

    _dropdownService.PopulateNewbornDropdowns(viewModel);
    ViewBag.Mode = mode;

    return PartialView("Newborn/_NewbornAccordionSections", viewModel);
}
        #endregion
        #region Finalize Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveFinalizeData([FromBody] FinalizeViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return Json(new { success = false, message = "Finalization data is required" });
                }

                if (!viewModel.BirthId.HasValue || viewModel.BirthId.Value <= 0)
                {
                    return Json(new { success = false, message = "Birth record not found" });
                }

                if (string.IsNullOrWhiteSpace(viewModel.RegistrarSignature))
                {
                    return Json(new { success = false, message = "Registrar signature is required" });
                }

                var birth = _repository.Births
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                    .Include(b => b.Prenatals)
                    .Include(b => b.MotherMrnNavigation)
                    .FirstOrDefault(b => b.BirthId == viewModel.BirthId.Value);

                if (birth == null)
                {
                    return Json(new { success = false, message = "Birth record not found" });
                }

                // Update the birth record
                viewModel.UpdateBirthRecord(birth);
                _repository.EditBirth(birth);

                return Json(new
                {
                    success = true,
                    message = "Birth record finalized successfully",
                    birthId = birth.BirthId,
                    finalizedDate = DateTime.Now.ToString("MMMM dd, yyyy 'at' h:mm tt"),
                    registrarSignature = viewModel.RegistrarSignature
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error finalizing birth record: {ex.Message}"
                });
            }
        }

        #endregion
    }
}