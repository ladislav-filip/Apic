using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Contracts.Documents;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Data.Context;
using Apic.Facades.Mappers;
using Apic.Services.AzureStorage;
using Apic.Services.Documents;
using Microsoft.EntityFrameworkCore;
using CustomerDto = Apic.Entities.Customers.Customer;
using DocumentDto = Apic.Entities.Documents.Document;


namespace Apic.Facades.Documents
{
	public interface IDocumentFacade
	{
		Task<DataResult<Document>> Get(Guid id);
		Task<DataResult<Document>> UploadDocument(int customerId, DocumentCreate createRequest, CancellationToken cancellationToken);
	}

	[ScopedService]
	public class DocumentFacade : IDocumentFacade
	{
		private readonly ApicDbContext dbContext;
		private readonly AzureStorageService azureStorageService;
		private readonly IDocumentMapper documentMapper;
 
		public DocumentFacade(ApicDbContext dbContext, AzureStorageService azureStorageService, IDocumentMapper documentMapper)
		{
			this.dbContext = dbContext;
			this.azureStorageService = azureStorageService;
			this.documentMapper = documentMapper;
		}

		public async Task<DataResult<Document>> Get(Guid id)
		{
			DocumentDto document = await dbContext.Documents.FirstOrDefaultAsync(x => x.Id == id);
			if (document == null)
			{
				return DataResult<Document>.NotFound("Dokument nebyl nalezen");
			}

			var doc = (await documentMapper.Map(dbContext.Documents.Where(x => x.Id == id))).FirstOrDefault();

			return DataResult<Document>.Ok(doc);
		}

		public async Task<DataResult<Document>> UploadDocument(int customerId, DocumentCreate createRequest, CancellationToken cancellationToken)
		{
			bool existsCustomer = await dbContext.Customers.AnyAsync(x => x.Id == customerId, cancellationToken);
			if (!existsCustomer)
			{
				return DataResult<Document>.NotFound("Customer nebyl nalezen!");
			}

			string containerName = ContainerPaths.CustomerDocuments(customerId);

			DocumentDto document = documentMapper.Map(customerId, createRequest);

			await azureStorageService.UploadAsync(createRequest.Datastream, containerName, document.Id.ToString(), cancellationToken);

			dbContext.Documents.Add(document);
			await dbContext.SaveChangesAsync(cancellationToken);

			return DataResult<Document>.Ok(await documentMapper.Map(document));
		}
	}
}
