using PropelTechAddressBook.Server.Models;
using System.Text.Json;

namespace PropelTechAddressBook.Server.Core;

public static class Utils
{
    // Cache the JsonSerializerOptions instance to reuse it
    private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.");

        // any extra validation can go here
        // e.g. regex pattern matching
    }

    public static void ValidateFilePath(string filePath)
    {
        // Bad appsettings.json configuration - filepath not quite right - dev mistake - 500 error
        // Alternatively could CreateDirectory or create seperate method to create file if not exists
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        // any extra validation can go here
    }

    // Supposedly can use SemaphoreSlim for multi user safety but not needed for this simple app 

    public async static Task<string> ReadFileContents(string filePath)
    {
        ValidateFilePath(filePath);

        return await File.ReadAllTextAsync(filePath);
    }

    public async static Task WriteFileContents(string filePath, List<AddressBookLine> lines)
    {
        ValidateFilePath(filePath);

        string json = JsonSerializer.Serialize(lines, CachedJsonSerializerOptions);

        // Completely replace file contents
        await File.WriteAllTextAsync(filePath, json);
    }
}
