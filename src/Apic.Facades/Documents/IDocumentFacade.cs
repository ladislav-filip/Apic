using System;
using System.Threading;
using System.Threading.Tasks;
using Apic.Contracts.Documents;

namespace Apic.Facades.Documents
{
    public interface IDocumentFacade
    {
        Task<Document> Get(Guid id);
        Task<Document> UploadDocument(int customerId, DocumentCreate createRequest, CancellationToken cancellationToken);
    }
}