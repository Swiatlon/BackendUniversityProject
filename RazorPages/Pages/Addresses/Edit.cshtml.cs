using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace RazorPages.Pages.Addresses
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly AddressService _addressService;

        public EditModel(AddressService addressService)
        {
            _addressService = addressService;
        }

        [BindProperty]
        public AddressFullDto Address { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Address = await _addressService.GetAddressByIdAsync(id);

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _addressService.UpdateAddressAsync(Address);

            return RedirectToPage("./Index");
        }
    }
}
