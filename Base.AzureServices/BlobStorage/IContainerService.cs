using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.AzureServices.BlobStorage
{
    public interface IContainerService
    {
        Task<string> UploadImageToBlobStorageAsync(string name, IFormFile image);
    }
}
