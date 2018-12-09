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
				.CustomizeHealtchCheck()
				.CustomizeLogging()
				.UseApplicationInsights()
				.UseStartup<Startup>()
				.Build()
				.CustomizeDatabaseMigration()
				.RunAsync();
		}
	}
}
