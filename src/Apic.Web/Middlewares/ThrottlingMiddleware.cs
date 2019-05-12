using System;
using System.Net;
using System.Threading.Tasks;
using Apic.Common.Options;
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
                TimeSpan period = TimeSpan.FromMinutes(1);

                throttlingDemoService.LogAccess(ipAddress);

                if (throttlingDemoService.ThresholdExceeded(ipAddress, throttlingSettings.MaxRequestsPerMinute, period))
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    await httpContext.Response.WriteAsync("IP Address Limit Exceeded");
                    return;
                }

                int remainingCount = throttlingDemoService.RemainingLimit(ipAddress, throttlingSettings.MaxRequestsPerMinute, period);
                httpContext.Response.Headers.Add("X-Rate-Limit-Remaining", remainingCount.ToString());
                httpContext.Response.Headers.Add("X-Rate-Limit-Period", period.ToString());
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
