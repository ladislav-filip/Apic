using System.Net;
using Microsoft.AspNetCore.Mvc;
using StatusResults = Apic.Contracts.Infrastructure.Transfer.StatusResults;

namespace Apic.Web.Areas._Base
{
    [Route("api")]
	[ProducesResponseType(typeof(StatusResults.ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), (int)HttpStatusCode.NotFound)]
	[ProducesResponseType(typeof(StatusResults.ProblemDetails), (int)HttpStatusCode.InternalServerError)]
	public class ApiControllerBase : ControllerBase
	{
	}
}
