using System;
using System.Threading;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Common.Exceptions;
using Apic.Contracts.Documents;
using Apic.Data.Context;
using Apic.Services.AzureStorage;
using Apic.Services.Documents;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DocumentDbo = Apic.Entities.Documents.Document;


namespace Apic.Facades.Documents
{
    public interface IDocumentFacade
	{
		Task<Document> Get(Guid id);
		Task<Document> UploadDocument(int customerId, DocumentCreate createRequest, CancellationToken cancellationToken);
	}

	[ScopedService]
	public class DocumentFacade : IDocumentFacade
	{
		private readonly ApicDbContext dbContext;
		private readonly IAzureStorageService azureStorageService;
		private readonly IMapper mapper;
 
		public DocumentFacade(ApicDbContext dbContext, IAzureStorageService azureStorageService, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.azureStorageService = azureStorageService;
			this.mapper = mapper;
		}

		public async Task<Document> Get(Guid id)
		{
			DocumentDbo document = await dbContext.Documents.FirstOrDefaultAsync(x => x.Id == id);
			if (document == null)
			{
                throw new ObjectNotFoundException("Dokument nebyl nalezen");
			}

            Document doc = mapper.Map<Document>(document);

			return doc;
		}

		public async Task<Document> UploadDocument(int customerId, DocumentCreate createRequest, CancellationToken cancellationToken)
		{
			bool existsCustomer = await dbContext.Customers.AnyAsync(x => x.Id == customerId, cancellationToken);
			if (!existsCustomer)
			{
                throw new ObjectNotFoundException("Customer nebyl nalezen!");
			}

			string containerName = ContainerPaths.CustomerDocuments(customerId);

			DocumentDbo document = mapper.Map<DocumentDbo>(createRequest, x=> x.Items.Add("customerId", customerId));

			await azureStorageService.UploadAsync(createRequest.Datastream, containerName, document.Id.ToString(), cancellationToken);

			dbContext.Documents.Add(document);
			await dbContext.SaveChangesAsync(cancellationToken);

			return mapper.Map<Document>(document);
		}
	}
}
