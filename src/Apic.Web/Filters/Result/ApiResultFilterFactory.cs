using System;
using Apic.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apic.Web.Filters.Result
{
    public class ApiResultFilterFactory : IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var modelStateAccessor = (ModelStateAccessor)serviceProvider.GetService(typeof(ModelStateAccessor));
            return new ApiResultFilter(modelStateAccessor);
        }
    }
}
