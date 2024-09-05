using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.AzureServices.BlobStorage
{
    public class ContainerService : IContainerService
    {
        private readonly BlobStorageCredentials _blobStorageSettings;

        public ContainerService(IOptions<BlobStorageCredentials> blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task<string> UploadImageToBlobStorageAsync(string name, IFormFile image)
        {
            string connectionString = _blobStorageSettings.ConnectionString;
            string containerName = _blobStorageSettings.ContainerName;

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            string blobName = $"{name}~{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
