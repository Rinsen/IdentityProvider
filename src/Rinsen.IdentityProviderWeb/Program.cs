using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;
using Rinsen.Logger;

namespace Rinsen.IdentityProviderWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("Rinsen", LogLevel.Information)
                        .AddRinsenLogger(hostingContext.Configuration, hostingContext.HostingEnvironment.EnvironmentName);
                })
                .UseStartup<Startup>()
                .Build();

            webHost.Run();
        }
    }
}
