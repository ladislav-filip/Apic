using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Apic.Services.AzureStorage
{
    public interface IAzureStorageService
    {
        string GetBlobUrl(string containerName, string blobName);
        Task UploadAsync(Stream dataStream, string containerName, string blobName, CancellationToken cancellationToken);
        CloudBlobClient Client { get; }
    }
}