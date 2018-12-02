using System.Threading.Tasks;
using Apic.Web.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Apic.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			await WebHost
				.CreateDefaultBuilder(args)
				.CustomizeConfigurationFiles()
				.CustomizeLogging()
				.UseApplicationInsights()
				.UseStartup<Startup>()
				.Build()
				.MigrateDatabase()
				.RunAsync();
		}
	}
}
