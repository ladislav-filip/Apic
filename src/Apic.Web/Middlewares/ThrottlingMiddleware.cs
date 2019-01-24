using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Apic.Common.Configuration;
using Apic.Services.Throttling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Apic.Web.Middlewares
{
    public class ThrottlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Throttling throttlingSettings;
        private readonly ThrottlingDemoService throttlingDemoService;

        public ThrottlingMiddleware(RequestDelegate next, IOptions<Throttling> throttlingSettings, ThrottlingDemoService throttlingDemoService)
        {
            this.next = next;
            this.throttlingSettings = throttlingSettings.Value;
            this.throttlingDemoService = throttlingDemoService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (IsApiRequest(httpContext.Request))
            {
                IPAddress ipAddress = httpContext.Connection.RemoteIpAddress;

                throttlingDemoService.LogAccess(ipAddress);

                if (throttlingDemoService.ThresholdExceeded(ipAddress, throttlingSettings.MaxRequestsPerMinute,
                    TimeSpan.FromMinutes(1)))
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    await httpContext.Response.WriteAsync("IP address requests limit exceeded");
                    return;
                }
            }

            await next(httpContext);
        }

        private bool IsApiRequest(HttpRequest request)
        {
            return request.Path.Value.Contains("/api/");
        }
    }

    public static class ThrottlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseThrottlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThrottlingMiddleware>();
        }
    }
}
