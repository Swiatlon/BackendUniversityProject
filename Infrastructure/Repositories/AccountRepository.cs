using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CompanyContext _context;

        public AccountRepository(CompanyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }
        public async Task<Account> GetByIdAsync(Guid id)
        {
            var account = await _context.Accounts
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account is null) throw new KeyNotFoundException("Account not found");
            return account;
        }
        public async Task<Account> CreateAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<Account> UpdateAsync(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task DeleteAsync(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is not null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var account = await _context.Accounts
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Username == username);

            if (account is null) throw new KeyNotFoundException("Account not found");
            return account;
        }
    }
}
