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

namespace Tests.UnitTests.Application
{
    public class AddressServiceTests
    {
        private readonly AddressService _addressService;
        private readonly Mock<IAddressReposiotry> _addressRepositoryMock = new Mock<IAddressReposiotry>();
        private readonly IMapper _mapper;

        public AddressServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressOnlyDto>().ReverseMap();
                cfg.CreateMap<Address, AddressFullDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();
            _addressService = new AddressService(_addressRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAddressesAsync_ShouldReturnAllAddresses()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new Address { Id = Guid.NewGuid(), City = "Warszawa", Street = "Krakowskie Przedmieście", HouseNumber = "1" },
                new Address { Id = Guid.NewGuid(), City = "Kraków", Street = "Floriańska", HouseNumber = "2" }
            };

            _addressRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(addresses);

            // Act
            var result = await _addressService.GetAllAddressesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(a => a.City == "Warszawa" && a.Street == "Krakowskie Przedmieście");
            result.Should().Contain(a => a.City == "Kraków" && a.Street == "Floriańska");
        }

        [Fact]
        public async Task GetAddressByIdAsync_ShouldReturnAddress()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var address = new Address { Id = addressId, City = "Warszawa", Street = "Krakowskie Przedmieście", HouseNumber = "1" };

            _addressRepositoryMock.Setup(repo => repo.GetByIdAsync(addressId)).ReturnsAsync(address);

            // Act
            var result = await _addressService.GetAddressByIdAsync(addressId);

            // Assert
            result.Should().NotBeNull();
            result.City.Should().Be("Warszawa");
            result.Street.Should().Be("Krakowskie Przedmieście");
        }
    }
}
