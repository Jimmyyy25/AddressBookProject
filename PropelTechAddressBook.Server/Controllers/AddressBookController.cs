using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropelTechAddressBook.Server.Core;
using PropelTechAddressBook.Server.Models;
using System.Text.Json;

namespace PropelTechAddressBook.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController(IOptions<Configuration> config, ILogger<AddressBookController> logger) : ControllerBase
{
    private readonly Configuration _config = config.Value;
    private readonly ILogger<AddressBookController> _logger = logger;

    [HttpGet("GetList")]
    public IActionResult GetList()
    {
        string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _config.DataFolderPath));

        try
        {
            // Bad appsettings.json configuration - filepath not quite right - dev mistake - 500 error
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"File not found JC: {filePath}");

            string fileContents = System.IO.File.ReadAllText(filePath);

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