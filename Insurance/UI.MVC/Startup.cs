using System.Collections.Generic;
using System.Globalization;
using Distances;
using Insurance.BL;
using Insurance.DAL;
using Insurance.DAL.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace UI.MVC {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            // Localization
            services.AddLocalization (opt => { opt.ResourcesPath = "Resources"; });
            services.AddControllersWithViews ()
                .AddViewLocalization (LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization ();

            services.Configure<RequestLocalizationOptions> (
                opt => {
                    List<CultureInfo> supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo ("en-US"),
                        new CultureInfo ("fr-FR"),
                        new CultureInfo ("nl-BE")
                    };
                    opt.DefaultRequestCulture = new RequestCulture ("en-US");
                    opt.SupportedCultures = supportedCultures;
                    opt.SupportedUICultures = supportedCultures;
                });
            
            //caching
            services.AddResponseCaching();
            services.AddMvc();


            services.AddScoped<IDistanceLocalizer, DistanceLocalizer> ();
            services.AddScoped<IDoubleParser, DoubleParser> ();

            // Old dependency injection
            services.AddDbContext<InsuranceDbContext> ();
            services.AddScoped<IRepository, Repository> ();
            services.AddScoped<IManager, Manager> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            
            //Caching
            app.UseResponseCaching();

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            // app.UseAuthorization ();

            app.UseRequestLocalization (app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>> ().Value);

            /*app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });*/

            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}