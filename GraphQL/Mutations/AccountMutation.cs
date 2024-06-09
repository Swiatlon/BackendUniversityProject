using Application.Dtos;
using Application.Services;

namespace GraphQL.Mutations
{
    public class AccountMutation
    {
        public async Task<AccountFullDto> AddAccount(AccountFullDto accountDto, [Service] AccountService accountService)
        {
            return await accountService.AddAccountAsync(accountDto);
        }

        public async Task<AccountFullDto> UpdateAccount(AccountFullDto accountDto, [Service] AccountService accountService)
        {
            return await accountService.UpdateAccountAsync(accountDto);
        }

        public async Task<bool> DeleteAccount(Guid id, [Service] AccountService accountService)
        {
            await accountService.DeleteAccountAsync(id);
            return true;
        }
    }
}
