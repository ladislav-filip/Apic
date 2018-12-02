using System.Data.SqlClient;
using System.Threading.Tasks;
using Apic.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Apic.Web.Middlewares
{
	/// <summary>
	/// Plánuje se pro ASP.NET Core 2.2
	/// https://blogs.msdn.microsoft.com/webdev/2018/08/22/asp-net-core-2-2-0-preview1-healthcheck
	/// </summary>
	public class HealthMonitorMiddleware
	{
		private readonly RequestDelegate next;
		private readonly string path;
		private readonly ApicDbContext apicDbContext;

		public HealthMonitorMiddleware(RequestDelegate next, ApicDbContext apicDbContext, string path)
		{
			this.next = next;
			this.apicDbContext = apicDbContext;
			this.path = path;
		}

		public async Task Invoke(HttpContext context)
		{
			if (context.Request.Path.Value == path)
			{
				try
				{
					await apicDbContext.Database.OpenConnectionAsync();

					context.Response.StatusCode = 200;
					await context.Response.WriteAsync("OK");
				}
				catch (SqlException ex)
				{
					context.Response.StatusCode = 400;
					await context.Response.WriteAsync("ERROR " + ex.Message);
				}
			}
			else
			{
				await this.next(context);
			}
		}
	}

	public static class HealthMonitorMiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomizedHealthMonitor(this IApplicationBuilder builder, string path)
		{
			return builder.UseMiddleware<HealthMonitorMiddleware>(path);
		}
	}
}
