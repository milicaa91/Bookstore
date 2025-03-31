using AuthenticationService.API.Controllers;
using AuthenticationService.Application.Features.Users.Commands.Register;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<ISender> _mockSender;

        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mockSender = new Mock<ISender>(); // Mock MediatR
            _controller = new AuthenticationController(_mockSender.Object);
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
            var userId = Guid.NewGuid();

            _mockSender
            .Setup(x => x.Send(It.IsAny<IRequest<Guid>>(), It.IsAny<CancellationToken>()))           
            .ReturnsAsync(userId);

            // Act
            var result = await _controller.RegisterUser(request, CancellationToken.None);

            // Assert
            var actualResult = result.Result as OkObjectResult;
            Assert.NotNull(actualResult); // Ensure it is an OkObjectResult
            Assert.Equal(200, actualResult.StatusCode); // Ensure the status code is 200 OK
            Assert.Equal(userId, actualResult.Value); // Ensure the GUID matches the mocked value

            // Verify the mock was called correctly
            _mockSender.Verify(x => x.Send(It.IsAny<AddUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
