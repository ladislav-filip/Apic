using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Apic.Web.Extensions
{
	public static class WebHostBuilderExtensions
	{
		public static IWebHostBuilder CustomizeConfigurationFiles(this IWebHostBuilder webHost)
		{
			webHost.ConfigureAppConfiguration((host, config) => config.AddIniFile("appsettings.ini", false));
			webHost.ConfigureAppConfiguration((host, config) => config.AddIniFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.ini", true));

			return webHost;
		}

		public static IWebHostBuilder CustomizeLogging(this IWebHostBuilder webHost)
		{
			return webHost;
		}

		public static IWebHost CustomizeDatabaseMigration(this IWebHost webHost)
		{
			//using (IServiceScope scope = webHost.Services.CreateScope())
			//{
			//	ApicDbContext dbContext = scope.ServiceProvider.GetService<ApicDbContext>();
			//	dbContext.Database.Migrate();
			//}

			return webHost;
		}
	}
}
