using Microsoft.AspNetCore.Mvc;
using PropelTechAddressBook.Server.Models;
using PropelTechAddressBook.Server.Services.Interfaces;

namespace PropelTechAddressBook.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController(IAddressBookService addressBookService,
                                   ILogger<AddressBookController> logger) : ControllerBase
{
    private readonly ILogger<AddressBookController> _logger = logger;

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {
        try
        {
            IEnumerable<AddressBookLine> lines = addressBookService.GetAll();
            return Ok(new { isSuccess = true, payload = lines });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("[action]")]
    public IActionResult GetByEmail(string email)
    {
        try
        {
            AddressBookLine? line = addressBookService.GetByEmail(email);
            
            if (line == null)
                return NotFound(new { isSuccess = false, message = $"Contact not found: {email}" });
         
            return Ok(new { isSuccess = true, payload = line });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("[action]")]
    public IActionResult Update([FromBody] AddressBookLine updatedEntry)
    {
        try
        {
            AddressBookLine line = addressBookService.Update(updatedEntry);
            return Ok(new { isSuccess = true, payload = line });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromBody] AddressBookLine newEntry)
    {
        try
        {
            AddressBookLine line = addressBookService.Create(newEntry);
            return Ok(new { isSuccess = true, payload = line });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("[action]")]
    public IActionResult Delete(string email)
    {
        try
        {
            addressBookService.Delete(email);
            return Ok(new { isSuccess = true, message = $"Contact deleted: {email}" });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private IActionResult HandleException(Exception ex)
    {
        if (ex is FileNotFoundException)
            return NotFound(new { isSuccess = false, message = ex.Message });

        _logger.LogError(ex, "Unhandled exception");

        return StatusCode(500, new { isSuccess = false, message = "Unexpected server error." });
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