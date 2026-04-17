using System.Linq;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class BirthRegistryViewModel
    {
        public int? BirthId { get; set; }
        public bool IsComplete { get; set; }
        public BirthFacilityViewModel FacilityViewModel { get; set; }
        public MothersInformationViewModel MothersInformationViewModel { get; set; }
        public FatherInformationViewModel FatherInformationViewModel { get; set; }
        public PrenatalCareViewModel PrenatalCareViewModel { get; set; }
        public LaborAndDeliveryViewModel LaborAndDeliveryViewModel { get; set; }
        public NewbornViewModel NewbornViewModel { get; set; }
        public FinalizeViewModel FinalizeViewModel { get; set; }

        public string MotherFullName
        {
            get
            {
                if (MothersInformationViewModel == null) return null;

                var parts = new[]
                {
                    MothersInformationViewModel.FirstName,
                    MothersInformationViewModel.MiddleName,
                    MothersInformationViewModel.LastName
                };

                return string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
            }
        }

        public string MotherMrn => MothersInformationViewModel?.Mrn;

        public string MotherDateOfBirth => MothersInformationViewModel?.DateOfBirth?.ToString("MM/dd/yyyy");

        public string NewbornFullName
        {
            get
            {
                if (NewbornViewModel == null || string.IsNullOrWhiteSpace(NewbornViewModel.FirstName))
                    return null;

                var parts = new[]
                {
                    NewbornViewModel.FirstName,
                    NewbornViewModel.MiddleName,
                    NewbornViewModel.LastName
                };

                return string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
            }
        }

        public string NewbornMrn => NewbornViewModel?.NewbornMrn;

        public string NewbornDateOfBirth => NewbornViewModel?.DateAndTimeOfBirth?.ToString("MM/dd/yyyy hh:mm tt");

        public bool HasMotherInformation => !string.IsNullOrWhiteSpace(MotherFullName);

        public bool HasNewbornInformation => !string.IsNullOrWhiteSpace(NewbornFullName);
    }
}