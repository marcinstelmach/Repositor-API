using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IBlobService
    {
        CloudBlobClient CreateBlobClient();
        CloudBlobContainer CreateRepository(CloudBlobClient client, string repositoryName);
        CloudBlobDirectory CreateVersion(CloudBlobContainer blobContainer, string versionName);
        CloudBlockBlob AddFile(CloudBlobDirectory blobDirectory, string fileName, string path);
        
    }
}
