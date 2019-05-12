using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apic.Web.Cors;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

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
				x.InjectStylesheet("css/swagger.css");
				x.DisplayOperationId();
				x.EnableFilter();
				x.DocExpansion(DocExpansion.List);

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
