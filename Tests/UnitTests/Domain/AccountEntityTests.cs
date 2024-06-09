using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.UnitTests.Domain
{
    public class AccountEntityTests
    {
        [Fact]
        public void Account_ShouldInitializeProperly()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var username = "admin";
            var email = "admin@example.com";
            var isActive = true;

            // Act
            var account = new Account
            {
                Id = accountId,
                Username = username,
                Email = email,
                IsActive = isActive
            };

            // Assert
            account.Id.Should().Be(accountId);
            account.Username.Should().Be(username);
            account.Email.Should().Be(email);
            account.IsActive.Should().Be(isActive);
        }

        [Fact]
        public void Account_ShouldUpdateProperly()
        {
            // Arrange
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@example.com",
                IsActive = true
            };

            var newUsername = "newAdmin";
            var newEmail = "newAdmin@example.com";

            // Act
            account.Username = newUsername;
            account.Email = newEmail;

            // Assert
            account.Username.Should().Be(newUsername);
            account.Email.Should().Be(newEmail);
        }
    }
}
