using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Roles="Admin")]
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

            await _accountService.AddAccountAsync(Account);

            return RedirectToPage("./Index");
        }
    }
}
