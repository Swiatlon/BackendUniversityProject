using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Infrastructure
{
    public class EmployeeRepositoryTests
    {
        private readonly EmployeeRepository _repository;
        private readonly CompanyContext _context;

        public EmployeeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<CompanyContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CompanyContext(options);
            _repository = new EmployeeRepository(_context);
        }

        [Fact]
        public async Task Can_Add_Employee()
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

            // Act
            await _repository.CreateAsync(employee);
            var result = await _context.Employees.FindAsync(employee.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Jan");
            result.Surname.Should().Be("Kowalski");
        }
    }
}
