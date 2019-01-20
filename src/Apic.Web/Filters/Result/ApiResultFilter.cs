using System.Linq;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Services;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StatusResults = Apic.Contracts.Infrastructure.Transfer.StatusResults;

namespace Apic.Web.Filters.Result
{
    /// <summary>
    /// Úprava výsledné podoby response, owrapování výsledku zpracování
    /// </summary>
    public class ApiResultFilter : ResultFilterAttribute
    {
        private readonly ModelStateAccessor modelStateAccessor;

        public ApiResultFilter(ModelStateAccessor modelStateAccessor)
        {
            this.modelStateAccessor = modelStateAccessor;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = StatusResults.ValidationProblemDetails.FromErrors(context.ModelState.ToValidationErrorMessages());
                context.Result = new BadRequestObjectResult(message);
                return;
            }

            //CreatedResult createdResult = context.Result as CreatedResult;
            //NoContentResult noResult = context.Result as NoContentResult;
            OkObjectResult okResult = context.Result as OkObjectResult;
            if (okResult != null)
            {
                okResult.Value = new Result<object>
                {
                    Messages = modelStateAccessor.Messages.ToList(),
                    Data = okResult.Value
                };
            }

            base.OnResultExecuting(context);
        }
    }
}
