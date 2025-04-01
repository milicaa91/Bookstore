using AuthenticationService.Application.Features.Users.Commands.Login;
using AuthenticationService.Application.Features.Users.Commands.Logout;
using AuthenticationService.Application.Features.Users.Commands.Refresh;
using AuthenticationService.Application.Features.Users.Commands.Register;
using AuthenticationService.Application.Features.Users.Commands.UpdateRole;
using AuthenticationService.Application.Features.Users.Queries;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;
using LoginRequest = AuthenticationService.Application.Features.Users.Commands.Login.LoginRequest;

namespace AuthenticationService.API.Controllers
{
    /// <summary>
    /// Controller for managing authentication.
    /// </summary>   
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The user registration request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The ID of the newly registered user.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> RegisterUser([FromBody] AddUserRequest request, CancellationToken cancellationToken)
        {
            var addUserCommand = new AddUserCommand(request.Username, request.Password, request.Email, request.FullName);
            var result = await _sender.Send(addUserCommand, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequest">The login request containing username and password.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The login response containing tokens.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUser(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var loginCommand = new LoginCommand(loginRequest.Username, loginRequest.Password);
            var result = await _sender.Send(loginCommand, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Fetches details of the currently logged-in user.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The details of the logged-in user.</returns>
        [HttpGet("me")]
        public async Task<ActionResult<GetUserDetailsResponse>> FetchLoggedUser(CancellationToken cancellationToken)
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User is not authenticated");

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                return BadRequest("Invalid user ID");

            var query = new GetUserDetailsQuery(userId.ToString());
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates the role of a user.
        /// </summary>
        /// <param name="id">The ID of the user whose role is to be updated.</param>
        /// <param name="request">The request containing the new role.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The number of affected rows.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("roles/{id}")]
        public async Task<ActionResult<int>> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var updateRoleCommand = new UpdateRoleCommand(id, request.Role);
            var result = await _sender.Send(updateRoleCommand, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Generates Access and Refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The new access and refresh tokens.</returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<AddRefreshTokenResponse>> RefreshToken([FromBody] AddRefreshTokenRequest refreshToken, CancellationToken cancellationToken)
        {
            var addRefreshTokenCommand = new AddRefreshTokenCommand(refreshToken.CurrentRefreshToken);
            var result = await _sender.Send(addRefreshTokenCommand, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Logs out the currently logged-in user.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An IActionResult indicating the result of the logout operation.</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User is not authenticated");

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                return BadRequest("Invalid user ID");

            var logoutCommand = new LogoutCommand(userId.ToString());
            var result = await _sender.Send(logoutCommand, cancellationToken);

            return Ok(result);
        }
    }
}
