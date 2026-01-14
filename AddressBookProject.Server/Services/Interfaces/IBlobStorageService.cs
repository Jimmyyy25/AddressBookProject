namespace AddressBookProject.Server.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> ReadAsync();
        Task WriteAsync(string json);
    }
}
