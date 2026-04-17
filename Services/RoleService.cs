using System.Security.Claims; // For ClaimsPrincipal
using System.Threading.Tasks; // For async/Task
using Microsoft.AspNetCore.Identity; // For UserManager and IdentityUser
using System.Linq; // Required for LINQ methods like Any()

namespace IS_Proj_HIT.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> UserHasRolesAsync(ClaimsPrincipal user)
        {
            if (user == null || !(user.Identity?.IsAuthenticated ?? false))
            {
                // User is not authenticated, return false (no roles assigned)
                return false;
            }

            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null)
            {
                // No user found, return false
                return false;
            }

            var roles = await _userManager.GetRolesAsync(appUser);
            return roles.Any(); // returns true
        }
    }
}