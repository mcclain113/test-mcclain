using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IS_Proj_HIT.Services;

namespace IS_Proj_HIT.Controllers
{

    public class HomeController : BaseController
    {       
        public IConfiguration Configuration { get; }

        public IActionResult Index()
        {
                // Fetch User Session permissions using the helper method
	        var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
	            // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View();
        }
        public IActionResult Privacy()
        {
            ViewBag.privacy = "privacy";
            return View("Privacy");
        }
    }
}
