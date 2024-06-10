using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AddressService
    {
        private readonly IAddressReposiotry _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressReposiotry addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressOnlyDto>> GetAllAddressesAsync()
        {
            var addresses = await _addressRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AddressOnlyDto>>(addresses);
        }

        public async Task<AddressFullDto> GetAddressByIdAsync(Guid id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address is not null)
            {
                return _mapper.Map<AddressFullDto>(address);
            }
            else
            {
                throw new KeyNotFoundException("Address not found");
            }
        }

        public async Task<AddressFullDto> AddAddressAsync(AddressFullDto addressDto)
        {
            var addressEntity = _mapper.Map<Address>(addressDto);
            var address = await _addressRepository.CreateAsync(addressEntity);
            return _mapper.Map<AddressFullDto>(address);
        }

        public async Task<AddressFullDto> UpdateAddressAsync(AddressFullDto addressDto)
        {
            var addressEntity = await _addressRepository.GetByIdAsync(addressDto.Id);

            if (addressEntity is null)
            {
                throw new KeyNotFoundException("Address not found");
            }

            _mapper.Map(addressDto, addressEntity);

            var address = await _addressRepository.UpdateAsync(addressEntity);
            return _mapper.Map<AddressFullDto>(address);
        }

        public async Task DeleteAddressAsync(Guid id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address is null)
            {
                throw new KeyNotFoundException("Address not found");
            }
            await _addressRepository.DeleteAsync(id);
        }
    }
}
