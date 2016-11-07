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

namespace Rinsen.IdentityProviderWeb
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("aspnet5-Rinsen.Web-20150804040342");
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRinsenIdentity(options => options.ConnectionString = Configuration["Data:DefaultConnection:ConnectionString"]);

            services.AddLogger(options =>
            {
                options.ConnectionString = Configuration["Data:DefaultConnection:ConnectionString"];
                options.EnvironmentName = _env.EnvironmentName;
                options.MinLevel = LogLevel.Trace;
            });

            services.AddDatabaseInstaller(Configuration["Data:DefaultConnection:ConnectionString"]);

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IRinsenLoggerInitializer logInitializer)
        {
            logInitializer.Run(new FilterLoggerSettings {
                { "Microsoft", LogLevel.Warning },
                { "Rinsen", LogLevel.Information }
            });

            app.UseStatusCodePagesWithRedirects("~/errors/Code{0}");

            app.UseDeveloperExceptionPage();

            app.UseLogMiddleware();

            if (env.IsDevelopment())
            {
                app.RunDatabaseInstaller(new DatabaseVersion[] { new FirstVersion(), new IdentityProviderWebFirstVersion() });
            }
            
            app.UseStaticFiles();

            app.UseCookieAuthentication(new RinsenDefaultCookieAuthenticationOptions(Configuration["Data:DefaultConnection:ConnectionString"]));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "areaRoute",
                //    template: "{area:exists}/{controller}/{action}");
            });
        }
    }
}
