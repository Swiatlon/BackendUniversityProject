using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Addresses
{
    [Authorize(Policy = "AdminOrUserPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly AddressService _addressService;

        public DetailsModel(AddressService addressService)
        {
            _addressService = addressService;
        }

        public AddressFullDto Address { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            Address = await _addressService.GetAddressByIdAsync(id);
        }
    }
}
