using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests.UnitTests.Infrastructure
{
    public class AccountRepositoryTests
    {
        private readonly DbContextOptions<CompanyContext> _dbContextOptions;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CompanyContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _passwordHasher = new PasswordHasher<Account>();
        }

        [Fact]
        public async Task AddAccountAsync_ShouldAddAccount()
        {
            // Arrange
            var context = new CompanyContext(_dbContextOptions);
            var repository = new AccountRepository(context);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                IsActive = true
            };

            // Hash the password before saving the account
            account.Password = _passwordHasher.HashPassword(account, "testpassword");

            // Act
            var addedAccount = await repository.CreateAsync(account);
            var fetchedAccount = await context.Accounts.FindAsync(addedAccount.Id);

            // Assert
            fetchedAccount.Should().NotBeNull();
            fetchedAccount.Username.Should().Be("testuser");
            fetchedAccount.Email.Should().Be("testuser@example.com");
            fetchedAccount.Password.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnAccount()
        {
            // Arrange
            var context = new CompanyContext(_dbContextOptions);
            var repository = new AccountRepository(context);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                IsActive = true
            };

            // Hash the password before saving the account
            account.Password = _passwordHasher.HashPassword(account, "testpassword");

            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            var fetchedAccount = await repository.GetByIdAsync(account.Id);

            // Assert
            fetchedAccount.Should().NotBeNull();
            fetchedAccount.Username.Should().Be("testuser");
            fetchedAccount.Email.Should().Be("testuser@example.com");
        }
    }
}
