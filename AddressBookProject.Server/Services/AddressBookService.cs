using Microsoft.Extensions.Options;
using AddressBookProject.Server.Core;
using AddressBookProject.Server.Models;
using AddressBookProject.Server.Services.Interfaces;
using System.Text.Json;

namespace AddressBookProject.Server.Services;

public class AddressBookService(IOptions<Configuration> config, BlobStorageService blobStorageService) : IAddressBookService
{
    private readonly Configuration _config = config.Value;

    private string AddressBookFilePath => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _config.DataFolderPath));

    // implement interface methods here
    public async Task<IEnumerable<AddressBookLine>> GetAllAsync()
    {
        //string fileContents = await Utils.ReadFileContents(AddressBookFilePath);

        var json = await blobStorageService.ReadAsync();
        // deserialize, modify, reserialize
        //await blobStorageService.WriteAsync(updatedJson);


        // Deserialize data to ensure it's valid JSON and matches expected structure
        //var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(fileContents) ?? [];
        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(json) ?? [];

        return lines;
    }

    public async Task<AddressBookLine?> GetByEmailAsync(string email)
    {
        // Always work on fresh read of file to avoid stale data issues
        string fileContents = await Utils.ReadFileContents(AddressBookFilePath);

        // Deserialize data so the 1 line can be found
        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(fileContents) ?? [];

        if (lines.Count == 0)
            return null;

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        return line;
    }

    public async Task<AddressBookLine> UpdateAsync(AddressBookLine addressBookLine)
    {
        // Always work on fresh read of file to avoid stale data issues
        string fileContents = await Utils.ReadFileContents(AddressBookFilePath);

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(fileContents) ?? new List<AddressBookLine>();

        if (lines.Count == 0)
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(addressBookLine.Email, StringComparison.OrdinalIgnoreCase)) 
                                ?? 
                                    throw new KeyNotFoundException($"Contact not found: {addressBookLine.Email}");

        line.FirstName = addressBookLine.FirstName;
        line.LastName = addressBookLine.LastName;
        line.Phone = addressBookLine.Phone;
        line.Email = addressBookLine.Email;

        await Utils.WriteFileContents(AddressBookFilePath, lines);

        return line;
    }

    public async Task<AddressBookLine> CreateAsync(AddressBookLine addressBookLine)
    {
        // Always work on fresh read of file to avoid stale data issues
        string fileContents = await Utils.ReadFileContents(AddressBookFilePath);

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(fileContents) ?? new List<AddressBookLine>();

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(addressBookLine.Email, StringComparison.OrdinalIgnoreCase));
        
        if (line != null)
            throw new InvalidOperationException($"Contact with email already exists: {addressBookLine.Email}");

        lines.Add(addressBookLine);

        await Utils.WriteFileContents(AddressBookFilePath, lines);

        return addressBookLine;
    }

    public async Task DeleteAsync(string email)
    {
        // Always work on fresh read of file to avoid stale data issues
        string fileContents = await Utils.ReadFileContents(AddressBookFilePath);

        var lines = JsonSerializer.Deserialize<List<AddressBookLine>>(fileContents) ?? new List<AddressBookLine>();

        if (lines.Count == 0)
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) 
                                ?? 
                                    throw new KeyNotFoundException($"Contact not found: {email}");

        lines.Remove(line);

        await Utils.WriteFileContents(AddressBookFilePath, lines);
    }
}
