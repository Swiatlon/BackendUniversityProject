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
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _employeeService;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        private readonly IMapper _mapper;

        public EmployeeServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeOnlyDto>().ReverseMap();
                cfg.CreateMap<Employee, EmployeeFullDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = Guid.NewGuid(), Name = "Jan", Surname = "Kowalski" },
                new Employee { Id = Guid.NewGuid(), Name = "Anna", Surname = "Nowak" }
            };

            _employeeRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Name == "Jan" && e.Surname == "Kowalski");
            result.Should().Contain(e => e.Name == "Anna" && e.Surname == "Nowak");
        }
    }
}
