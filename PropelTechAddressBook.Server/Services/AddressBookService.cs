using PropelTechAddressBook.Server.Models;
using PropelTechAddressBook.Server.Services.Interfaces;

namespace PropelTechAddressBook.Server.Services;

public class AddressBookService : IAddressBookService
{
    // implement interface methods here
    public IEnumerable<AddressBookLine> GetAllEntries()
    {
        return null;
    }

    public AddressBookLine? GetEntryByEmail(string email)
    {
        return null;
    }
}
