using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Tests.IntegrationTests
{
    public class AccountIntegrationTests : IClassFixture<WebApplicationFactory<RestApi.Program>>
    {
        private readonly WebApplicationFactory<RestApi.Program> _factory;
        private readonly HttpClient _client;

        public AccountIntegrationTests(WebApplicationFactory<RestApi.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<CompanyContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<CompanyContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<CompanyContext>();

                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        // Seed the database with test data
                        SeedDatabase(db);
                    }
                });
            });

            _client = _factory.CreateClient();
        }

        private void SeedDatabase(CompanyContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var address1Id = Guid.NewGuid();
            var address2Id = Guid.NewGuid();
            var account1Id = Guid.NewGuid();
            var account2Id = Guid.NewGuid();
            var employee1Id = Guid.NewGuid();
            var employee2Id = Guid.NewGuid();

            var account1 = new Account
            {
                Id = account1Id,
                Username = "admin",
                Email = "admin@example.com",
                IsActive = true
            };
            account1.Password = new PasswordHasher<Account>().HashPassword(account1, "admin");

            var account2 = new Account
            {
                Id = account2Id,
                Username = "user",
                Email = "user@example.com",
                IsActive = true
            };
            account2.Password = new PasswordHasher<Account>().HashPassword(account2, "user");

            context.Accounts.AddRange(account1, account2);

            context.Addresses.AddRange(
                new Address
                {
                    Id = address1Id,
                    City = "Warszawa",
                    Street = "Krakowskie Przedmieście",
                    HouseNumber = "1",
                    PostalCode = "00-001",
                    Country = "Polska"
                },
                new Address
                {
                    Id = address2Id,
                    City = "Kraków",
                    Street = "Floriańska",
                    HouseNumber = "2",
                    PostalCode = "31-021",
                    Country = "Polska"
                }
            );

            context.Employees.AddRange(
                new Employee
                {
                    Id = employee1Id,
                    Name = "Jan",
                    Surname = "Kowalski",
                    BirthDate = new DateTime(1980, 5, 15),
                    Pesel = "80051512345",
                    Gender = Domain.Enums.Gender.Male,
                    AddressId = address1Id,
                    AccountId = account1Id
                },
                new Employee
                {
                    Id = employee2Id,
                    Name = "Anna",
                    Surname = "Nowak",
                    BirthDate = new DateTime(1990, 7, 20),
                    Pesel = "90072012345",
                    Gender = Domain.Enums.Gender.Female,
                    AddressId = address2Id,
                    AccountId = account2Id
                }
            );

            context.SaveChanges();
        }


        private async Task<string> GetJwtTokenAsync()
        {
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "admin"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/auth/login", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<JsonElement>(responseString);
            var token = responseJson.GetProperty("token").GetString();

            return token;
        }

        [Fact]
        public async Task CreateAccount_ShouldAddAccount()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accountDto = new AccountFullDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password123",
                IsActive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(accountDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/account", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("newuser");
        }
    }
}
