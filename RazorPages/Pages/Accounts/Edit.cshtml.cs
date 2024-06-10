using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Policy = "AdminOrUserPolicy")]
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
            try
            {
                Account = await _accountService.GetAccountByIdAsync(id);
                if (Account == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Account.IsActive && !Account.DeactivationDate.HasValue)
            {
                ModelState.AddModelError("Account.DeactivationDate", "Deactivation Date is required when account is inactive.");
                return Page();
            }

            try
            {
                await _accountService.UpdateAccountAsync(Account);
                return RedirectToPage("./Index");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
