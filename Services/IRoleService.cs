using System.Security.Claims; // For ClaimsPrincipal
using System.Threading.Tasks; // For async/Task

namespace IS_Proj_HIT.Services
{
    public interface IRoleService
    {
        Task<bool> UserHasRolesAsync(ClaimsPrincipal user);
    }
}