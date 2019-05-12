using System;
using System.Net;
using System.Threading.Tasks;
using Apic.Contracts.Documents;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Documents;
using Apic.Services;
using Apic.Web.Controllers._Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apic.Web.Controllers
{
	public class CustomerDocumentsController : ApiControllerBase
	{
		private readonly IDocumentFacade documentFacade;

		public CustomerDocumentsController(IDocumentFacade documentFacade, ModelStateAccessor modelStateAccessor) : base(modelStateAccessor)
		{
			this.documentFacade = documentFacade;
		}

		[Route("documents/{id}")]
		[HttpGet, HttpHead]
        [ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<Result<Document>>> GetDocuments(Guid id)
		{
            Document result = await documentFacade.Get(id);

			return Ok(result);
		}
    }
}
