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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Apic.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuilder = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webhostBuilder =>
                {
                    webhostBuilder
                        .UseStartup<Startup>()
                        .UseUrls("https://localhost:44342")
                        
                        .ConfigureAppConfiguration((host, config) =>
                        {
                            config.Sources.Clear();

                            config.AddJsonFile("appsettings.json", true);
                            config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true);

                            config.AddJsonFile("beatpulse.json", true);

                            config.AddIniFile("appsettings.ini", true);
                            config.AddIniFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.ini", true);
                        })
                        
                        .ConfigureLogging(builder =>
                        {
                            builder.AddConsole();
                            builder.AddDebug();
                            builder.AddEventSourceLogger();
                            builder.AddApplicationInsights();
                        });
                });
            try
            {
                IHost host = hostBuilder.Build();

                if (!args.Contains("skipmigrations"))
                {
                    MigrateDatabase(host);
                }

                await host.RunAsync();
            }
            catch (Exception e)
            {
                IConfiguration fallbackConfig = new ConfigurationBuilder()
                    .AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("AICriticalKey",
                            "663d2b21-f9d7-4a2a-a398-8de4f92fdf5a")
                    })
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

        private static void MigrateDatabase(IHost webhost)
        {
            using (IServiceScope scope = webhost.Services.CreateScope())
            {
                IUnitOfWork dbContext = scope.ServiceProvider.GetService<IUnitOfWork>();
                dbContext.Database.Migrate();
            }
        }
    }
}