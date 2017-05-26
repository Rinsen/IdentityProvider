using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Rinsen.IdentityProvider.Token;
using Rinsen.IdentityProvider.Core;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClientWeb
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRinsenAuthentication();

            services.AddAuthorization(options =>
            {
                
                //options.AddPolicy("AlwaysFail", policy => policy.Requirements.Add(new AlwaysFailRequirement()));

            });

            // Add framework services.
            services.AddMvc(config =>
            {
                config.Filters.Add(new RequireHttpsAttribute());

                //var policy = new AuthorizationPolicyBuilder()
                //                 .RequireAuthenticatedUser()
                //                 .Build();

                //config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTokenAuthenticationWithCookieAuthentication(new TokenOptions(Configuration["Data:DefaultConnection:ConnectionString"])
            {
                ApplicationKey = Configuration["IdentityProvider:ApplicationKey"],
                LoginPath = Configuration["IdentityProvider:LoginPath"],
                ValidateTokenPath = Configuration["IdentityProvider:ValidateTokenPath"]
            },
                new RinsenDefaultCookieAuthenticationOptions(Configuration["Data:DefaultConnection:ConnectionString"])
            );

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
