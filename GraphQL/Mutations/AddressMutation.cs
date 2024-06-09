using Application.Dtos;
using Application.Services;

namespace GraphQL.Mutations
{
    public class AddressMutation
    {
        public async Task<AddressFullDto> AddAddress(AddressFullDto addressDto, [Service] AddressService addressService)
        {
            return await addressService.AddAddressAsync(addressDto);
        }

        public async Task<AddressFullDto> UpdateAddress(AddressFullDto addressDto, [Service] AddressService addressService)
        {
            return await addressService.UpdateAddressAsync(addressDto);
        }

        public async Task<bool> DeleteAddress(Guid id, [Service] AddressService addressService)
        {
            await addressService.DeleteAddressAsync(id);
            return true;
        }
    }
}
