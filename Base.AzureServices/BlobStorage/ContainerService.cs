using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.AzureServices.BlobStorage
{
    public static class ContainerService
    {
        public static async Task<string> UploadImageToBlobStorageAsync(IFormFile image)
        {
            //string connectionString = Environment.GetEnvironmentVariable("BlobContainerConnection");
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=sbitemspicturestorage;AccountKey=/cEL61nMZWFWZCPIh2enO+MhDRdWrLyKKKzDPu9IJ0/RnUFihMsroC7/Y+CL5KgQ8yOocTDiS25/+ASty30Xtw==;EndpointSuffix=core.windows.net";
            string containerName = "items-imgs";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Create a unique name for the image
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload the image
            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
