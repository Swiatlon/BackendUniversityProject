using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressOnlyDto>>> GetAddresses()
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressFullDto>> GetAddressById(Guid id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);
            if (address is null)
                return NotFound("Address not found");
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<AddressFullDto>> AddAddress(AddressFullDto addressDto)
        {
            var address = await _addressService.AddAddressAsync(addressDto);
            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(Guid id, AddressFullDto addressDto)
        {
            if (id != addressDto.Id)
                return BadRequest("Address ID mismatch");

            var updatedAddress = await _addressService.UpdateAddressAsync(addressDto);
            if (updatedAddress is null)
                return NotFound("Address not found");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);
            if (address is null)
                return NotFound("Address not found");

            await _addressService.DeleteAddressAsync(id);
            return NoContent();
        }
    }
}
