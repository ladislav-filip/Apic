﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Common.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Apic.Services.AzureStorage
{
	public class AzureStorageService
	{
		private readonly AppSettings appSettings;

		public AzureStorageService(IOptions<AppSettings> appSettings)
		{
			this.appSettings = appSettings.Value;
		}

		public async Task<string> GetBlobUrl(string containerName, string blobName)
		{
			var container = Client.GetContainerReference(containerName);
			var blob = container.GetBlockBlobReference(blobName);

			return blob.Uri.ToString();
		}

		public async Task UploadAsync(Stream dataStream, string containerName, string blobName, CancellationToken cancellationToken)
		{
			var container = Client.GetContainerReference(containerName);
			await container.CreateIfNotExistsAsync();

			var blob = container.GetBlockBlobReference(blobName);

			dataStream.Position = 0;
			await blob.UploadFromStreamAsync(dataStream, AccessCondition.GenerateEmptyCondition(), new BlobRequestOptions(), new OperationContext(), cancellationToken);
			dataStream.Dispose();
		}

		public CloudBlobClient Client
		{
			get
			{
				CloudStorageAccount account = CloudStorageAccount.Parse(appSettings.ConnectionStrings.AzureStorage);
				CloudBlobClient client = account.CreateCloudBlobClient();
				
				return client;
			}
		}
	}
}