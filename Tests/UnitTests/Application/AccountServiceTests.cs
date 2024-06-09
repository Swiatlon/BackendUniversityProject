using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using AutoMapper;
using Application.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Tests.UnitTests.Application
{
    public class AccountServiceTests
    {
        private readonly AccountService _accountService;
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new Mock<IAccountRepository>();
        private readonly IMapper _mapper;
        private readonly Mock<IPasswordHasher<Account>> _passwordHasherMock = new Mock<IPasswordHasher<Account>>();

        public AccountServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Account, AccountOnlyDto>().ReverseMap();
                cfg.CreateMap<Account, AccountFullDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();
            _accountService = new AccountService(_accountRepositoryMock.Object, _mapper, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ShouldReturnAllAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = Guid.NewGuid(), Username = "admin", Email = "admin@example.com" },
                new Account { Id = Guid.NewGuid(), Username = "user", Email = "user@example.com" }
            };

            _accountRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(a => a.Username == "admin" && a.Email == "admin@example.com");
            result.Should().Contain(a => a.Username == "user" && a.Email == "user@example.com");
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnAccount()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var account = new Account { Id = accountId, Username = "admin", Email = "admin@example.com" };

            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetAccountByIdAsync(accountId);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("admin");
            result.Email.Should().Be("admin@example.com");
        }
    }
}
