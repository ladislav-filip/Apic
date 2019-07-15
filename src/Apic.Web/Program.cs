using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apic.Data.Context;
using Apic.Web.Extensions;
using BeatPulse;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apic.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IWebHostBuilder builder = WebHost
                .CreateDefaultBuilder(args)
                .UseCustomizedConfigurationFiles()
                .UseCustomizedLogging()
                .UseUrls("https://localhost:44342")
                .UseStartup<Startup>();
            try
            {
                IWebHost host = builder.Build();
                ProcessCommands(args, host);
                await host.RunAsync();
            }
            catch (Exception e)
            {
                IConfiguration fallbackConfig = new ConfigurationBuilder()
                    .AddInMemoryCollection(new[] {new KeyValuePair<string, string>("AICriticalKey", "663d2b21-f9d7-4a2a-a398-8de4f92fdf5a")})
                    .AddEnvironmentVariables()
                    .Build();
                
                TelemetryConfiguration.Active.InstrumentationKey = fallbackConfig["AICriticalKey"];
                var telemetryClient = new TelemetryClient();

                telemetryClient.TrackTrace("Startup Error");
                telemetryClient.TrackException(e);
                telemetryClient.Flush();

                throw;
            }
        }

        private static void ProcessCommands(string[] args, IWebHost host)
        {
            if (!args.Contains("disablemigrations"))
            {
                MigrateDatabase(host);
            }
        }

        private static void MigrateDatabase(IWebHost webhost)
        {
            using (IServiceScope scope = webhost.Services.CreateScope())
            {
                IUnitOfWork dbContext = scope.ServiceProvider.GetService<IUnitOfWork>();
                dbContext.Database.Migrate();
            }
        }
    }
}
