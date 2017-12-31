using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rinsen.IdentityProvider;
using Rinsen.DatabaseInstaller;
using Rinsen.IdentityProvider.Installation;
using Microsoft.AspNetCore.Mvc;
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
            services.AddRinsenIdentity(options => options.ConnectionString = Configuration["Rinsen:ConnectionString"]);
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.SessionStore = new SqlTicketStore(new SessionStorage(Configuration["Rinsen:ConnectionString"]));
                    options.LoginPath = "/Identity/Login";
                });

            if (Env.IsDevelopment())
            {
                services.AddDatabaseInstaller(Configuration["Rinsen:ConnectionString"]);
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseStatusCodePagesWithRedirects("~/errors/Code{0}");
                app.UseExceptionHandler("/Home/Error");
            }

            if (Env.IsDevelopment())
            {
                app.RunDatabaseInstaller(new DatabaseVersion[] { new FirstVersion(), new ReferenceIdentityFirstVersion() });
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
