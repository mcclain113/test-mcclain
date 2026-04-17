using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace IS_Proj_HIT.Controllers
{
    public class SessionController : BaseController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KeepAlive()
        {
            // Touch the session so IdleTimeout is extended
            HttpContext.Session.SetString("LastKeepAlive", DateTime.UtcNow.ToString("o"));
            return Ok(new { kept = true });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Expired()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Sign out the authentication cookie
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return View();

        }
    }
}
