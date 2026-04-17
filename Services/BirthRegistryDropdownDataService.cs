using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.ViewModels.BirthRegistry;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace IS_Proj_HIT.Services
{
    public class BirthRegistryDropdownDataService : IBirthRegistryDropdownDataService
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public BirthRegistryDropdownDataService(IWCTCHealthSystemRepository repository)
        {
            _repository = repository;
        }

        #region Shared Dropdowns

        public List<SelectListItem> GetReligions()
        {
            return [.. _repository.Religions
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.ReligionId.ToString(),
                    Text = r.Name
                })];
        }

        public List<SelectListItem> GetEthnicities()
        {
            return [.. _repository.Ethnicities
                .OrderBy(e => e.Name)
                .Select(e => new SelectListItem
                {
                    Value = e.EthnicityId.ToString(),
                    Text = e.Name
                })];
        }

        public List<SelectListItem> GetEducationLevels()
        {
            return [.. _repository.EducationLevels
                .OrderBy(e => e.EducationLevelName)
                .Select(e => new SelectListItem
                {
                    Value = e.EducationId.ToString(),
                    Text = e.EducationLevelName
                })];
        }

        public List<SelectListItem> GetRaces()
        {
            return [.. _repository.Races
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.RaceId.ToString(),
                    Text = r.Name
                })];
        }

        public List<SelectListItem> GetStates()
        {
            return [.. _repository.AddressStates
                .OrderBy(s => s.StateName)
                .Select(s => new SelectListItem
                {
                    Value = s.StateID.ToString(),
                    Text = s.StateName
                })];
        }

        public List<SelectListItem> GetCountries()
        {
            return [.. _repository.Countries
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.Name
                })];
        }

        #endregion

        #region Facility Dropdowns

        public List<SelectListItem> GetFacilities()
        {
            return [.. _repository.Facilities
                .OrderBy(f => f.Name)
                .Select(f => new SelectListItem
                {
                    Value = f.FacilityId.ToString(),
                    Text = f.Name,
                    Selected = f.FacilityId == 4
                })];
        }

        public List<SelectListItem> GetBirthPlaceTypes()
        {
            return [.. _repository.BirthPlaceTypes
                .OrderBy(bpt => bpt.BirthPlaceTypeName)
                .Select(bpt => new SelectListItem
                {
                    Value = bpt.BirthPlaceTypeId.ToString(),
                    Text = bpt.BirthPlaceTypeName
                })];
        }

        public List<SelectListItem> GetPhysicians()
        {
            return [.. _repository.Physicians
                .Where(p => p.ProviderStatusId == 1)
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                .Select(p => new SelectListItem
                {
                    Value = p.PhysicianId.ToString(),
                    Text = $"{p.FirstName} {p.LastName} - {p.Credentials}"
                })];
        }

        public List<SelectListItem> GetAttendantTitles()
        {
            return new List<SelectListItem>
            {
                new() { Value = "M.D. (Doctor of Medicine)", Text = "M.D. (Doctor of Medicine)" },
                new() { Value = "D.O. (Doctor of Osteopathy)", Text = "D.O. (Doctor of Osteopathy)" },
                new() { Value = "CNM/CM (Certified Nurse Midwife / Certified Midwife)", Text = "CNM/CM (Certified Nurse Midwife / Certified Midwife)" },
                new() { Value = "Other Midwife (any other than CNM/CM)", Text = "Other Midwife (any other than CNM/CM)" },
                new() { Value = "Other", Text = "Other" }
            };
        }

        #endregion

        #region Prenatal Dropdowns

        public List<SelectListItem> GetPregnancyInfections()
        {
            return [.. _repository.PregnancyInfections
                .OrderBy(pi => pi.InfectionName)
                .Select(pi => new SelectListItem
                {
                    Value = pi.InfectionId.ToString(),
                    Text = pi.InfectionName,
                    Group = !string.IsNullOrEmpty(pi.InfectionDescription)
                        ? new SelectListGroup { Name = pi.InfectionDescription }
                        : null
                })];
        }

        public List<SelectListItem> GetPregnancyRiskFactors()
        {
            return [.. _repository.PregnancyRiskFactors
                .OrderBy(prf => prf.RiskFactorName)
                .Select(prf => new SelectListItem
                {
                    Value = prf.RiskFactorId.ToString(),
                    Text = prf.RiskFactorName,
                    Group = !string.IsNullOrEmpty(prf.RiskFactorDescription)
                        ? new SelectListGroup { Name = prf.RiskFactorDescription }
                        : null
                })];
        }

        #endregion

        #region Labor & Delivery Dropdowns

        public List<SelectListItem> GetCharacteristicsOfLabor()
        {
            return [.. _repository.CharacteristicOfLabors
                .OrderBy(col => col.CharacteristicName)
                .Select(col => new SelectListItem
                {
                    Value = col.CharacteristicId.ToString(),
                    Text = col.CharacteristicName,
                    Group = !string.IsNullOrEmpty(col.CharacteristicDescription)
                        ? new SelectListGroup { Name = col.CharacteristicDescription }
                        : null
                })];
        }

        public List<SelectListItem> GetFetalPresentationAtBirths()
        {
            return [.. _repository.FetalPresentationAtBirths
                .OrderBy(fpab => fpab.FetalPresentationName)
                .Select(fpab => new SelectListItem
                {
                    Value = fpab.FetalPresentationAtBirthId.ToString(),
                    Text = fpab.FetalPresentationName,
                    Group = !string.IsNullOrEmpty(fpab.FetalPresentationDescription)
                        ? new SelectListGroup { Name = fpab.FetalPresentationDescription }
                        : null
                })];
        }

        public List<SelectListItem> GetFinalRouteAndMethodOfDeliveries()
        {
            return [.. _repository.FinalRouteAndMethodOfDeliveries
                .OrderBy(framod => framod.FinalRouteAndMethodName)
                .Select(framod => new SelectListItem
                {
                    Value = framod.FinalRouteAndMethodId.ToString(),
                    Text = framod.FinalRouteAndMethodName,
                    Group = !string.IsNullOrEmpty(framod.FinalRouteAndMethodDescription)
                        ? new SelectListGroup { Name = framod.FinalRouteAndMethodDescription }
                        : null
                })];
        }

        public List<SelectListItem> GetMaternalMorbidities()
        {
            return [.. _repository.MaternalMorbidities
                .OrderBy(mm => mm.MaternalMorbidityName)
                .Select(mm => new SelectListItem
                {
                    Value = mm.MaternalMorbidityId.ToString(),
                    Text = mm.MaternalMorbidityName,
                    Group = !string.IsNullOrEmpty(mm.MaternalMorbidityDescription)
                        ? new SelectListGroup { Name = mm.MaternalMorbidityDescription }
                        : null
                })];
        }

        public List<SelectListItem> GetOnsetOfLabors()
        {
            return [.. _repository.OnsetOfLabors
                .OrderBy(ool => ool.OnsetOfLaborName)
                .Select(ool => new SelectListItem
                {
                    Value = ool.OnsetOfLaborId.ToString(),
                    Text = ool.OnsetOfLaborName,
                    Group = !string.IsNullOrEmpty(ool.OnsetOfLaborDescription)
                        ? new SelectListGroup { Name = ool.OnsetOfLaborDescription }
                        : null
                })];
        }

        #endregion

        #region Newborn Dropdowns

        public List<SelectListItem> GetSexOptions()
        {
            return [.. _repository.Sexes
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.SexId.ToString(),
                    Text = s.Name
                })];
        }

        public List<SelectListItem> GetAbnormalConditions()
        {
            return [.. _repository.AbnormalConditions
                .OrderBy(ac => ac.AbnormalConditionName)
                .Select(ac => new SelectListItem
                {
                    Value = ac.AbnormalConditionId.ToString(),
                    Text = ac.AbnormalConditionName,
                    Group = !string.IsNullOrEmpty(ac.AbnormalConditionDescription)
                        ? new SelectListGroup { Name = ac.AbnormalConditionDescription }
                        : null
                })];
        }

        public List<SelectListItem> GetCongenitalAnomalies()
        {
            return [.. _repository.CongenitalAnomalies
                .OrderBy(ca => ca.CongenitalAnomalyName)
                .Select(ca => new SelectListItem
                {
                    Value = ca.CongenitalAnomalyId.ToString(),
                    Text = ca.CongenitalAnomalyName,
                    Group = !string.IsNullOrEmpty(ca.CongenitalAnomalyDescription)
                        ? new SelectListGroup { Name = ca.CongenitalAnomalyDescription }
                        : null
                })];
        }

        #endregion

        #region ViewModel Population Methods

        public void PopulateFacilityDropdowns(BirthFacilityViewModel viewModel)
        {
            if (viewModel == null) return;
            
            viewModel.Facilities = GetFacilities();
            viewModel.BirthPlaceTypes = GetBirthPlaceTypes();
            viewModel.Physicians = GetPhysicians();
            viewModel.AttendantTitles = GetAttendantTitles();
        }

        public void PopulateMotherDropdowns(MothersInformationViewModel viewModel)
        {
            if (viewModel == null) return;

            // Clear lists to prevent duplicates
            viewModel.Religions = new List<SelectListItem>();
            viewModel.Ethnicities = new List<SelectListItem>();
            viewModel.EducationLevels = new List<SelectListItem>();
            viewModel.Races = new List<SelectListItem>();
            viewModel.States = new List<SelectListItem>();
            viewModel.Countries = new List<SelectListItem>();

            viewModel.Religions = GetReligions();
            viewModel.Ethnicities = GetEthnicities();
            viewModel.EducationLevels = GetEducationLevels();
            viewModel.Races = GetRaces();
            viewModel.States = GetStates();
            viewModel.Countries = GetCountries();
        }

        public void PopulateFatherDropdowns(FatherInformationViewModel viewModel)
        {
            if (viewModel == null) return;
            
            viewModel.EducationLevels = GetEducationLevels();
            viewModel.Ethnicities = GetEthnicities();
            viewModel.Races = GetRaces();
        }

        public void PopulatePrenatalDropdowns(PrenatalCareViewModel viewModel)
        {
            if (viewModel == null) return;
            
            viewModel.PregnancyInfections = GetPregnancyInfections();
            viewModel.PregnancyRiskFactors = GetPregnancyRiskFactors();
        }

        public void PopulateLaborDropdowns(LaborAndDeliveryViewModel viewModel)
        {
            if (viewModel == null) return;
            
            viewModel.CharacteristicsOfLabor = GetCharacteristicsOfLabor();
            viewModel.FetalPresentationAtBirths = GetFetalPresentationAtBirths();
            viewModel.FinalRouteAndMethodOfDeliveries = GetFinalRouteAndMethodOfDeliveries();
            viewModel.MaternalMorbidities = GetMaternalMorbidities();
            viewModel.OnsetOfLabors = GetOnsetOfLabors();
        }

        public void PopulateNewbornDropdowns(NewbornViewModel viewModel)
        {
            if (viewModel == null) return;
            
            viewModel.SexOptions = GetSexOptions();
            viewModel.AbnormalConditions = GetAbnormalConditions();
            viewModel.CongenitalAnomalies = GetCongenitalAnomalies();
        }

        #endregion
    }
}