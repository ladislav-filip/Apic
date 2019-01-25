using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Apic.Web.Middlewares
{
    public class OptionsMethodMiddleware
    {
        private readonly RequestDelegate next;

        public OptionsMethodMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await next(httpContext);

            if (httpContext.Request.Method.ToLowerInvariant() == HttpMethods.Options.ToLowerInvariant() && httpContext.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
            {
                httpContext.Response.Headers[HttpResponseHeader.Allow.ToString()] += ", OPTIONS";
                httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
            }
        }
    }

    public static class OptionsMethodMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomizedOptionsMethodMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsMethodMiddleware>();
        }
    }
}
