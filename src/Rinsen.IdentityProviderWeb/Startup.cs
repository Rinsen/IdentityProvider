using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rinsen.IdentityProvider;
using Rinsen.Logger;
using Rinsen.DatabaseInstaller;
using Rinsen.IdentityProvider.Installation;
using Microsoft.AspNetCore.Mvc;
using Rinsen.IdentityProviderWeb.Installation;
using Rinsen.IdentityProviderWeb.IdentityExtensions;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Installation.ReferenceIdentityInstallation;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Rinsen.IdentityProviderWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRinsenIdentity(options => options.ConnectionString = Configuration["Data:DefaultConnection:ConnectionString"]);

            //services.AddLogger(options =>
            //{
            //    options.EnvironmentName = env.EnvironmentName;
            //    options.MinLevel = LogLevel.Trace;
            //    options.ApplicationLogKey = Configuration["Logging:LogApplicationKey"];
            //    options.LogServiceUri = Configuration["Logging:Uri"];
            //});

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.SessionStore = new SqlTicketStore(new SessionStorage(Configuration["Data:DefaultConnection:ConnectionString"]));
                    options.LoginPath = "/Identity/Login";
                });

            if (Env.IsDevelopment())
            {
                services.AddDatabaseInstaller(Configuration["Data:DefaultConnection:ConnectionString"]);
            }

            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminsOnly", policy => policy.RequireClaim("http://rinsen.se/Administrator"));
            });

            services.Remove(new ServiceDescriptor(typeof(ILoginService), typeof(LoginService), ServiceLifetime.Transient));
            services.AddTransient<ILoginService, IdentityWebLoginService>();
            services.AddTransient<AdministratorStorage, AdministratorStorage>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app/*, IHostingEnvironment env, ILoggerFactory loggerFactory, IRinsenLoggerInitializer logInitializer*/)
        {
            //logInitializer.Run(new FilterLoggerSettings {
            //    { "Microsoft", LogLevel.Warning },
            //    { "Rinsen", LogLevel.Information }
            //});

            app.UseAuthentication();

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseStatusCodePagesWithRedirects("~/errors/Code{0}");

            //app.UseDeveloperExceptionPage();

            //app.UseLogMiddleware();

            if (Env.IsDevelopment())
            {
                app.RunDatabaseInstaller(new DatabaseVersion[] { new FirstVersion(), new IdentityProviderWebFirstVersion(), new ReferenceIdentityFirstVersion() });
            }
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}");
            });
        }
    }
}
