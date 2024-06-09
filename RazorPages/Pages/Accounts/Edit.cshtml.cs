using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly AccountService _accountService;

        public EditModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public AccountFullDto Account { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Account = await _accountService.GetAccountByIdAsync(id);

            if (Account == null)
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

            await _accountService.UpdateAccountAsync(Account);

            return RedirectToPage("./Index");
        }
    }
}
