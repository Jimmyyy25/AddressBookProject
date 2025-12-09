using Microsoft.Extensions.Options;
using PropelTechAddressBook.Server.Controllers;
using PropelTechAddressBook.Server.Core;
using PropelTechAddressBook.Server.Models;
using PropelTechAddressBook.Server.Services.Interfaces;
using System.Text.Json;

namespace PropelTechAddressBook.Server.Services;

public class AddressBookService(IOptions<Configuration> config, ILogger<AddressBookController> logger) : IAddressBookService
{
    private readonly Configuration _config = config.Value;

    private string AddressBookFilePath => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _config.DataFolderPath));

    // implement interface methods here
    public IEnumerable<AddressBookLine> GetAll()
    {
        Utils.ValidateFilePath(AddressBookFilePath);

        string fileContents = Utils.ReadFileContents(AddressBookFilePath);

        // Deserialize data to ensure it's valid JSON and matches expected structure
        var lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents) ?? [];

        return lines;
    }

    public AddressBookLine? GetByEmail(string email)
    {
        Utils.ValidateFilePath(AddressBookFilePath);
        Utils.ValidateEmail(email);

        // Always work on fresh read of file to avoid stale data issues
        string fileContents = Utils.ReadFileContents(AddressBookFilePath);

        // Deserialize data so the 1 line can be found
        IEnumerable<AddressBookLine> lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents) ?? [];

        if (!lines.Any())
            return null; 

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        return line;
    }

    public AddressBookLine Update(AddressBookLine addressBookLine)
    {
        Utils.ValidateFilePath(AddressBookFilePath);
        Utils.ValidateEmail(addressBookLine.Email);

        // Always work on fresh read of file to avoid stale data issues
        string fileContents = Utils.ReadFileContents(AddressBookFilePath);

        IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents) ?? [];

        if (!lines.Any())
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(addressBookLine.Email, StringComparison.OrdinalIgnoreCase)) 
                                ?? 
                                    throw new KeyNotFoundException($"Contact not found: {addressBookLine.Email}");

        line.FirstName = addressBookLine.FirstName;
        line.LastName = addressBookLine.LastName;
        line.Phone = addressBookLine.Phone;
        line.Email = addressBookLine.Email;

        Utils.WriteFileContents(AddressBookFilePath, lines);

        return line;
    }

    public AddressBookLine Create(AddressBookLine addressBookLine)
    {
        Utils.ValidateFilePath(AddressBookFilePath);
        Utils.ValidateEmail(addressBookLine.Email);

        // Always work on fresh read of file to avoid stale data issues
        string fileContents = Utils.ReadFileContents(AddressBookFilePath);

        IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents) ?? [];

        if (!lines.Any())
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(addressBookLine.Email, StringComparison.OrdinalIgnoreCase));
        
        if (line != null)
            throw new InvalidOperationException($"Contact with email already exists: {addressBookLine.Email}");

        List<AddressBookLine> updatedLines = lines.ToList();
        updatedLines.Add(addressBookLine);

        Utils.WriteFileContents(AddressBookFilePath, updatedLines);

        return addressBookLine;
    }

    public void Delete(string email)
    {
        Utils.ValidateFilePath(AddressBookFilePath);
        Utils.ValidateEmail(email);

        // Always work on fresh read of file to avoid stale data issues
        string fileContents = Utils.ReadFileContents(AddressBookFilePath);

        IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents) ?? [];

        if (!lines.Any())
            throw new Exception("Address Book is empty or contents are in an invalid format.");

        AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) 
                                ?? 
                                    throw new KeyNotFoundException($"Contact not found: {email}");

        List<AddressBookLine> updatedLines = lines.ToList();
        updatedLines.Remove(line);

        Utils.WriteFileContents(AddressBookFilePath, updatedLines);
    }
}
