using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IS_Proj_HIT.Controllers
{
    [Authorize]
    [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete","SecurityAssignRole","SecurityRemoveRole")]
    public class AdministrationController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IWCTCHealthSystemRepository repo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _repository = repo;
        }

        /// <summary>
        ///     Display the Admin tools index
        /// </summary>
        [Authorize]
        [PermissionAuthorize("StructuredDataAdd","StructuredDataView","StructuredDataEdit","StructuredDataDelete","SecurityAssignRole","SecurityRemoveRole")]
        public IActionResult Index()
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            return View();
        }

    }
}

