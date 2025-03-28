using AuthenticationService.Application.Features.Users.Commands.Login;
using AuthenticationService.Application.Features.Users.Commands.Refresh;
using AuthenticationService.Application.Interfaces.Repositories;
using AuthenticationService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Common.Exceptions;

namespace AuthenticationService.Application.Features.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, string>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId is null)
                throw new UserNotFoundException($"User doesn't exists.");

            var storedToken = await _refreshTokenRepository.GetByUserId(request.UserId);

            if (storedToken == null)
                throw new UnauthorizedAccessException("Failed to revoke token - current token is missing!");

            storedToken.IsRevoked = true;
            await _unitOfWork.SaveChangesAsync();

            return "User is logged out.";
        }
    }
}
