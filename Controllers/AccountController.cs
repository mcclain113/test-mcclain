using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.ViewModels.Account;
using IS_Proj_HIT.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography; // For SHA256
using System.Security.Claims;
using System.Text; // For Encoding

namespace IS_Proj_HIT.Controllers
{
    public class AccountController : BaseController
    {
        private IWCTCHealthSystemRepository _repository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly PermissionService _permissionService;

        public AccountController(IWCTCHealthSystemRepository repo,
                                SignInManager<IdentityUser> signInManager, 
                                UserManager<IdentityUser> userManager, 
                                ILogger<AccountController> logger,
                                PermissionService permissionService)
        {
            _repository = repo;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _permissionService = permissionService;
        }

        #region Login   // begin user login region
        /// <summary>
        ///     User Login - Getter
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null, string error = null)
        {
            // if the user clicks the 'Forgot My Password' without entering their email, this will activate
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"]= error;   // the value of 'error' is passed from Login.cshtml from the script
            }

            // If the user is authenticated, handle permissions and redirect
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                if (!string.IsNullOrEmpty(userEmail))
                {
                    // Fetch user data from the database
                    var user = await _repository.UserTables
                        .Include(u => u.AspNetUsers)
                        .FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (user != null)
                    {
                            // Retrieve permissions for the authenticated user
                        var permissions = await _permissionService.GetPermissionsForUserAsync(user);

                            // Log and store permissions
                        //     Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        // Console.WriteLine("Permissions (AccountController Login method): " + string.Join(", ", permissions));
                        //     Console.ResetColor();
                        HttpContext.Session.SetString("Permissions", string.Join(",", permissions));
                    }
                    else
                    {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("Authenticated user not found in the database.");
                            Console.ResetColor();
                    }

                    // Fetch roles and facilities for the authenticated user
                    var roles = _repository.AspNetUserRoles
                        .Where(ur => ur.UserId == user.AspNetUsersId)
                        .Select(ur => ur.RoleId)
                        .ToList();  // a list of role ids

                    var facilities = _repository.UserFacilities
                        .Where(uf => uf.UserId == user.UserId)
                        .Select(uf => uf.Facility)
                        .ToList();  // a list of Facility objects

                    if(facilities.Count == 1)
                    {
                            // if User has only one facility, store it now
                        HttpContext.Session.SetString("Facility", facilities[0].FacilityId.ToString());

                            // log the result for tracking/verification puroses only
                        var facilityIdFromSession = HttpContext.Session.GetString("Facility");
                        int facilityId = int.Parse(facilityIdFromSession);
                        var facilityObjectInSession = _repository.Facilities.FirstOrDefault(f => f.FacilityId == facilityId);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"\n Facility for user (EncounterController Login Method): {string.Join(", ", facilityObjectInSession.Name)}\n");
                            Console.ResetColor();
                    }

                    // Check for multiple roles or facilities
                    if (roles.Count > 1 || facilities.Count > 1)
                    {
                        // Redirect to selection page with available roles and facilities
                        var selectionViewModel = new RoleFacilitySelectionViewModel
                        {
                            Roles = roles.Select(r => new IdentityRoleViewModel
                            {
                                Id = r,
                                Name = _repository.AspNetRoles.FirstOrDefault(role => role.Id == r).Name
                            }).ToList(),
                            Facilities = facilities.Select(f => new FacilityViewModel
                            {
                                FacilityId = f.FacilityId,
                                Name = f.Name
                            }).ToList()
                        };

                        return View("SelectRoleAndFacility", selectionViewModel);
                    }
                }

                // otherwise redirect authenticated users to their default view
                return RedirectToAction("Index", "Home");
            }
            
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }


        /// <summary>
        ///     User Login - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);   // PasswordSignInAsync can return: .Succeeded, .Failed, .LockedOut, .RequiresTwoFactor
                if (result.Succeeded)
                {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("User logged in successfully.");
                        Console.ResetColor();

                    var user = _repository.UserTables
                        .Include(u => u.AspNetUsers)
                        .FirstOrDefault(u => u.Email == model.Email);

                    if (user != null)
                    {
                        // Fetch user's security questions
                        var userSecurityQuestions = _repository.UserSecurityQuestions
                            .Where(q => q.UserId == user.UserId).ToList();

                        // Check if any security questions match the placeholder values
                        var hasPlaceholderQuestions = userSecurityQuestions.Any(q =>
                            q.AnswerHash == HashText("PlaceholderAnswer"));

                        if (hasPlaceholderQuestions)
                        {
                            // Redirect to AddSecurityQuestions
                            return RedirectToAction("AddSecurityQuestions", "Account", new { id = user.UserId, returnUrl });
                        }

                        // Check if the password is the default password
                        var passwordHasher = new PasswordHasher<AspNetUser>();
                        var isDefaultPassword = passwordHasher.VerifyHashedPassword(
                            user.AspNetUsers,
                            user.AspNetUsers.PasswordHash,
                            "r3s3tP@ssw0rd") == PasswordVerificationResult.Success;

                        if (isDefaultPassword)
                        {
                            // Redirect to ResetPassword if default password is being used
                            return RedirectToAction("ResetPassword", "Account", new { requiresReset = true });
                        }
                        
                        // Fetch roles and facilities for the user
                        var roles = _repository.AspNetUserRoles
                            .Where(ur => ur.UserId == user.AspNetUsersId)
                            .Select(ur => ur.RoleId)
                            .ToList();  // a list of role ids

                        var facilities = _repository.UserFacilities
                            .Where(uf => uf.UserId == user.UserId)
                            .Select(uf => uf.Facility)
                            .ToList();  // a list of Facility objects

                        // Check for multiple roles or facilities
                        if (roles.Count > 1 || facilities.Count > 1)
                        {
                            // Redirect to selection page with available roles and facilities
                            var selectionViewModel = new RoleFacilitySelectionViewModel
                            {
                                Roles = roles.Select(r => new IdentityRoleViewModel
                                {
                                    Id = r,
                                    Name = _repository.AspNetRoles.FirstOrDefault(role => role.Id == r).Name
                                }).ToList(),
                                Facilities = facilities.Select(f => new FacilityViewModel
                                {
                                    FacilityId = f.FacilityId,
                                    Name = f.Name
                                }).ToList()
                            };

                            return View("SelectRoleAndFacility", selectionViewModel);
                        }
                        else if (roles.Count == 0)
                        {
                            // Roles not yet assigned, no access allowed
                            return RedirectToAction("RegisteredButWithoutRole");
                        }

                        // Otherwise, set the default role and facility
                        var defaultRoleId = roles.FirstOrDefault();
                        var defaultRole = _repository.AspNetRoles.FirstOrDefault(r => r.Id == defaultRoleId);
                        var defaultFacility = facilities.FirstOrDefault();

                        HttpContext.Session.SetString("Role", defaultRole.ToString());
                        HttpContext.Session.SetString("Facility", defaultFacility.FacilityId.ToString());

                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"\n Facilities for user: {string.Join(", ", facilities.Select(f => f.Name))}\n");
                            Console.ResetColor();

                        var permissions = await _permissionService.GetPermissionsForRoleAsync(defaultRole.Name);
                        HttpContext.Session.SetString("Permissions", string.Join(",", permissions));

                        return RedirectToLocal(returnUrl);
                    }
                }

                // for any result other than .Succeeded
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        #endregion  // end of the user login content

        /// <summary>
        ///     Setter method for user logout
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Access Denied - Getter
        /// </summary>
        /// <returns></returns>
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        ///     New User without Role - Getter
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisteredButWithoutRole()
        {
            return View();
        }

        #region Set Session Role, Facility
        /// <summary>
        ///     User with multiple Roles or Facilities selects which Role & Facility to apply in Session, 
        ///         then sets the Permissions in Session for that Role - Setter
        /// </summary>
        /// <param name="selectedRole"></param>
        /// <param name="selectedFacility"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetRoleAndFacility(string selectedRole, int selectedFacility)
        {
            if (!string.IsNullOrEmpty(selectedRole) && selectedFacility > 0)
            {
                HttpContext.Session.SetString("Role", selectedRole);
                HttpContext.Session.SetString("Facility", selectedFacility.ToString());

                var selectedFacilityObject = _repository.Facilities.FirstOrDefault(f => f.FacilityId == selectedFacility);

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"\n Facility for user: {string.Join(", ", selectedFacilityObject.Name)}\n");
                    Console.ResetColor();

                var selectedRoleId = selectedRole;
                var selectedRoleToApply = _repository.AspNetRoles.FirstOrDefault(r => r.Id == selectedRoleId);

                var permissions = _permissionService.GetPermissionsForRoleAsync(selectedRoleToApply.Name).Result;
                HttpContext.Session.SetString("Permissions", string.Join(",", permissions));

                var returnUrl = HttpContext.Session.GetString("ReturnUrl");
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid selection.");
            return View("SelectRoleAndFacility");
        }

        #endregion  // end of Set Role, Facility section

        #region Profile
        public IActionResult UpdateProfile()
        {
            //find current user
            var id = _userManager.GetUserId(HttpContext.User);

            //select the information I want to display
            var dbUser = _repository.UserTables.FirstOrDefault(u => u.AspNetUsersId == id);

            return View(dbUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(UserTable model)
        {
            if (!ModelState.IsValid) return View(model);

            if (string.IsNullOrWhiteSpace(model.AspNetUsersId))
                model.AspNetUsersId = _userManager.GetUserId(HttpContext.User);
            if (string.IsNullOrWhiteSpace(model.Email))
                model.Email = User.Identity?.Name;
            model.LastModified = DateTime.Now;
            _repository.EditUser(model);
            // return RedirectToAction("Index","Home");
            return RedirectToAction("UpdateProfile", new { id = model.UserId });
        }
        #endregion  // end of Profile region

        #region Register
        /// <summary>
        ///     Register a New User - Getter
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterViewModel
            {
                Programs = _repository.Programs
                    .OrderBy(p => p.Name)
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProgramId.ToString(),
                        Text = p.Name
                    }).ToList(),
                ReturnUrl = returnUrl
            };
            return View("Register", model);
        }

        /// <summary>
        ///     Register a new User - Setter
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            registerViewModel.Programs = _repository.Programs.Select(p => new SelectListItem
            {
                Value = p.ProgramId.ToString(),
                Text = p.Name
            }).ToList();

            registerViewModel.ReturnUrl ??= Url.Content("~/Home/Index");

            if(!registerViewModel.PrivacyPolicyIsChecked)
                ModelState.AddModelError("Privacy", "Privacy Policy must be reviewed.");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registerViewModel.Email, Email = registerViewModel.Email };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var newUserTable = new UserTable
                    {
                        AspNetUsersId = user.Id,
                        Email = registerViewModel.Email,
                        LastModified = DateTime.Now,
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName,
                    };

                    _repository.AddUser(newUserTable);

                    // Bind program and user
                    _repository.AddUserProgram(new UserProgram()
                    {
                        UserId = newUserTable.UserId,
                        ProgramId = registerViewModel.ProgramId
                    });

                    // Get all programs bound to user
                    var userPrograms = _repository.UserPrograms.Where(p => p.UserId == newUserTable.UserId).ToList();

                    if (userPrograms.Count == 1)
                    {
                        // All facilities are available based on program
                        var availableFacilities = _repository.ProgramFacilities.Where(p => p.ProgramId == registerViewModel.ProgramId).ToList();

                        Facility assignedFacility = null;
                        foreach (var facility in availableFacilities.Select(programFacility => _repository.Facilities.FirstOrDefault(f => f.FacilityId == programFacility.FacilityId))
                            .Where(facility => facility.Name.Contains("SIM")))
                        {
                            assignedFacility = facility;
                        }

                        // Bind user and facility
                        _repository.AddUserFacility(new UserFacility()
                        {
                            UserId = newUserTable.UserId,
                            FacilityId = assignedFacility.FacilityId,
                            LastModified = DateTime.Now,
                        });
                    }

                    return RedirectToAction("AddSecurityQuestions", new { ReturnUrl = registerViewModel.ReturnUrl, Id = newUserTable.UserId });
                
                }
                
                foreach (var error in result.Errors)
                {
                   ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // return the view because something failed
            return View(registerViewModel);
        }
        #endregion  // end of the Register section

        #region Reset Password
        /// <summary>
        ///     User reset their Password - Getter
        /// </summary>
        /// <param name="requiresReset"></param>
        /// <param name="redirectToProfile"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ResetPassword(bool requiresReset = false,  bool redirectToProfile = false)
        {
            if (requiresReset || redirectToProfile)
            {
                var userId = _userManager.GetUserId(User); // This gets the AspNetUsersId
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login"); // Fallback if no user is identified
                }
                return View(new ResetPasswordViewModel { UserId = userId, RedirectToProfile = redirectToProfile });
            }
            else
            {
                return RedirectToAction("Login");   // Redirect back to login if the reset is not required
            }
        }

        /// <summary>
        ///     User reset their Password - setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the view with validation messages
                return View(model);
            }

            // Retrieve the user based on UserId (assuming UserId is stored in the session or passed with the view)
            var user = _repository.UserTables
                .Include(u => u.AspNetUsers)
                .FirstOrDefault(u => u.AspNetUsersId == model.UserId);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(model);
            }

            // Hash and update the new password
            var passwordHasher = new PasswordHasher<AspNetUser>();
            user.AspNetUsers.PasswordHash = passwordHasher.HashPassword(user.AspNetUsers, model.NewPassword);

            // Update the database
            _repository.EditUser(user);

            if(model.RedirectToProfile)
            {
                TempData["SuccessMessage"] = "Your password has been reset successfully.";
                return RedirectToAction("UpdateProfile");
            }
            
            return RedirectToAction("Login");
        }

        /// <summary>
        ///     Forgot Password - Getter
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _repository.UserTables.FirstOrDefaultAsync(ut => ut.Email == email);
            if (user == null)
            {
                TempData["Error"] = "Please enter your Email";
                return RedirectToAction("Login");
            }

            var securityQuestions = await _repository.UserSecurityQuestions
                .Where(q => q.UserId == user.UserId)
                .Select(q => new SecurityQuestionAnswerVm
                {
                    SecurityQuestionId = q.SecurityQuestionId,
                    QuestionText = _repository.SecurityQuestions.FirstOrDefault(sq => sq.SecurityQuestionId == q.SecurityQuestionId).QuestionText
                })
                .ToListAsync();

            var model = new ForgotPasswordViewModel
            {
                Email = user.Email,
                SecurityQuestionsAndAnswers = securityQuestions
            };

            return View(model);
        }


        /// <summary>
        ///     Forgot Password - setter
        ///     Validates the User via their Security Questions & Answers
        ///     If successful, it sets a temporary password.  Then the workflow is the same as if an Admin reset their password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // find the user in the UserTable
            var aspNetUser = await _repository.UserTables.FirstOrDefaultAsync(ut => ut.Email == model.Email);
            if (aspNetUser == null)
            {
                TempData["Error"] = "Email not found.";
                return RedirectToAction("Login");
            }

            var storedQuestions = await _repository.UserSecurityQuestions
                .Where(q => q.UserId == aspNetUser.UserId)
                .ToListAsync();

            bool isValid = storedQuestions.All(stored =>
                model.SecurityQuestionsAndAnswers.Any(a => a.SecurityQuestionId == stored.SecurityQuestionId &&
                VerifyHash(HashText(a.Answer), stored.AnswerHash)));

            if (isValid)
            {
                // find the Identity user
                var user = await _userManager.FindByIdAsync(aspNetUser.AspNetUsersId);
                if (user == null)
                {
                    TempData["Error"] = "User not found in Identity database.";
                    return RedirectToAction("Login");
                }

                // set a temporary password for this User
                var newPassword = "r3s3tP@ssw0rd";
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            
                if (result.Succeeded)
                    {
                        TempData["Message"] = "Password has been reset successfully.  The temporary password is: r3s3tP@ssw0rd";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to reset the password.";
                    }

                return RedirectToAction("Login");  //logic already in place for identifying this password and forcing the User to reset their password
            }

            ModelState.AddModelError(string.Empty, "Security question answers incorrect.");
            TempData["Error"] = "Security question answers incorrect.  Please try again.";
            return RedirectToAction("ForgotPassword", new { email = model.Email });
        }

        #endregion  // end of Reset Password section

        #region Security Questions
        /// <summary>
        ///     Add Security Questions - Getter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddSecurityQuestions(int id, string returnUrl = null)
        {
            var model = new SecurityQuestionsViewModel
            {
                Questions = _repository.SecurityQuestions.Select(q => new SelectListItem
                {
                    Value = q.SecurityQuestionId.ToString(),
                    Text = q.QuestionText
                }).ToList(),
                ReturnUrl = returnUrl,
                CurrentUserId = id
            };

            return View("AddSecurityQuestions", model);
        }

        /// <summary>
        ///     Add Security Questions - Setter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSecurityQuestions(SecurityQuestionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = model.ReturnUrl ?? Url.Content("~/");

                var earlierQuestions = _repository.UserSecurityQuestions
                    .Where(q => q.UserId == model.CurrentUserId).ToList();
                if (earlierQuestions.Count != 0)
                {
                    foreach (var question in earlierQuestions)
                    {
                        _repository.DeleteUserSecurityQuestion(question);
                    }
                }

                var securityQuestions = new List<UserSecurityQuestion>
                {
                    new()
                    {
                        UserId = model.CurrentUserId,
                        SecurityQuestionId = model.SecurityQuestion1,
                        AnswerHash = HashText(model.SecurityQuestion1Answer)
                    },
                    new()
                    {
                        UserId = model.CurrentUserId,
                        SecurityQuestionId = model.SecurityQuestion2,
                        AnswerHash = HashText(model.SecurityQuestion2Answer)
                    },
                    new()
                    {
                        UserId = model.CurrentUserId,
                        SecurityQuestionId = model.SecurityQuestion3,
                        AnswerHash = HashText(model.SecurityQuestion3Answer)
                    }
                };

                foreach (var question in securityQuestions)
                {
                    _repository.AddUserSecurityQuestion(question);
                }

                TempData["SuccessMessage"] = "Your security questions have been reset.";
                return LocalRedirect(returnUrl);
            }

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                
                Console.WriteLine("\n" +modelError.ErrorMessage);
            }
                Console.ResetColor();
                
            model.Questions = _repository.SecurityQuestions.Select(q => new SelectListItem
            {
                Value = q.SecurityQuestionId.ToString(),
                Text = q.QuestionText
            }).ToList();

            return View(model);
        }
        #endregion  // end of the Security Questions region

        private static string HashText(string text)
        {
            using var myHash = SHA256.Create();
            var byteRaw = Encoding.UTF8.GetBytes(text);
            var byteResult = myHash.ComputeHash(byteRaw);

            return string.Concat(Array.ConvertAll(byteResult, h => h.ToString("X2")));
        }

        private bool VerifyHash(string inputHash, string storedHash)
        {
            return string.Equals(inputHash.Trim(), storedHash.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
