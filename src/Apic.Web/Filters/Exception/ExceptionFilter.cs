using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Apic.Common.Exceptions;
using Apic.Contracts.Infrastructure.Transfer.StatusResults;
using Apic.Services;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using ProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ProblemDetails;
using ValidationProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ValidationProblemDetails;

namespace Apic.Web.Filters.Exception
{
    /// <summary>
    /// V případě výjimky v rámci MVC request pipeline se pokusí nastavit vhodný status. Výchozí je klasická výjimka (500)
    /// Primárně je to dobré místo pro pohodlnou práci s custom výjimkami v rámci MVC invocation pipeline
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private ModelStateAccessor modelStateAccessor;
        private IWebHostEnvironment host;
        private List<IApiException> apiExceptions;

        public ExceptionFilter(ModelStateAccessor modelStateAccessor, IWebHostEnvironment host, IEnumerable<IApiException> apiExceptions)
        {
            this.modelStateAccessor = modelStateAccessor;
            this.host = host;
            this.apiExceptions = apiExceptions.ToList();
        }

        public void OnException(ExceptionContext context)
        {
            context.ModelState.AddModelError("", context.Exception.Message);
            ValidationProblemDetails message = ValidationProblemDetailsHelper.FromErrors(context.ModelState.ToValidationErrorMessages());

            IApiException apiException = apiExceptions.FirstOrDefault(x =>
            {
                var fullName = x.GetType().FullName;
                return fullName != null && fullName.Equals(context.Exception.GetType().ToString());
            });

            if (apiException != null)
            {
                System.Exception ex = (System.Exception) apiException;
                
                message.Title = apiException.Title;
                message.Status = apiException.StatusCode;
                message.Detail = host.IsDevelopment() ? ex.StackTrace : string.Empty;
                
                context.Result = new ObjectResult(message)
                {
                    StatusCode = apiException.StatusCode,
                    DeclaredType = typeof(ProblemDetails)
                };
            }
            else
            {
                context.Result = new ObjectResult(ProblemDetails.FromException(context.Exception, host.IsDevelopment()))
                {
                    StatusCode = 500,
                    DeclaredType = typeof(ProblemDetails)
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
