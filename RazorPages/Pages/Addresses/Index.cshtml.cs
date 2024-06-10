using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Addresses
{
    [Authorize(Policy = "AdminOrUserPolicy")]
    public class IndexModel : PageModel
    {
        private readonly AddressService _addressService;

        public IndexModel(AddressService addressService)
        {
            _addressService = addressService;
        }

        public IList<AddressOnlyDto> Addresses { get; set; }

        public async Task OnGetAsync()
        {
            Addresses = (IList<AddressOnlyDto>)await _addressService.GetAllAddressesAsync();
        }
    }
}
