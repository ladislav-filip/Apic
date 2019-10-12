using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ProblemDetails;

namespace Apic.Web.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IWebHostEnvironment host;
		private readonly IActionResultExecutor<ObjectResult> executor;
		private readonly ILogger logger;

		public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment host, IActionResultExecutor<ObjectResult> executor, ILogger<ErrorHandlingMiddleware> logger)
		{
			this.next = next;
			this.host = host;
			this.executor = executor;
			this.logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				logger.LogError("Request execution failed", ex);

				context.Response.Clear();
				
				ProblemDetails problemDetails = null;
				if (ex is OperationCanceledException)
				{
					context.Response.StatusCode = StatusCodes.Status400BadRequest;
					problemDetails = ProblemDetails.FromMessage(HttpStatusCode.BadRequest, "Request was cancelled");
				}
				else
				{
					context.Response.StatusCode = StatusCodes.Status500InternalServerError;
					problemDetails = ProblemDetails.FromException(ex, host.IsDevelopment());
				}

				await WriteProblemDetails(context, problemDetails);
			}
		}

		private Task WriteProblemDetails(HttpContext context, object details)
		{
			var routeData = context.GetRouteData() ?? new RouteData();

			var actionContext = new ActionContext(context, routeData, new ActionDescriptor());

			var result = new ObjectResult(details)
			{
				StatusCode = context.Response.StatusCode,
				DeclaredType = details.GetType(),
			};

			return executor.ExecuteAsync(actionContext, result);
		}
	}
}
