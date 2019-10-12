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

		public CustomerDocumentsController(IDocumentFacade documentFacade)
		{
			this.documentFacade = documentFacade;
		}

		[Route("documents/{id}")]
		[HttpGet, HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<Document>> GetDocuments(Guid id)
		{
            Document result = await documentFacade.Get(id);

			return Ok(result);
		}
    }
}
