using System.Linq;
using System.Security.Claims;
using IS_Proj_HIT.Entities.Data;
using Microsoft.AspNetCore.Identity;
public static class UserHelper
{
    public static int GetEditedBy(IWCTCHealthSystemRepository repository, UserManager<IdentityUser> userManager, ClaimsPrincipal user)
    {
        return repository.UserTables
                         .Where(x => x.AspNetUsersId == userManager.GetUserId(user))
                         .Select(x => x.UserId)
                         .FirstOrDefault();
    }
}