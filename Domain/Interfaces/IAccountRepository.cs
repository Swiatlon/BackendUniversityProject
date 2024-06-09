using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(Guid id);
        Task<Account> CreateAsync(Account account);
        Task<Account> UpdateAsync(Account account);
        Task DeleteAsync(Guid id);
        Task<Account> GetByUsernameAsync(string username);
    }
}
