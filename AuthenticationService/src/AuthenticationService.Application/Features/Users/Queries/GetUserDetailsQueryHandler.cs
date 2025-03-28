using AuthenticationService.Application.Features.Users.Commands.Login;
using AuthenticationService.Application.Interfaces.Repositories;
using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Queries
{
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsResponse>
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public GetUserDetailsQueryHandler(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }
        public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request is null || request.UserId is null)
                throw new ArgumentNullException($"User details can't be fetched");

            var user = await _authenticationRepository.GetUserByIdAsync(request.UserId);

            if (user is null)
                throw new UserNotFoundException($"User with ID {request.UserId} not found");

            return new GetUserDetailsResponse
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty
            };
        }
    }
}
