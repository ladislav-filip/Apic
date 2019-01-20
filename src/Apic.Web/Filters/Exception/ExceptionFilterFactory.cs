using System;
using Apic.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apic.Web.Filters.Exception
{
    public class ExceptionFilterFactory : IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var modelStateAccessor = (ModelStateAccessor)serviceProvider.GetService(typeof(ModelStateAccessor));
            var hostingEnvironment = (IHostingEnvironment)serviceProvider.GetService(typeof(IHostingEnvironment));
            return new ExceptionFilter(modelStateAccessor, hostingEnvironment);
        }
    }
}
