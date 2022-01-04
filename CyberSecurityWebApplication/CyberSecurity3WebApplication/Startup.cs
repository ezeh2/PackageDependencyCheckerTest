using CyberSecurity3WebApplication.DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberSecurity3WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // extension method "AddFile" comes from nuget-package "Serilog.Extensions.Logging.File"
            loggerFactory.AddFile(@"c:\temp\CyberSecurity3WebApplication.log");

            // don't allow inline javascript
            // mitigate XSS
            // https://content-security-policy.com/
            app.Use(async (ctx, next) =>
            {
                // "Content-Security-Policy-Report-Only" instructs the browser to only send reports (does not block anything).
                // change later from "Content-Security-Policy-Report-Only" to "Content-Security-Policy"

                // This is the Starter Policy according to https://content-security-policy.com/
                // This policy allows images, scripts, AJAX, form actions, and CSS from the same origin, and does not allow any other resources to load (eg object, frame, media, etc). 
                ctx.Response.Headers.Add("Content-Security-Policy-Report-Only",
                                         "default-src 'none'; script-src 'self'; connect-src 'self'; img-src 'self'; style-src 'self';base-uri 'self';form-action 'self'; report-uri /api/cspreport");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
