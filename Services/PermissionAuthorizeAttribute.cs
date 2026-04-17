using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace IS_Proj_HIT.Services
{
    public class PermissionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;

        public PermissionAuthorizeAttribute(params string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissions = context.HttpContext.Session.GetString("Permissions")?.Split(',');

            if (permissions == null)
            {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n*** Permissions not found in session ***\n");
                    Console.ResetColor();
                context.Result = new ForbidResult();
                return;
            }

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"\n Permissions in session: {string.Join(", ", permissions)}");
            Console.WriteLine($" Required permissions: {string.Join(", ", _requiredPermissions)} \n");
                Console.ResetColor();

            if (!_requiredPermissions.Any(required => permissions.Contains(required)))
            {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"\n Authorization failed. User does not have the required permission: {string.Join(", ", _requiredPermissions)} \n");
                    Console.ResetColor();
                context.Result = new ForbidResult();
            }
        }

            // create a helper method for use in providing In Session permissions to a view
        public static List<string> GetPermissionsForUser(HttpContext httpContext)
        {
            // Get permissions from session (or wherever they are stored)
            var permissions = httpContext.Session.GetString("Permissions")?.Split(',').ToList();
            return permissions ?? new List<string>();
        }

    }
}