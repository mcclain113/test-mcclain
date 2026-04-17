using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IS_Proj_HIT.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.SessionTimeoutSeconds = (int)TimeSpan.FromMinutes(60).TotalSeconds;
            base.OnActionExecuting(context);
        }
    }
}