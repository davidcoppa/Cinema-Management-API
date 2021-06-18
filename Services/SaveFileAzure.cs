using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cinema.Services
{
    public class SaveFileAzure : ISaveFile
    {
        private readonly string connectionString;

        public SaveFileAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task DeleteFile(string url, string container)
        {
            if (url != null)
            {
                var account = CloudStorageAccount.Parse(connectionString);
                var client = account.CreateCloudBlobClient();
                var reference = client.GetContainerReference(container);

                var blobName = Path.GetFileName(url);
                var blob = reference.GetBlobReference(blobName);
                await blob.DeleteIfExistsAsync();
            }
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string url, string contentType)
        {
            await DeleteFile(url, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer reference = client.GetContainerReference(container);

            await reference.CreateIfNotExistsAsync();
            await reference.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            string fileName = $"{Guid.NewGuid()}{extension}";
            CloudBlockBlob blob = reference.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = contentType;
            await blob.SetPropertiesAsync();
            return blob.Uri.ToString();
        }
    }
}
