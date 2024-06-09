using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Domain
{
    public class AddressEntityTests
    {
        [Fact]
        public void Address_ShouldInitializeProperly()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var city = "Warszawa";
            var street = "Krakowskie Przedmieście";
            var houseNumber = "1";
            var postalCode = "00-001";
            var country = "Polska";

            // Act
            var address = new Address
            {
                Id = addressId,
                City = city,
                Street = street,
                HouseNumber = houseNumber,
                PostalCode = postalCode,
                Country = country
            };

            // Assert
            address.Id.Should().Be(addressId);
            address.City.Should().Be(city);
            address.Street.Should().Be(street);
            address.HouseNumber.Should().Be(houseNumber);
            address.PostalCode.Should().Be(postalCode);
            address.Country.Should().Be(country);
        }

        [Fact]
        public void Address_ShouldUpdateProperly()
        {
            // Arrange
            var address = new Address
            {
                Id = Guid.NewGuid(),
                City = "Warszawa",
                Street = "Krakowskie Przedmieście",
                HouseNumber = "1",
                PostalCode = "00-001",
                Country = "Polska"
            };

            var newCity = "Gdańsk";
            var newStreet = "Długa";

            // Act
            address.City = newCity;
            address.Street = newStreet;

            // Assert
            address.City.Should().Be(newCity);
            address.Street.Should().Be(newStreet);
        }
    }
}
