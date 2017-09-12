using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Rinsen.IdentityProvider.Token;
using Rinsen.IdentityProvider.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Rinsen.IdentityProvider.Contracts;

namespace IdentityClientWeb
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddToken(TokenDefaults.AuthenticationScheme, options =>
                {
                    
                    options.CallbackPath = new PathString("/token");
                    options.ClaimsIssuer = RinsenIdentityConstants.RinsenIdentityProvider;
                    options.ConnectionString = Configuration["Data:DefaultConnection:ConnectionString"];
                    options.ApplicationKey = Configuration["IdentityProvider:ApplicationKey"];
                    options.LoginPath = Configuration["IdentityProvider:LoginPath"];
                    options.ValidateTokenPath = Configuration["IdentityProvider:ValidateTokenPath"];
                })
                .AddCookie(options =>
                {
                    options.SessionStore = new SqlTicketStore(new SessionStorage(Configuration["Data:DefaultConnection:ConnectionString"]));
                });

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            
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
