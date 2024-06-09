using Application.Dtos;
using Application.Services;

namespace GraphQL.Queries
{
    public class AccountQuery
    {
        public async Task<IEnumerable<AccountOnlyDto>> GetAccounts([Service] AccountService accountService)
        {
            return await accountService.GetAllAccountsAsync();
        }
        public async Task<AccountFullDto> GetAccountById(Guid id, [Service] AccountService accountService)
        {
            return await accountService.GetAccountByIdAsync(id);
        }
        public async Task<AccountFullDto> GetAccountByUsername(string username, [Service] AccountService accountService)
        {
            return await accountService.GetByUsernameAsync(username);
        }
    }
}
