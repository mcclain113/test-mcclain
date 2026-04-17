namespace IS_Proj_HIT.Controllers;
using System.IO;
using Microsoft.AspNetCore.Mvc;

public class RevenueCycleController : BaseController
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ExportReport()
    {
        return View();
    }
}