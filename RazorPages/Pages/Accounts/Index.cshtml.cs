using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Policy = "AdminOrUserPolicy")]
    public class IndexModel : PageModel
    {
        private readonly AccountService _accountService;

        public IndexModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IList<AccountOnlyDto> Accounts { get; set; }

        public async Task OnGetAsync()
        {
            Accounts = (IList<AccountOnlyDto>)await _accountService.GetAllAccountsAsync();
        }
    }
}
