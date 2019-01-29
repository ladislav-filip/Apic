using System;
using System.Linq;
using System.Threading.Tasks;
using Apic.Data.Context;
using Apic.Web.Extensions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Apic.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                IWebHost host = WebHost
                    .CreateDefaultBuilder(args)
                    .CustomizeConfigurationFiles()
                    .CustomizeHealtchCheck()
                    .CustomizeLogging()
                    .UseApplicationInsights()
                    .UseStartup<Startup>()
                    .Build();

                ProcessCommands(args, host);

                await host.RunAsync();
            }
            catch (Exception e)
            {
                TelemetryClient tc = new TelemetryClient {InstrumentationKey = ""};

                tc.TrackTrace("Startup Error");
                tc.TrackException(e);
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
                ApicDbContext dbContext = scope.ServiceProvider.GetService<ApicDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
