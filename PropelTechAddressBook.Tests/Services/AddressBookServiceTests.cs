using System.Text.Json;
using Microsoft.Extensions.Options;
using PropelTechAddressBook.Server.Models;
using PropelTechAddressBook.Server.Services;
using Xunit;
//using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropelTechAddressBook.Server.Core;

public class AddressBookServiceTests
{
    // GitHub Copilot generated tests - needs Microsoft Fakes for Utils class etc. - requires more learning

    //private readonly Mock<IOptions<Configuration>> _configMock;
    //private readonly string _filePath = "test.json";
    //private readonly Configuration _config;

    //public AddressBookServiceTests()
    //{
    //    _config = new Configuration { DataFolderPath = _filePath };
    //    _configMock = new Mock<IOptions<Configuration>>();
    //    _configMock.Setup(c => c.Value).Returns(_config);
    //}

    //private AddressBookService CreateService() => new AddressBookService(_configMock.Object);

    //private AddressBookLine GetSampleLine() => new AddressBookLine
    //{
    //    FirstName = "John",
    //    LastName = "Doe",
    //    Phone = "123456789",
    //    Email = "john@doe.com"
    //};

    //[Fact]
    //public async Task GetAllAsync_ReturnsAllLines()
    //{
    //    var lines = new List<AddressBookLine> { GetSampleLine() };
    //    var json = JsonSerializer.Serialize(lines);

    //    using (ShimsContext.Create())
    //    {
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ReadFileContentsString = _ => Task.FromResult(json);

    //        var service = CreateService();
    //        var result = await service.GetAllAsync();

    //        Assert.Single(result);
    //        Assert.Equal("john@doe.com", result.First().Email);
    //    }
    //}

    //[Fact]
    //public async Task GetByEmailAsync_ReturnsCorrectLine()
    //{
    //    var lines = new List<AddressBookLine> { GetSampleLine() };
    //    var json = JsonSerializer.Serialize(lines);

    //    using (ShimsContext.Create())
    //    {
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ValidateEmailString = _ => { };
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ReadFileContentsString = _ => Task.FromResult(json);

    //        var service = CreateService();
    //        var result = await service.GetByEmailAsync("john@doe.com");

    //        Assert.NotNull(result);
    //        Assert.Equal("John", result.FirstName);
    //    }
    //}

    //[Fact]
    //public async Task UpdateAsync_UpdatesLine()
    //{
    //    var lines = new List<AddressBookLine> { GetSampleLine() };
    //    var json = JsonSerializer.Serialize(lines);

    //    using (ShimsContext.Create())
    //    {
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ValidateEmailString = _ => { };
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ReadFileContentsString = _ => Task.FromResult(json);
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.WriteFileContentsStringIEnumerableOfAddressBookLine = (_, _) => Task.CompletedTask;

    //        var service = CreateService();
    //        var updatedLine = GetSampleLine();
    //        updatedLine.FirstName = "Jane";

    //        var result = await service.UpdateAsync(updatedLine);

    //        Assert.Equal("Jane", result.FirstName);
    //    }
    //}

    //[Fact]
    //public async Task CreateAsync_AddsNewLine()
    //{
    //    var lines = new List<AddressBookLine>();
    //    var json = JsonSerializer.Serialize(lines);

    //    using (ShimsContext.Create())
    //    {
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ValidateEmailString = _ => { };
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ReadFileContentsString = _ => Task.FromResult(json);
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.WriteFileContentsStringIEnumerableOfAddressBookLine = (_, _) => Task.CompletedTask;

    //        var service = CreateService();
    //        var newLine = GetSampleLine();

    //        var result = await service.CreateAsync(newLine);

    //        Assert.Equal("john@doe.com", result.Email);
    //    }
    //}

    //[Fact]
    //public async Task DeleteAsync_RemovesLine()
    //{
    //    var lines = new List<AddressBookLine> { GetSampleLine() };
    //    var json = JsonSerializer.Serialize(lines);

    //    using (ShimsContext.Create())
    //    {
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ValidateEmailString = _ => { };
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.ReadFileContentsString = _ => Task.FromResult(json);
    //        PropelTechAddressBook.Server.Core.Fakes.ShimUtils.WriteFileContentsStringIEnumerableOfAddressBookLine = (_, _) => Task.CompletedTask;

    //        var service = CreateService();
    //        await service.DeleteAsync("john@doe.com");

    //        // No exception means success
    //    }
    //}
}