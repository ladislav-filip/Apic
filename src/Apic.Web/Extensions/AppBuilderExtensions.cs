using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apic.Web.Cors;
using Microsoft.AspNetCore.Builder;

namespace Apic.Web.Extensions
{
	public static class AppBuilderExtensions
	{
		public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app)
		{
			app.UseSwagger();

			app.UseSwaggerUI(x =>
			{
				x.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");

				x.RoutePrefix = string.Empty;
			});

			return app;
		}

		public static IApplicationBuilder UseCustomizedCors(this IApplicationBuilder app)
		{
			app.UseCors(CorsPolicies.Default);

			return app;
		}
	}
}
