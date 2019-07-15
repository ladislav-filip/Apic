using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Apic.Web.Extensions
{
	public static class WebHostBuilderExtensions
	{
		public static IWebHostBuilder UseCustomizedConfigurationFiles(this IWebHostBuilder webHost)
		{
			
			webHost.ConfigureAppConfiguration((host, config) =>
			{
				config.Sources.Clear();
				
				config.AddJsonFile("appsettings.json", true);
				config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true);
				
				config.AddJsonFile("beatpulse.json", true);
				
				config.AddIniFile("appsettings.ini", true);
				config.AddIniFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.ini", true);
			});

			return webHost;
		}

		public static IWebHostBuilder UseCustomizedLogging(this IWebHostBuilder webHost)
		{
			return webHost.ConfigureLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
				builder.AddEventSourceLogger();
				builder.AddApplicationInsights();
			});
		}

	}
}
