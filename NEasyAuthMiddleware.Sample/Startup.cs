using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEasyAuthMiddleware.Core;

namespace NEasyAuthMiddleware.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get;  }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddEasyAuth(options =>
            {
                options.ClaimTypesWithCommaSeparatedValues.Add("some-claim-type");
                options.IgnoreClaimTypes.Add("ignored-claim-type");
            });

            if (HostingEnvironment.IsDevelopment()) // Use the mock json file when not running in an app service
            {
                var mockFile = $"{HostingEnvironment.ContentRootPath}\\mock_user.json";
                services.UseJsonFileToMockEasyAuth(mockFile);
            }

            services.AddSingleton<IHeaderDictionaryTransformer, CustomHeaderDictionaryTransformer>(); // register custom header dictionary transformer
            services.AddSingleton<IClaimMapper, CustomClaimMapper>(); // register custom header mapper
            services.AddSingleton<IClaimsTransformer, CustomClaimsTransformer>(); // register custom claims transformer
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
