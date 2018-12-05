using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Contracts.Documents;
using Apic.Services.AzureStorage;
using Apic.Services.Documents;
using DocumentDbo = Apic.Entities.Documents.Document;

namespace Apic.Facades.Mappers
{
	public interface IDocumentMapper
	{
		Task<List<Document>> Map(IQueryable<DocumentDbo> documents);
		Task<Document> Map(DocumentDbo document);
		DocumentDbo Map(int customerId, DocumentCreate document);
	}

	[ScopedService]
	public class DocumentMapper : IDocumentMapper
	{
		private readonly AzureStorageService azureStorageService;

		public DocumentMapper(AzureStorageService azureStorageService)
		{
			this.azureStorageService = azureStorageService;
		}

		public async Task<List<Document>> Map(IQueryable<DocumentDbo> documents)
		{
			List<Document> result = documents.Select(x => new Document()
			{
				Id = x.Id,
				CustomerId = x.CustomerId,
				Name = x.Name,
				ContentType = x.ContentType
			}).ToList();

			foreach (var document in result)
			{
				try
				{
					document.Url =
						await azureStorageService.GetBlobUrl(ContainerPaths.CustomerDocuments(document.CustomerId),
							document.Id.ToString());
					document.Availability = "OK";
				}
				catch (Exception e)
				{
					document.Availability = "N/A: " + e.Message;
				}
			}

			return result;
		}

		public async Task<Document> Map(DocumentDbo document)
		{
			return (await Map(new List<DocumentDbo>() { document }.AsQueryable())).FirstOrDefault();
		}

		public DocumentDbo Map(int customerId, DocumentCreate document)
		{
			return new DocumentDbo()
			{
				ContentType = document.ContentType,
				Name = document.Name,
				CustomerId = customerId
			};
		}
	}
}