using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestApi;
using Xunit;
namespace Tests.IntegrationTests
{
    public class AddressIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AddressIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<string> GetJwtTokenAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("/api/Auth/login", new
            {
                Username = username,
                Password = password
            });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result.Token;
        }

        [Fact]
        public async Task CreateAddress_ShouldAddAddress()
        {
            var token = await GetJwtTokenAsync("admin", "admin");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var newAddress = new AddressFullDto
            {
                City = "Gdańsk",
                Street = "Długa",
                HouseNumber = "10",
                PostalCode = "80-001",
                Country = "Polska"
            };

            var response = await _client.PostAsJsonAsync("/api/Address", newAddress);
            response.EnsureSuccessStatusCode();

            var createdAddress = await response.Content.ReadFromJsonAsync<AddressFullDto>();
            createdAddress.Should().NotBeNull();
            createdAddress.City.Should().Be(newAddress.City);
        }
    }
}