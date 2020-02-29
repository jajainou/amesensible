using amesensible.Core.Interfaces;
using amesensible.Data;
using amesensible.Data.Repositories;
using amesensible.Identity.Db;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace amesensible
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
            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/amesensible/browser";
            });

            services.AddDbContext<AmeSensibleContext>();
            services.AddDbContext<IdentityDbContext>();

            //services.Configure<PaypalConfiguration>(Configuration.GetSection("Paypal"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityDbContext>();
            services.AddIdentityServer(configuration => configuration.UserInteraction.LoginUrl = "/business/login")
                .AddApiAuthorization<ApplicationUser, IdentityDbContext>();
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.Scan(scan => scan.FromAssembliesOf(typeof(AmeSensibleContext), typeof(Core.Interfaces.IRepository<>))
                                      .AddClasses(true)
                                      .AsImplementedInterfaces());

            services.AddSwaggerDocument();

            services.AddMediatR(typeof(Core.Interfaces.IRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }

        private X509Certificate2 loadCertificateFromFile()
        {
            // NOTE:
            // You should check out the identity of your application pool and make sure
            // that the `Load user profile` option is turned on, otherwise the crypto susbsystem won't work.
            var certificate = new X509Certificate2(
                fileName: Configuration["X509Certificate:FileName"],
                password: Configuration["X509Certificate:Password"],
                keyStorageFlags: X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet |
                                 X509KeyStorageFlags.Exportable);
            return certificate;
        }
    }
}
