using PropelTechAddressBook.Server.Models;

namespace PropelTechAddressBook.Server.Services.Interfaces
{
    public interface IAddressBookService
    {
        IEnumerable<AddressBookLine> GetAll();
        AddressBookLine? GetByEmail(string email);
        AddressBookLine Update(AddressBookLine addressBookLine);
        AddressBookLine Create(AddressBookLine addressBookLine);
        void Delete(string email);
    }
}
