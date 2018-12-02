using Apic.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ValidationProblemDetails;

namespace Apic.Web.ActionFilters
{
	public class ModelStateValidationActionFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var message = ValidationProblemDetails.FromErrors(context.ModelState.ToValidationErrorMessages());
				context.Result = new BadRequestObjectResult(message);
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}
