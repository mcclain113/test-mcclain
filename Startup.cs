using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IS_Proj_HIT.Data;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        static readonly string _RequireAuthenticatedUserPolicy = "RequireAuthenticatedUserPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Custom paths for MVC
                options.LoginPath = "/Account/Login"; // The MVC AccountController Login action
                options.LogoutPath = "/Account/Logout"; // The MVC AccountController Logout action
                options.AccessDeniedPath = "/Account/AccessDenied"; // The MVC AccountController AccessDenied action

                // Persistent login configuration
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Persistent cookie duration
                options.SlidingExpiration = true; // Extends expiration time with activity

                options.Cookie.HttpOnly = true; // Secure cookie (prevents access via JavaScript)
                options.Cookie.IsEssential = true; // Required for GDPR compliance

                options.Events.OnValidatePrincipal = context =>
                {
                    // Log validation behavior for debugging
                    context.HttpContext.Response.Headers.Append("Cookie-Debug", "Validation Happened");
                    return Task.CompletedTask;
                };
            });

            // this registration is paired with the services.AddIdentity<IdentityUser, IdentityRole>()
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:HIT:ConnectionString"]));

            // was .AddDefaultIdentity... which forces use of Razor Pages;  this change allows the MVC Razor file use
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Customize Identity options go here if needed
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // this registration is for the database which handles the application data
            services.AddDbContext<WCTCHealthSystemContext>(options => options.UseSqlServer(Configuration["Data:HIT:ConnectionString"]));
            services.AddTransient<IWCTCHealthSystemRepository, EFWCTCHealthSystemRepository>();
            services.AddScoped<PermissionService>();
            services.AddHttpContextAccessor();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout - set this also in the BaseController.cs OnActionExecuting method
                options.Cookie.HttpOnly = true; // Ensure the cookie is accessible only via HTTP
                options.Cookie.IsEssential = true; // Mark the cookie as essential for GDPR compliance
            });

            // AddControllersWithViews for full MVC support
            services.AddControllersWithViews()
                .AddSessionStateTempDataProvider(); // This ensures TempData works across redirects

            // Retain AddMvc for backward compatibility or Razor runtime compilation (needed for Razor Pages)
            var builder = services.AddMvc();
            if (Env.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }

            services.AddScoped<IDisclosureReportService, DisclosureReportService>();
            services.AddScoped<IEncounterService, EncounterService>();
            services.AddScoped<IAllergenService, AllergenService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IMedicationService, MedicationService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBirthRegistrySearchService, BirthRegistrySearchService>();
            services.AddScoped<IBirthRegistryDropdownDataService, BirthRegistryDropdownDataService>();
            services.AddScoped<IBirthRegistryValidationService, BirthRegistryValidationService>();

            services.AddAuthorizationBuilder()
                .AddPolicy(_RequireAuthenticatedUserPolicy, builder => builder.RequireAuthenticatedUser());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Load Static Files early so they short-circuit the pipeline for those requests and avoid unnecessary middleware work
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Routing before CORS as routing must be established before middleware that depends on route info, like CORS
            app.UseRouting();

            // CORS middleware should run after UseRouting and before authentication/authorization so preflight and CORS checks happen early for routed endpoints.
            app.UseCors();

            // Session needs to be available to handlers and any auth code that reads/writes session state; it must be enabled before endpoint execution (before UseEndpoints / MapControllerRoute) but after static files to avoid unnecessary session work for static assets.
            // Session runs before UseAuthentication so session is available when auth runs, since the authentication logic depends on session state (app stores auth info in session).
            app.UseSession();

            // Authentication must run before Authorization so the request has a user principal to check; both should run after routing so they apply to the correct endpoint metadata.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Map explicit route for login for all users (authenticated or not)
                // This allows the Login getter method to apply logic depending upon authentication state of the User
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}"
                ).AllowAnonymous(); // Allow unauthenticated access to the Login action

                // Apply global authorization policy to all other routes
                endpoints.MapControllerRoute(
                    name: "authenticatedDefault",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                ).RequireAuthorization(_RequireAuthenticatedUserPolicy);

            });

        }
    }
}