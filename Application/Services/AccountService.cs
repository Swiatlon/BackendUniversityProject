using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IPasswordHasher<Account> passwordHasher)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<AccountOnlyDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountOnlyDto>>(accounts);
        }

        public async Task<AccountFullDto> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is not null)
            {
                return _mapper.Map<AccountFullDto>(account);
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }

        public async Task<AccountFullDto> AddAccountAsync(AccountFullDto accountDto)
        {
            var accountEntity = _mapper.Map<Account>(accountDto);
            accountEntity.Password = _passwordHasher.HashPassword(accountEntity, accountDto.Password);
            var account = await _accountRepository.CreateAsync(accountEntity);
            return _mapper.Map<AccountFullDto>(account);
        }

        public async Task<AccountFullDto> UpdateAccountAsync(AccountFullDto accountDto)
        {
            var accountEntity = await _accountRepository.GetByIdAsync(accountDto.Id);
            if (accountEntity is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            _mapper.Map(accountDto, accountEntity);

            if (!string.IsNullOrWhiteSpace(accountDto.Password))
            {
                accountEntity.Password = _passwordHasher.HashPassword(accountEntity, accountDto.Password);
            }

            await _accountRepository.UpdateAsync(accountEntity);
            return _mapper.Map<AccountFullDto>(accountEntity);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            await _accountRepository.DeleteAsync(id);
        }

        public async Task<AccountFullDto> GetByUsernameAsync(string username)
        {
            var account = await _accountRepository.GetByUsernameAsync(username);
            if(account is null)
                throw new KeyNotFoundException("Account not found");
            return _mapper.Map<AccountFullDto>(account);
        }

        public async Task<Account> ValidateUserAsync(string username, string password)
        {
            // Fetch the user
            var user = await _accountRepository.GetByUsernameAsync(username);

            if (user is not null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            throw new Exception("Invalid login attemp");
        }
    }
}
