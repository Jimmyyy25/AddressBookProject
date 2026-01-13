using AddressBookProject.Server.Controllers;
using AddressBookProject.Server.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System.Text.Json;

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
            try
            {
                var blob = _container.GetBlobClient(_blobName);
                var response = await blob.DownloadContentAsync();
                return response.Value.Content.ToString();
            }
            catch (Exception ex)
            {
                // Blob not found, return empty content
                return JsonSerializer.Serialize(_container) + " - " + JsonSerializer.Serialize(ex);
            }
        }

        public async Task WriteAsync(string json)
        {
            var blob = _container.GetBlobClient(_blobName);
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

            await blob.UploadAsync(stream, overwrite: true);
        }
    }
}
