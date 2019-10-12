using Apic.Contracts.Infrastructure.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusResults = Apic.Contracts.Infrastructure.Transfer.StatusResults;

namespace Apic.Web.Controllers._Base
{
    [Route("api")]
	[ProducesResponseType(typeof(StatusResults.ValidationProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ApiController]
	public class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase()
        {
        }
    }
}
