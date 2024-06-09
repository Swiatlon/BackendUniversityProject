using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace RazorPages.Pages.Addresses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly AddressService _addressService;

        public DeleteModel(AddressService addressService)
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _addressService.DeleteAddressAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
