using Microsoft.AspNetCore.Mvc;
using AddressBookProject.Server.Core;
using AddressBookProject.Server.Models;
using AddressBookProject.Server.Services.Interfaces;

namespace AddressBookProject.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController(IAddressBookService addressBookService,
                                   ILogger<AddressBookController> logger) : ControllerBase
{
    private readonly ILogger<AddressBookController> _logger = logger;

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            IEnumerable<AddressBookLine> lines = await addressBookService.GetAllAsync();
            return Ok(new { isSuccess = true, payload = lines });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            Utils.ValidateEmail(email);

            AddressBookLine? line = await addressBookService.GetByEmailAsync(email);
            
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
    public async Task<IActionResult> Update([FromBody] AddressBookLine updatedEntry)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            AddressBookLine line = await addressBookService.UpdateAsync(updatedEntry);
            return Ok(new { isSuccess = true, payload = line });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] AddressBookLine newEntry)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            AddressBookLine line = await addressBookService.CreateAsync(newEntry);
            return Ok(new { isSuccess = true, payload = line });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("[action]/{email}")]
    public async Task<IActionResult> Delete(string email)
    {
        try
        {
            Utils.ValidateEmail(email);

            await addressBookService.DeleteAsync(email);
            return Ok(new { isSuccess = true, message = $"Contact deleted: {email}" });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private IActionResult HandleException(Exception ex)
    {
        if (ex is FileNotFoundException || ex is KeyNotFoundException)
            return NotFound(new { isSuccess = false, message = ex.Message }); // 404
        else if (ex is ArgumentException || ex is InvalidOperationException || ex is FormatException)
            return BadRequest(new { isSuccess = false, message = ex.Message }); // 400 

        // if we reach here - unexpected error on our part - log and return generic message
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