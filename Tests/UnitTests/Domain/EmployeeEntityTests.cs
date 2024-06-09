using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Domain
{
    public class EmployeeEntityTests
    {
        [Fact]
        public void Can_Create_Employee()
        {
            // Arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Jan",
                Surname = "Kowalski",
                BirthDate = new DateTime(1990, 1, 1),
                Pesel = "90010112345",
                Gender = Gender.Male
            };

            // Act & Assert
            employee.Name.Should().Be("Jan");
            employee.Surname.Should().Be("Kowalski");
            employee.BirthDate.Should().Be(new DateTime(1990, 1, 1));
        }
    }
}
