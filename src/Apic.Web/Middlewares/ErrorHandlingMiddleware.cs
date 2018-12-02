using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ProblemDetails;

namespace Apic.Web.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IHostingEnvironment host;
		private readonly IActionResultExecutor<ObjectResult> executor;
		private readonly ILogger logger;

		private static readonly HashSet<string> CorsHeaderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			HeaderNames.AccessControlAllowCredentials,
			HeaderNames.AccessControlAllowHeaders,
			HeaderNames.AccessControlAllowMethods,
			HeaderNames.AccessControlAllowOrigin,
			HeaderNames.AccessControlExposeHeaders,
			HeaderNames.AccessControlMaxAge,
		};

		public ErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment host, IActionResultExecutor<ObjectResult> executor, ILogger<ErrorHandlingMiddleware> logger)
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

				if (context.Response.HasStarted)
				{
					return;
				}

				object problemDetails = GetProblemResult(context);

				if (problemDetails != null)
				{
					ClearResponse(context, context.Response.StatusCode);
					await WriteProblemDetails(context, problemDetails);
				}
			}
			catch (Exception ex)
			{
				logger.LogError("Request execution failed", ex);

				ClearResponse(context, StatusCodes.Status500InternalServerError);
				var errorDetails = GetExceptionDetails(context, ex);

				await WriteProblemDetails(context, errorDetails);
			}
		}

		private object GetProblemResult(HttpContext context)
		{
			if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
			{
				return ProblemDetails.FromMessage(HttpStatusCode.NotFound);
			}

			if (context.Response.StatusCode == (int)HttpStatusCode.UnsupportedMediaType)
			{
				return ProblemDetails.FromMessage(HttpStatusCode.UnsupportedMediaType);
			}

			return null;
		}

		/// <summary>
		/// Přenastavení hlaviček a stavového kódu
		/// </summary>
		private static void ClearResponse(HttpContext context, int statusCode)
		{
			HeaderDictionary headers = new HeaderDictionary();

			headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
			headers.Append(HeaderNames.Pragma, "no-cache");
			headers.Append(HeaderNames.Expires, "0");

			foreach (var header in context.Response.Headers)
			{
				if (CorsHeaderNames.Contains(header.Key))
				{
					headers.Add(header);
				}
			}

			context.Response.Clear();
			context.Response.StatusCode = statusCode;

			foreach (var header in headers)
			{
				context.Response.Headers.Add(header);
			}
		}

		private ProblemDetails GetExceptionDetails(HttpContext context, Exception exception)
		{
			return ProblemDetails.FromException(exception, host.IsDevelopment());
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

			result.ContentTypes.Add("application/problem+json");
			result.ContentTypes.Add("application/problem+xml");
			result.ContentTypes.Add("application/json");
			result.ContentTypes.Add("application/xml");

			return executor.ExecuteAsync(actionContext, result);
		}
	}
	

	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomizedExceptionHandling(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ErrorHandlingMiddleware>();
		}
	}
}
