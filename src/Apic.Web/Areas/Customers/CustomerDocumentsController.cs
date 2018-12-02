using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Apic.Contracts.Documents;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Documents;
using Apic.Web.Areas._Base;
using Apic.Web.Attributes;
using Apic.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apic.Web.Areas.Customers
{
	[Area("customers")]
	public class DocumentsController : ApiControllerBase
	{
		private readonly IDocumentFacade documentFacade;

		public DocumentsController(IDocumentFacade documentFacade)
		{
			this.documentFacade = documentFacade;
		}

		[Route("customers/{customerId}/documents")]
		[HttpPost]
		[ProducesResponseType(typeof(DataResult<Document>), (int)HttpStatusCode.Created)]
		[Consumes("multipart/form-data")]
		[RequestFormLimits(MultipartBodyLengthLimit = 10_485_760)] // max 10 MB
		[RequestSizeLimit(10_485_760)] // max 10 MB
		public async Task<IActionResult> Upload(int customerId, IFormFile file, CancellationToken cancellationToken)
		{
			DataResult<Document> result = await documentFacade.UploadDocument(customerId, new DocumentCreate()
			{
				Name = file.FileName,
				ContentType = file.ContentType,
				Datastream = file.OpenReadStream()
			}, cancellationToken);

			return ErrorResult(result) ??
			       Ok(result);
		}

		[Route("customers/{customerId}/documents/big")]
		[HttpPost]
		[ProducesResponseType(typeof(DataResult<Document>), (int)HttpStatusCode.Created)]
		[Consumes("multipart/form-data")]
		[RequestFormLimits(MultipartBodyLengthLimit = 1073741824)] // 1 GB
		[RequestSizeLimit(1073741824)] // 1 GB
		[DisableFormValueModelBinding]
		public async Task<IActionResult> UploadBig(int customerId, CancellationToken cancellationToken)
		{
			DocumentCreate document = new DocumentCreate()
			{
				ContentType = Request.ContentType,
			};

			await Request.StreamFile(document.Datastream);

			DataResult<Document> result = await documentFacade.UploadDocument(customerId, document, cancellationToken);

			return ErrorResult(result) ??
			       Ok(result);
		}
	}
}
