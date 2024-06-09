using Application.Dtos;
using Application.Services;

namespace GraphQL.Queries
{
    public class AddressQuery
    {
        public async Task<IEnumerable<AddressOnlyDto>> GetAddresses([Service] AddressService addressService)
        {
            return await addressService.GetAllAddressesAsync();
        }

        public async Task<AddressFullDto> GetAddressById(Guid id, [Service] AddressService addressService)
        {
            return await addressService.GetAddressByIdAsync(id);
        }
    }
}