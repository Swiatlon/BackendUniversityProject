using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorPages.Pages.Addresses
{
    [Authorize(Roles = "User")]
    public class IndexModel : PageModel
    {
        private readonly AddressService _addressService;

        public IndexModel(AddressService addressService)
        {
            _addressService = addressService;
        }

        public IList<AddressFullDto> Addresses { get; set; }

        public async Task OnGetAsync()
        {
            Addresses = (IList<AddressFullDto>)await _addressService.GetAllAddressesAsync();
        }
    }
}
