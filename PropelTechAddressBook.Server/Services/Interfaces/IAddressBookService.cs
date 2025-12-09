using PropelTechAddressBook.Server.Models;

namespace PropelTechAddressBook.Server.Services.Interfaces
{
    public interface IAddressBookService
    {
        IEnumerable<AddressBookLine> GetAllEntries();
        AddressBookLine? GetEntryByEmail(string email);
    }
}
