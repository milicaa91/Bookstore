using AuthenticationService.Application.Features.Users.Commands.Register;
using AuthenticationService.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISender _sender;

        public AuthenticationController(IConfiguration configuration, ISender sender)
        {
            _configuration = configuration;
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<int>> RegisterUser([FromBody] AddUserRequest request, CancellationToken cancellationToken)
        {
            var addUserCommand = new AddUserCommand(request.Username, request.Password, request.Email);
            var result = await _sender.Send(addUserCommand, cancellationToken);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<int>> LoginUser(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            //TODO rename
            var getUserByEmailQuery = new GetUserByEmailQuery(loginRequest.Email, loginRequest.Password);
            var result = await _sender.Send(getUserByEmailQuery, cancellationToken);

            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<ActionResult<int>> FetchLoggedUser(CancellationToken cancellationToken)
        {
            //TODO get user from token

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("roles/{id}")]
        public async Task<ActionResult<int>> UpdateRole([FromQuery]string id, CancellationToken cancellationToken)
        {
            //get user from token.
            return Ok();
        }
    }
}
