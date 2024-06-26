using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Policy = "AdminOrUserPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly AccountService _accountService;

        public DetailsModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        public AccountFullDto Account { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            Account = await _accountService.GetAccountByIdAsync(id);
        }
    }
}
