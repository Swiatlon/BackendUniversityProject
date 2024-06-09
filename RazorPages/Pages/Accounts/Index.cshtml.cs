using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorPages.Pages.Accounts
{
    [Authorize(Roles = "User")]
    public class IndexModel : PageModel
    {
        private readonly AccountService _accountService;

        public IndexModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IList<AccountFullDto> Accounts { get; set; }

        public async Task OnGetAsync()
        {
            Accounts = (IList<AccountFullDto>)await _accountService.GetAllAccountsAsync();
        }
    }
}
