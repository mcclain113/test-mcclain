using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(IS_Proj_HIT.Areas.Identity.IdentityHostingStartup))]
namespace IS_Proj_HIT.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}