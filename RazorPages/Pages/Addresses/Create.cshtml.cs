using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Addresses
{
    [Authorize(Policy = "AdminPolicy")]
    public class CreateModel : PageModel
    {
        private readonly AddressService _addressService;

        public CreateModel(AddressService addressService)
        {
            _addressService = addressService;
        }

        [BindProperty]
        public AddressFullDto Address { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _addressService.AddAddressAsync(Address);

            return RedirectToPage("./Index");
        }
    }
}
