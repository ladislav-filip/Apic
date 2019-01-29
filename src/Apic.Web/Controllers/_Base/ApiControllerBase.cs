using System.Net;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Services;
using Microsoft.AspNetCore.Mvc;
using StatusResults = Apic.Contracts.Infrastructure.Transfer.StatusResults;

namespace Apic.Web.Controllers._Base
{
    [Route("api")]
	[ProducesResponseType(typeof(StatusResults.ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), (int)HttpStatusCode.NotFound)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), (int)HttpStatusCode.InternalServerError)]
	public class ApiControllerBase : ControllerBase
    {
        private readonly ModelStateAccessor modelStateAccessor;

        public ApiControllerBase(ModelStateAccessor modelStateAccessor)
        {
            this.modelStateAccessor = modelStateAccessor;
        }

        public OkObjectResult Ok<T>(T value)
        {
            var result = new Result<T>
            {
                Messages = modelStateAccessor.Messages,
                Data = value
            };

            return base.Ok(result);
        }
    }
}
