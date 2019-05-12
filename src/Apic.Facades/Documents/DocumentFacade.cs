using System;
using System.Threading;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Common.Exceptions;
using Apic.Contracts.Documents;
using Apic.Data.Context;
using Apic.Data.Repositories;
using Apic.Entities.Customers;
using Apic.Services.AzureStorage;
using Apic.Services.Documents;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DocumentDbo = Apic.Entities.Documents.Document;


namespace Apic.Facades.Documents
{
    [ScopedService]
	public class DocumentFacade : IDocumentFacade
	{
		private readonly IUnitOfWork uow;
		private readonly IAzureStorageService azureStorageService;
		private readonly IMapper mapper;
 
		public DocumentFacade(IUnitOfWork uow, IAzureStorageService azureStorageService, IMapper mapper)
		{
			this.uow = uow;
			this.azureStorageService = azureStorageService;
			this.mapper = mapper;
        }

		public async Task<Document> Get(Guid id)
        {
            DocumentDbo document = await uow.Documents.GetSingle(id);
            Document doc = mapper.Map<Document>(document);

			return doc;
		}

		public async Task<Document> UploadDocument(int customerId, DocumentCreate createRequest, CancellationToken cancellationToken)
		{
			bool existsCustomer = await uow.Set<Customer>().AnyAsync(x => x.Id == customerId, cancellationToken);
			if (!existsCustomer)
			{
                throw new ObjectNotFoundException("Customer nebyl nalezen!");
			}

			string containerName = ContainerPaths.CustomerDocuments(customerId);

			DocumentDbo document = mapper.Map<DocumentDbo>(createRequest, x=> x.Items.Add("customerId", customerId));

			await azureStorageService.UploadAsync(createRequest.Datastream, containerName, document.Id.ToString(), cancellationToken);

			uow.Add(document);
			await uow.SaveChangesAsync(cancellationToken);

			return mapper.Map<Document>(document);
		}
	}
}
