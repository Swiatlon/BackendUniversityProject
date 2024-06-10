using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Policy = "AdminPolicy")]
    public class CreateModel : PageModel
    {
        private readonly AccountService _accountService;

        public CreateModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public AccountFullDto Account { get; set; }

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
            Account.Id = Guid.NewGuid();
            await _accountService.AddAccountAsync(Account);

            return RedirectToPage("./Index");
        }
    }
}
