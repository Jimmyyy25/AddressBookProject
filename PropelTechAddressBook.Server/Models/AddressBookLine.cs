using System.Text.Json.Serialization;

namespace PropelTechAddressBook.Server.Models;

public record AddressBookLine
{
    [JsonPropertyName("first_name")]
    public required string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; set; }

    [JsonPropertyName("phone")]
    public required string Phone { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
