using System.Net;
using Apic.Contracts.Infrastructure.Transfer.StatusResults;
using Apic.Services;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ProblemDetails;
using ValidationProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ValidationProblemDetails;

namespace Apic.Web.Filters.Exception
{
    /// <summary>
    /// V případě výjimky v rámci MVC request pipeline se pokusí nastavit vhodný status. Výchozí je klasická výjimka (500)
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private ModelStateAccessor modelStateAccessor;
        private IHostingEnvironment host;

        public ExceptionFilter(ModelStateAccessor modelStateAccessor, IHostingEnvironment host)
        {
            this.modelStateAccessor = modelStateAccessor;
            this.host = host;
        }

        public void OnException(ExceptionContext context)
        {
            context.ModelState.AddModelError("", context.Exception.Message);
            ValidationProblemDetails message = ValidationProblemDetailsHelper.FromErrors(context.ModelState.ToValidationErrorMessages());

            switch (context.Exception.GetType().ToString())
            {
                case "Apic.Common.Exceptions.RequestFailedException":
                    message.Title = "Request cannot be processed";
                    message.Status = StatusCodes.Status400BadRequest;
                    context.Result = new BadRequestObjectResult(message);
                    return;
                case "Apic.Common.Exceptions.ObjectNotFoundException":
                    message.Status = StatusCodes.Status404NotFound;
                    message.Title = "Requested resource has not been found";
                    context.Result = new NotFoundObjectResult(message);
                    return;
                default:
                    context.Result = new ObjectResult(ProblemDetails.FromException(context.Exception, host.IsDevelopment()))
                    {
                        StatusCode = 500,
                        DeclaredType = typeof(ProblemDetails)
                    };
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}
