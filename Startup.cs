using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreHealthCheckSample.Extensions;

namespace NetCoreHealthCheckSample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            services.AddControllers();
            services.AddHealthChecks(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthorization();
            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = registration => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(delegate (Options options)
                {
                    options.UIPath = "/hc-ui";
                }
            );

            #region our custom json response writer,instead of HealthCheckUI response writer
            //app.UseHealthChecks("/hc", new HealthCheckOptions
            //{
            //    ResponseWriter = async (c, r) =>
            //    {
            //        c.Response.ContentType = "application/json";

            //        var result = JsonConvert.SerializeObject(new
            //        {
            //            status = r.Status.ToString(),
            //            components = r.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() })
            //        });
            //        await c.Response.WriteAsync(result);
            //    }
            //}); 
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecksUI();
            });
        }
    }
}
