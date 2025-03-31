using AuthenticationService.Application.Features.Users.Commands.Register;
using AuthenticationService.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Tests.Controllers
{
    public class AuthenticationControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthenticationControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test"); // Use the "Test" environment
       
            }).CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnOk_WithUserId()
        {
            var request = new AddUserRequest
            {
                Username = "testuser",
                Password = "SecurePass123!",
                Email = "test@example.com",
                FullName = "Test User"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
            response.EnsureSuccessStatusCode();

            var userId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, userId);
        }            

        [Fact]
        public async Task RegisterUser_ShouldReturn_BadRequest_WhenUserAlreadyExists()
        {
            var request = new AddUserRequest
            {
                Username = "testuser1",
                Password = "SecurePass123!",
                Email = "test1@example.com",
                FullName = "Test User 1"
            };
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);

            var secondResponse = await _client.PostAsJsonAsync("/api/auth/register", request);

            Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        }
    }
}
