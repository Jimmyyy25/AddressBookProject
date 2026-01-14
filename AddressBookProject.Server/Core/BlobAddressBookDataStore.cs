using AddressBookProject.Server.Models;
using AddressBookProject.Server.Services;
using AddressBookProject.Server.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AddressBookProject.Server.Core;

public class BlobAddressBookDataStore(IOptions<Configuration> config, BlobStorageService blobStorageService) : IAddressBookDataStore
{
    private readonly Configuration _config = config.Value;
    
    // Cache the JsonSerializerOptions instance to reuse it
    private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

    private string AddressBookFilePath => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _config.DataFolderPath));

    // implement interface methods here
    public async Task<IEnumerable<AddressBookLine>> GetAllAsync()
    {
        var json = await blobStorageService.ReadAsync();
        
        // deserialize, modify, reserialize
        //await blobStorageService.WriteAsync(updatedJson);

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(json) ?? [];

        return lines;
    }

    public async Task<AddressBookLine?> GetByEmailAsync(string email)
    {
        var json = await blobStorageService.ReadAsync();

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(json) ?? [];

        if (lines.Count == 0)
            return null;

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        return line;
    }

    public async Task<AddressBookLine> UpdateAsync(AddressBookLine addressBookLine)
    {
        var json = await blobStorageService.ReadAsync();

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(json) ?? [];

        if (lines.Count == 0)
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(addressBookLine.Email, StringComparison.OrdinalIgnoreCase))
                                ??
                                    throw new KeyNotFoundException($"Contact not found: {addressBookLine.Email}");

        line.FirstName = addressBookLine.FirstName;
        line.LastName = addressBookLine.LastName;
        line.Phone = addressBookLine.Phone;
        line.Email = addressBookLine.Email;

        json = JsonSerializer.Serialize(lines, CachedJsonSerializerOptions);
        await blobStorageService.WriteAsync(json);

        return line;
    }

    public async Task<AddressBookLine> CreateAsync(AddressBookLine addressBookLine)
    {
        var json = await blobStorageService.ReadAsync();

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(json) ?? [];

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(addressBookLine.Email, StringComparison.OrdinalIgnoreCase));

        if (line != null)
            throw new InvalidOperationException($"Contact with email already exists: {addressBookLine.Email}");

        lines.Add(addressBookLine);

        json = JsonSerializer.Serialize(lines, CachedJsonSerializerOptions);
        await blobStorageService.WriteAsync(json);

        return addressBookLine;
    }

    public async Task DeleteAsync(string email)
    {
        var json = await blobStorageService.ReadAsync();

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(json) ?? [];

        if (lines.Count == 0)
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                                ??
                                    throw new KeyNotFoundException($"Contact not found: {email}");

        lines.Remove(line);

        json = JsonSerializer.Serialize(lines, CachedJsonSerializerOptions);
        await blobStorageService.WriteAsync(json);
    }
}
