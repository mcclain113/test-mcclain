using IS_Proj_HIT.ViewModels.BirthRegistry;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IS_Proj_HIT.Services
{
    public interface IBirthRegistryDropdownDataService
    {
        #region Shared Dropdowns
        List<SelectListItem> GetReligions();
        List<SelectListItem> GetEthnicities();
        List<SelectListItem> GetEducationLevels();
        List<SelectListItem> GetRaces();
        List<SelectListItem> GetStates();
        List<SelectListItem> GetCountries();
        #endregion

        #region Facility Dropdowns
        List<SelectListItem> GetFacilities();
        List<SelectListItem> GetBirthPlaceTypes();
        List<SelectListItem> GetPhysicians();
        List<SelectListItem> GetAttendantTitles();
        #endregion

        #region Prenatal Dropdowns
        List<SelectListItem> GetPregnancyInfections();
        List<SelectListItem> GetPregnancyRiskFactors();
        #endregion

        #region Labor & Delivery Dropdowns
        List<SelectListItem> GetCharacteristicsOfLabor();
        List<SelectListItem> GetFetalPresentationAtBirths();
        List<SelectListItem> GetFinalRouteAndMethodOfDeliveries();
        List<SelectListItem> GetMaternalMorbidities();
        List<SelectListItem> GetOnsetOfLabors();
        #endregion

        #region Newborn Dropdowns
        List<SelectListItem> GetSexOptions();
        List<SelectListItem> GetAbnormalConditions();
        List<SelectListItem> GetCongenitalAnomalies();
        #endregion

        #region ViewModel Population Methods
        void PopulateFacilityDropdowns(BirthFacilityViewModel viewModel);
        void PopulateMotherDropdowns(MothersInformationViewModel viewModel);
        void PopulateFatherDropdowns(FatherInformationViewModel viewModel);
        void PopulatePrenatalDropdowns(PrenatalCareViewModel viewModel);
        void PopulateLaborDropdowns(LaborAndDeliveryViewModel viewModel);
        void PopulateNewbornDropdowns(NewbornViewModel viewModel);
        #endregion
    }
}