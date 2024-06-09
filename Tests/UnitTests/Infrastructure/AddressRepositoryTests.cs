using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests.UnitTests.Infrastructure
{
    public class AddressRepositoryTests
    {
        private readonly DbContextOptions<CompanyContext> _dbContextOptions;

        public AddressRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CompanyContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAddressAsync_ShouldAddAddress()
        {
            // Arrange
            var context = new CompanyContext(_dbContextOptions);
            var repository = new AddressRepository(context);

            var address = new Address
            {
                Id = Guid.NewGuid(),
                City = "Poznań",
                Street = "Nowa",
                HouseNumber = "10",
                PostalCode = "60-001",
                Country = "Polska"
            };

            // Act
            var addedAddress = await repository.CreateAsync(address);
            var fetchedAddress = await context.Addresses.FindAsync(addedAddress.Id);

            // Assert
            fetchedAddress.Should().NotBeNull();
            fetchedAddress.City.Should().Be("Poznań");
            fetchedAddress.Street.Should().Be("Nowa");
        }

        [Fact]
        public async Task GetAddressByIdAsync_ShouldReturnAddress()
        {
            // Arrange
            var context = new CompanyContext(_dbContextOptions);
            var repository = new AddressRepository(context);

            var address = new Address
            {
                Id = Guid.NewGuid(),
                City = "Poznań",
                Street = "Nowa",
                HouseNumber = "10",
                PostalCode = "60-001",
                Country = "Polska"
            };

            await context.Addresses.AddAsync(address);
            await context.SaveChangesAsync();

            // Act
            var fetchedAddress = await repository.GetByIdAsync(address.Id);

            // Assert
            fetchedAddress.Should().NotBeNull();
            fetchedAddress.City.Should().Be("Poznań");
            fetchedAddress.Street.Should().Be("Nowa");
        }
    }
}
