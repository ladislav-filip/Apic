using System.Linq;
using System.Threading.Tasks;
using Apic.Data.Context;
using Apic.Web.Extensions;
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

        private static void ProcessCommands(string[] args, IWebHost host)
        {
            if (args.Contains("migratedb"))
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
