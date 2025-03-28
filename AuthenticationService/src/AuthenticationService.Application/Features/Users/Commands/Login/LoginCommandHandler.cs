using AuthenticationService.Application.Features.Users.Commands.Refresh;
using AuthenticationService.Application.Features.Users.Commands.Register;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Interfaces.Repositories;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Application.Interfaces.Services.TokenGeneratorService;
using AuthenticationService.Application.Records;
using AuthenticationService.Domain.Entities;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Exceptions.ServerExceptions;

namespace AuthenticationService.Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(ITokenGeneratorService tokenGeneratorService,
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _tokenGeneratorService = tokenGeneratorService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null)
                throw new UserNotFoundException($"User with username {request.Username} doesn't exists.");

           var isPasswordMatched = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordMatched)
                throw new UserNotFoundException($"Invalid password.");

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles == null || !userRoles.Any())
                throw new UserNotFoundException($"User role not found.");

            var tokenGeneratorRequest = new TokenGeneratorRequest(user.Id, user.UserName, user.Email, userRoles.First());

            var accessToken = _tokenGeneratorService.GenerateAccessToken(tokenGeneratorRequest);

            if (accessToken is null)
                throw new InternalServerException("Access token generation failed, please try again.");

            var refreshToken = _tokenGeneratorService.GenerateRefreshToken();

            if (refreshToken is null)
                throw new InternalServerException("Refresh token generation failed, please try again.");

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };

        }

        private void ValidateRequest(LoginCommand request) //TODO move to a validator
        {
            if (request == null)
                throw new BadRequestException($"Request missing data");
            if (string.IsNullOrEmpty(request.Username))
                throw new BadRequestException($"Username cannot be null or empty");
            if (string.IsNullOrEmpty(request.Password))
                throw new BadRequestException("Password cannot be null or empty");
        }
    }
}
