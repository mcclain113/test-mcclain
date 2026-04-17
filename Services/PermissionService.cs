using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;

namespace IS_Proj_HIT.Services
{
    public class PermissionService
    {
        private readonly WCTCHealthSystemContext _context;
        public PermissionService(WCTCHealthSystemContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetPermissionsForUserAsync(UserTable user)
        {
            // Fetch the user with related roles
            var userWithRoles = await _context.UserTables
                .Include(u => u.AspNetUsers) // Include AspNetUsers navigation
                    .ThenInclude(a => a.AspNetUserRoles) // Include AspNetUserRoles navigation
                    .ThenInclude(ur => ur.Role) // Include the Role navigation
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            // Ensure roles are loaded
            if (userWithRoles?.AspNetUsers?.AspNetUserRoles == null)
            {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n ***AspNetUserRoles is null or not loaded***");
                    Console.ResetColor();
                return new List<string>();
            }

            // Extract role names
            var roles = userWithRoles.AspNetUsers.AspNetUserRoles
                .Select(ur => ur.Role.Name)
                .ToList();

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"\n  Roles for user: {string.Join(", ", roles)}\n ");
                Console.ResetColor();

            // Fetch permissions for those roles
            var permissions = await _context.AspNetRolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => roles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"\n Permissions for roles: {string.Join(", ", permissions)}\n");
                Console.ResetColor();

            return permissions;
        }

        public async Task<List<string>> GetPermissionsForRoleAsync(string roleName)
        {
            // Ensure the roleName is valid
            if (string.IsNullOrEmpty(roleName))
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n ***RoleName is null or empty***");
                    Console.ResetColor();
                return new List<string>();
            }

            // Fetch the permissions for the given role
            var permissions = await _context.AspNetRolePermissions
                .Include(rp => rp.Permission) // Include related Permission entity
                .Where(rp => rp.Role.Name == roleName) // Match the role by name
                .Select(rp => rp.Permission.Name) // Select the permission names
                .ToListAsync();

            // Log the retrieved permissions
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"\n Permissions for role '{roleName}': {string.Join(", ", permissions)}\n");
                Console.ResetColor();

            return permissions;
        }
    }

}