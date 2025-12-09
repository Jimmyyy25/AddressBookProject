using PropelTechAddressBook.Server.Models;

namespace PropelTechAddressBook.Server.Services.Interfaces
{
    public interface IAddressBookService
    {
        Task<IEnumerable<AddressBookLine>> GetAllAsync();
        Task<AddressBookLine?> GetByEmailAsync(string email);
        Task<AddressBookLine> UpdateAsync(AddressBookLine addressBookLine);
        Task<AddressBookLine> CreateAsync(AddressBookLine addressBookLine);
        Task DeleteAsync(string email);
    }
}
