using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StatusResults = Apic.Contracts.Infrastructure.Transfer.StatusResults;

namespace Apic.Web.Areas._Base
{
	[Route("api")]
	[ProducesResponseType(typeof(StatusResults.ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), (int)HttpStatusCode.NotFound)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), (int)HttpStatusCode.InternalServerError)]
	public class ApiControllerBase : ControllerBase
	{
		[NonAction]
		public T UpdateModelState<T>(T result) where T : Result
		{
			if ((int)result.Code < 300)
			{
				return result;
			}

			foreach (ResultMessage validationMessage in result.Messages)
			{
				ModelState.AddModelError(validationMessage.Property, validationMessage.Message);
			}

			return result;
		}

		[NonAction]
		public IActionResult ErrorResult(Result result)
		{
			if (result.Code == ResultCodes.NotFound)
			{
				return NotFound(StatusResults.ProblemDetails.FromMessage(HttpStatusCode.NotFound));
			}

			if (result.Code == ResultCodes.BadRequest || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return null;
		}

		[NonAction]
		public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
		{
			var message = StatusResults.ValidationProblemDetails.FromErrors(modelState.ToValidationErrorMessages());

			return BadRequest(message);
		}

		[NonAction]
		protected async Task<IActionResult> ReturnResult(Func<Task<Result>> result, IActionResult success)
		{
			Result action = await result.Invoke();

			UpdateModelState(action);

			return ErrorResult(action) ??
			       success;
		}
	}
}