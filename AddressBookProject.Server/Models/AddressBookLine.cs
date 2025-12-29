using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AddressBookProject.Server.Models;

// TODO: Create DTOs CreateAddressBookRequest, UpdateAddressBookRequest
// TODO: Seen FluentValidation which looks good, could use this
// [Required] vs required - validation messaging different - discussion point

public class AddressBookLine
{
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    [JsonPropertyName("first_name")]
    public required string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    [JsonPropertyName("last_name")]
    public required string LastName { get; set; }

    [Required]
    [Phone]
    [JsonPropertyName("phone")]
    public required string Phone { get; set; }

    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
}
