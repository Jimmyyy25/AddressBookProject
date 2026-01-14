using AddressBookProject.Server.Models;
using AddressBookProject.Server.Services.Interfaces;

namespace AddressBookProject.Server.Services;

public class AddressBookService(IAddressBookDataStore addressBookDataStore) : IAddressBookService
{
    public async Task<IEnumerable<AddressBookLine>> GetAllAsync() 
        => await addressBookDataStore.GetAllAsync();

    public async Task<AddressBookLine?> GetByEmailAsync(string email) 
        => await addressBookDataStore.GetByEmailAsync(email);

    public async Task<AddressBookLine> UpdateAsync(AddressBookLine addressBookLine) 
        => await addressBookDataStore.UpdateAsync(addressBookLine);

    public async Task<AddressBookLine> CreateAsync(AddressBookLine addressBookLine) 
        => await addressBookDataStore.CreateAsync(addressBookLine);

    public async Task DeleteAsync(string email) 
        => await addressBookDataStore.DeleteAsync(email);
}
