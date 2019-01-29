using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Apic.Contracts.Documents;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Documents;
using Apic.Services;
using Apic.Web.Attributes;
using Apic.Web.Controllers._Base;
using Apic.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apic.Web.Controllers
{
	[Area("customers")]
	public class DocumentsController : ApiControllerBase
	{
		private readonly IDocumentFacade documentFacade;

		public DocumentsController(IDocumentFacade documentFacade, ModelStateAccessor modelStateAccessor) : base(modelStateAccessor)
		{
			this.documentFacade = documentFacade;
		}

		[Route("customers/{customerId}/documents")]
		[HttpPost]
		[ProducesResponseType(typeof(Result<Document>), (int)HttpStatusCode.Created)]
		[Consumes("multipart/form-data")]
		[RequestFormLimits(MultipartBodyLengthLimit = 10_485_760)] // max 10 MB
		[RequestSizeLimit(10_485_760)] // max 10 MB
		public async Task<IActionResult> Upload(int customerId, IFormFile file, CancellationToken cancellationToken)
		{
            Document result = await documentFacade.UploadDocument(customerId, new DocumentCreate()
			{
				Name = file.FileName,
				ContentType = file.ContentType,
				Datastream = file.OpenReadStream()
			}, cancellationToken);

			return Ok(result);
		}

		[Route("customers/{customerId}/documents/big")]
		[HttpPost]
		[ProducesResponseType(typeof(Result<Document>), (int)HttpStatusCode.Created)]
		[RequestFormLimits(MultipartBodyLengthLimit = 1073741824)] // 1 GB
		[RequestSizeLimit(1073741824)] // 1 GB
		[DisableFormValueModelBinding]
		public async Task<IActionResult> UploadBig(int customerId, CancellationToken cancellationToken)
		{
			DocumentCreate document = new DocumentCreate()
			{
				ContentType = Request.ContentType,
			};

			using (document.Datastream = new MemoryStream())
			{
				await Request.StreamFile(document.Datastream);

                Document result = await documentFacade.UploadDocument(customerId, document, cancellationToken);

				return Ok(result);
			}
		}
	}
}
