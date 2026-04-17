using System.Collections.Generic;

namespace IS_Proj_HIT.ViewModels.Account
{
    public class RoleFacilitySelectionViewModel
{
    public List<IdentityRoleViewModel> Roles { get; set; }
    public List<FacilityViewModel> Facilities { get; set; }
    public string SelectedRole { get; set; }
    public int SelectedFacility { get; set; }
}

    public class IdentityRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class FacilityViewModel
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
    }

}