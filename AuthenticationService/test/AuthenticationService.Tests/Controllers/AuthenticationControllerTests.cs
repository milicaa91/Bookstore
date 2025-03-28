using AuthenticationService.API.Controllers;
using AuthenticationService.Application.Features.Users.Commands.Register;
using MediatR;
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
        private readonly Mock<IConfiguration> _mockConfig;

        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mockSender = new Mock<ISender>(); // Mock MediatR
            _mockConfig = new Mock<IConfiguration>(); // Initialize _mockConfig
            _controller = new AuthenticationController(_mockConfig.Object, _mockSender.Object);
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

            //_mockSender.Setup(x => x.Send(It.IsAny<AddUserCommand>(), It.IsAny<CancellationToken>())).Returns(1);

            var result = await _controller.RegisterUser(request, CancellationToken.None);

            //var okResult = result.Result as OkObjectResult;
            //okResult.Should().NotBeNull();
            //okResult!.StatusCode.Should().Be(200);
            //okResult.Value.Should().Be(1);
            //return Ok(result);
        }
    }
}
