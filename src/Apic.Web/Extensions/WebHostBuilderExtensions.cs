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

		public static IWebHostBuilder CustomizeHealtchCheck(this IWebHostBuilder webHost)
		{
			webHost = webHost.UseBeatPulse(options =>
			{
				options.ConfigurePath("health")
					.ConfigureTimeout(1500)
					.ConfigureDetailedOutput(true, true);
			});

			return webHost;
		}
        
	}
}
