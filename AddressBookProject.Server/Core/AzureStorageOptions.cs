namespace AddressBookProject.Server.Core
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
        public string BlobName { get; set; } = null!;
    }

}
