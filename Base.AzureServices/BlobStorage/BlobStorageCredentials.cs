using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.AzureServices.BlobStorage
{
    public class BlobStorageCredentials
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}
