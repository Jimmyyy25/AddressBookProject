using PropelTechAddressBook.Server.Models;
using System.Text.Json;

namespace PropelTechAddressBook.Server.Core;

public static class Utils
{
    public static void ValidateFilePath(string filePath)
    {
        // Bad appsettings.json configuration - filepath not quite right - dev mistake - 500 error
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found JC: {filePath}");

        // any extra validation can go here
    }

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.");

        // any extra validation can go here
        // e.g. regex pattern matching
    }

    public static string ReadFileContents(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public static void WriteFileContents(string filePath, IEnumerable<AddressBookLine>? lines)
    {
        string? json = JsonSerializer.Serialize(lines);

        // Completely replace file contents
        File.WriteAllText(filePath, json);
    }
}
