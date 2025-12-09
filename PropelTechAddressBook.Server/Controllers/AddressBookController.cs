using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropelTechAddressBook.Server.Core;
using PropelTechAddressBook.Server.Models;
using PropelTechAddressBook.Server.Services.Interfaces;
using System.Text.Json;

namespace PropelTechAddressBook.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController(IOptions<Configuration> config, IAddressBookService addressBookService,
                                   ILogger<AddressBookController> logger) : ControllerBase
{
    private readonly Configuration _config = config.Value;
    private readonly ILogger<AddressBookController> _logger = logger;
    private string AddressBookFilePath => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _config.DataFolderPath));

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {

        try
        {
            Utils.ValidateFilePath(AddressBookFilePath);

            string fileContents = Utils.ReadFileContents(AddressBookFilePath);

            // Deserialize data to ensure it's valid JSON and matches expected structure
            IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents);

            // This should never happen unless the data file is corrupted or has invalid JSON
            if (lines == null)
                return StatusCode(500, new { isSuccess = false, message = "Data file contains invalid JSON." });

            return Ok(new { isSuccess = true, payload = lines });
        }
        catch (Exception e)
        {
            // Worth logging away for investigation. Could log to frontend assuming this is a internal CMS style app i.e. filePath won't be made public
            return StatusCode(500, new { isSuccess = false, message = $"Internal server error: {e.Message}" });
        }
    }

    [HttpGet("[action]")]
    public IActionResult GetByEmail(string email)
    {
        try
        {
            Utils.ValidateFilePath(AddressBookFilePath);
            Utils.ValidateEmail(email);

            // Always work on fresh read of file to avoid stale data issues
            string fileContents = Utils.ReadFileContents(AddressBookFilePath);

            // Deserialize data so the 1 line can be found
            IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents);

            // This should never happen unless the data file is corrupted or has invalid JSON
            if (lines == null)
                return StatusCode(500, new { isSuccess = false, message = "Data file contains invalid JSON." });

            AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            // This should never happen as the frontend should only be sending an unchangable email from a valid record.
            // If it does the request has been tampered with. Can still return NotFound anyway
            if (line == null)
                return NotFound($"Contact not found: {email}");

            return Ok(new { isSuccess = true, payload = line });
        }
        catch (Exception e)
        {
            // Worth logging away for investigation. Could log to frontend assuming this is a internal CMS style app i.e. filePath won't be made public
            return StatusCode(500, new { isSuccess = false, message = $"Internal server error: {e.Message}" });
        }
    }

    [HttpPut("[action]")]
    public IActionResult Update(string email, [FromBody] AddressBookLine updatedEntry)
    {
        try
        {
            Utils.ValidateFilePath(AddressBookFilePath);
            Utils.ValidateEmail(email);

            // Always work on fresh read of file to avoid stale data issues
            string fileContents = Utils.ReadFileContents(AddressBookFilePath);

            IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents);

            // This should never happen unless the data file is corrupted or has invalid JSON
            if (lines == null)
                return StatusCode(500, new { isSuccess = false, message = "Data file contains invalid JSON." });

            AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            // This should never happen as the frontend should only be sending an unchangable email from a valid record.
            // If it does the request has been tampered with. Can still return NotFound anyway
            if (line == null)
                return NotFound($"Contact not found: {email}");

            line.FirstName = updatedEntry.FirstName;
            line.LastName = updatedEntry.LastName;
            line.Phone = updatedEntry.Phone;
            line.Email = updatedEntry.Email;

            Utils.WriteFileContents(AddressBookFilePath, lines);

            return Ok(new { isSuccess = true, payload = lines });
        }
        catch (Exception e)
        {
            // Worth logging away for investigation. Could log to frontend assuming this is a internal CMS style app i.e. filePath won't be made public
            return StatusCode(500, new { isSuccess = false, message = $"Internal server error: {e.Message}" });
        }
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromBody] AddressBookLine newEntry)
    {
        try
        {
            Utils.ValidateFilePath(AddressBookFilePath);

            // Always work on fresh read of file to avoid stale data issues
            string fileContents = Utils.ReadFileContents(AddressBookFilePath);

            IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents);

            // This should never happen unless the data file is corrupted or has invalid JSON
            if (lines == null)
                return StatusCode(500, new { isSuccess = false, message = "Data file contains invalid JSON." });

            AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(newEntry.Email, StringComparison.OrdinalIgnoreCase));

            // Cannot write this - email must be unique
            if (line != null)
                return Conflict(new { isSuccess = false, message = $"Contact with email already exists: {newEntry.Email}" });

            List<AddressBookLine> updatedLines = lines.ToList();
            updatedLines.Add(newEntry);

            Utils.WriteFileContents(AddressBookFilePath, updatedLines);

            return Ok(new { isSuccess = true, payload = updatedLines });
        }
        catch (Exception e)
        {
            // Worth logging away for investigation. Could log to frontend assuming this is a internal CMS style app i.e. filePath won't be made public
            return StatusCode(500, new { isSuccess = false, message = $"Internal server error: {e.Message}" });
        }
    }

    [HttpDelete("[action]")]
    public IActionResult Delete(string email)
    {
        try
        {
            Utils.ValidateFilePath(AddressBookFilePath);
            Utils.ValidateEmail(email);

            // Always work on fresh read of file to avoid stale data issues
            string fileContents = Utils.ReadFileContents(AddressBookFilePath);

            IEnumerable<AddressBookLine>? lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents);

            // This should never happen unless the data file is corrupted or has invalid JSON
            if (lines == null)
                return StatusCode(500, new { isSuccess = false, message = "Data file contains invalid JSON." });

            AddressBookLine? line = lines.SingleOrDefault(l => l.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            // This should never happen as the frontend should only be sending an unchangable email from a valid record.
            // If it does the request has been tampered with. Can still return NotFound anyway
            if (line == null)
                return NotFound($"Contact not found: {email}");

            List<AddressBookLine> updatedLines = lines.ToList();
            updatedLines.Remove(line);

            Utils.WriteFileContents(AddressBookFilePath, updatedLines);

            return Ok(new { isSuccess = true, payload = updatedLines });
        }
        catch (Exception e)
        {
            // Worth logging away for investigation. Could log to frontend assuming this is a internal CMS style app i.e. filePath won't be made public
            return StatusCode(500, new { isSuccess = false, message = $"Internal server error: {e.Message}" });
        }
    }

}



// MISC COMMENTS
// -------------------

//if (!System.IO.File.Exists(filePath))
// return NotFound($"File not found: {filePath}"); - use if client dictates filepath (not the case here)


// Alternative way of reading file - for very large files or more granular reads so not needed here
//using (StreamReader sr = new(filePath))
//{
//    string fileContents = sr.ReadToEnd();

//    lines = JsonSerializer.Deserialize<IEnumerable<AddressBookLine>>(fileContents);
//}