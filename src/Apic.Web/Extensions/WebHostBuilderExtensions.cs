using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
				
				config.AddJsonFile("beatpulsesettings.json", true);
				
				config.AddIniFile("appsettings.ini", true);
				config.AddIniFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.ini", true);
			});

			return webHost;
		}

		public static IWebHostBuilder UseCustomizedLogging(this IWebHostBuilder webHost)
		{
			return webHost.ConfigureLogging(builder =>
			{
				
			});
		}

		public static IWebHostBuilder UseCustomizedBeatPulse(this IWebHostBuilder webHost)
		{
			webHost = webHost.UseBeatPulse(options =>
			{
				options.ConfigurePath("hc")
					.ConfigureTimeout(1500)
					.ConfigureDetailedOutput(true, true);
			});

			return webHost;
		}
        
	}
}
