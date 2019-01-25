using Apic.Services.AzureStorage;
using Apic.Services.Documents;
using AutoMapper;
using DocumentDbo = Apic.Entities.Documents.Document;
using Document = Apic.Contracts.Documents.Document;

namespace Apic.Facades.Mappers.Resolvers
{
    public class BlobUrlValueResolver : IValueResolver<DocumentDbo, Document, string>
    {
        private readonly IAzureStorageService azureStorageService;

        public BlobUrlValueResolver(IAzureStorageService azureStorageService)
        {
            this.azureStorageService = azureStorageService;
        }

        public string Resolve(DocumentDbo source, Document destination, string member, ResolutionContext context)
        {
            return azureStorageService.GetBlobUrl(ContainerPaths.CustomerDocuments(source.CustomerId), source.Id.ToString());
        }
    }
}
