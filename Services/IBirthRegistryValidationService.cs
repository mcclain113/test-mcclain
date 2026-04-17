using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Services
{
    public interface IBirthRegistryValidationService
    {
        bool IsRegistryComplete(Birth birth);
    }
}