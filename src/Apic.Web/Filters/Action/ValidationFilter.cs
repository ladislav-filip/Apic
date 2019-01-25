using System.Net;
using System.Net.Http;
using Apic.Services;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ValidationProblemDetails;

namespace Apic.Web.Filters.Action
{
    /// <summary>
    /// Zpřístupní ModelState skrze ModelState accessor
    /// Pokud není ModelState validní, ukončuje zpracování
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        private readonly ModelStateAccessor modelStateAccessor;

        public ValidationFilter(ModelStateAccessor modelStateAccessor)
        {
            this.modelStateAccessor = modelStateAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
		{
            CopyModelStateToModelStateAccessor(context);

            if (!context.ModelState.IsValid && !context.ActionDescriptor.ContainsFilter(typeof(IgnoreModelStateOnBinding)))
            {
                var message = ValidationProblemDetails.FromErrors(context.ModelState.ToValidationErrorMessages());
                context.Result = new BadRequestObjectResult(message);
                return;
            }
		}

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // nemá využití
        }

        private void CopyModelStateToModelStateAccessor(ActionExecutingContext context)
        {
            modelStateAccessor.ModelState = context.ModelState;
        }
    }
}
