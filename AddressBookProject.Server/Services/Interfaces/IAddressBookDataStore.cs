using AddressBookProject.Server.Models;

namespace AddressBookProject.Server.Services.Interfaces
{
    public interface IAddressBookDataStore
    {
        Task<IEnumerable<AddressBookLine>> GetAllAsync();
        Task<AddressBookLine?> GetByEmailAsync(string email);
        Task<AddressBookLine> UpdateAsync(AddressBookLine addressBookLine);
        Task<AddressBookLine> CreateAsync(AddressBookLine addressBookLine);
        Task DeleteAsync(string email);
    }

}
