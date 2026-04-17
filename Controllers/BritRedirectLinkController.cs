using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Web;

namespace IS_Proj_HIT.Controllers
{
    [Route("appsettings")]
    public class BritRedirectLinkController : BaseController
    {
        protected readonly IConfiguration _configuration;

        public BritRedirectLinkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Britredirect")]
        public IActionResult BritRedirect()
        {
            return View("~/Views/BritSystems/index.cshtml");
        }
        
        
    }
}
