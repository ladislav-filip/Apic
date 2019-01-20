using Apic.Contracts.Infrastructure.Transfer.StatusResults;
using Apic.Services;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ProblemDetails;

namespace Apic.Web.Filters.Exception
{
    /// <summary>
    /// V případě výjimky v rámci MVC request pipeline se pokusí nastavit vhodný status. Výchozí je klasická výjimka (500)
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private ModelStateAccessor modelStateAccessor; // TODO: zbytečné
        private IHostingEnvironment host;

        public ExceptionFilter(ModelStateAccessor modelStateAccessor, IHostingEnvironment host)
        {
            this.modelStateAccessor = modelStateAccessor;
            this.host = host;
        }

        public void OnException(ExceptionContext context)
        {
            context.ModelState.AddModelError("", context.Exception.Message);
            var message = ValidationProblemDetailsHelper.FromErrors(context.ModelState.ToValidationErrorMessages());
            
            context.ExceptionHandled = true;

            switch (context.Exception.GetType().ToString())
            {
                case "Apic.Common.Exceptions.RequestFailedException":
                    context.Result = new BadRequestObjectResult(message);
                    return;
                case "Apic.Common.Exceptions.ObjectNotFoundException":
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
        }
    }
}
