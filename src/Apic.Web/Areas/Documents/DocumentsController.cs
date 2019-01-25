using System;
using System.Net;
using System.Threading.Tasks;
using Apic.Contracts.Documents;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Documents;
using Apic.Services;
using Apic.Web.Areas._Base;
using Microsoft.AspNetCore.Mvc;

namespace Apic.Web.Areas.Documents
{
    [Area("documents")]
	public class CustomerDocumentsController : ApiControllerBase
	{
		private readonly IDocumentFacade documentFacade;

		public CustomerDocumentsController(IDocumentFacade documentFacade, ModelStateAccessor modelStateAccessor) : base(modelStateAccessor)
		{
			this.documentFacade = documentFacade;
		}

		[Route("documents/{id}")]
		[HttpGet]
		[ProducesResponseType(typeof(Result<Document>), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Get(Guid id)
		{
            Document result = await documentFacade.Get(id);

			return Ok(result);
		}
	}
}
