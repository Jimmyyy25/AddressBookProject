using AddressBookProject.Server.Controllers;
using AddressBookProject.Server.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace AddressBookProject.Server.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;
        private readonly string _blobName;

        public BlobStorageService(IOptions<AzureStorageOptions> options, ILogger<AddressBookController> logger)
        {
            var cfg = options.Value;

            logger.LogInformation(
                "Blob config: Container={Container}, Blob={Blob}",
                cfg.ContainerName,
                cfg.BlobName);


            _container = new BlobContainerClient(cfg.ConnectionString, cfg.ContainerName);
            _blobName = cfg.BlobName;
        }

        public async Task<string> ReadAsync()
        {
            var blob = _container.GetBlobClient(_blobName);
            var response = await blob.DownloadContentAsync();
            return response.Value.Content.ToString();
        }

        public async Task WriteAsync(string json)
        {
            var blob = _container.GetBlobClient(_blobName);
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

            await blob.UploadAsync(stream, overwrite: true);
        }
    }
}
