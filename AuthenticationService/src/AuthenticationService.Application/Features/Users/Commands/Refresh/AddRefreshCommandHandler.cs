using AuthenticationService.Application.Features.Users.Commands.Register;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Interfaces.Repositories;
using AuthenticationService.Application.Interfaces.Services.TokenGeneratorService;
using AuthenticationService.Application.Records;
using AuthenticationService.Domain.Entities;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Common.Exceptions.ServerExceptions;

namespace AuthenticationService.Application.Features.Users.Commands.Refresh
{
    public class AddRefreshCommandHandler : IRequestHandler<AddRefreshTokenCommand, AddRefreshTokenResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddRefreshCommandHandler(UserManager<ApplicationUser> userManager,
            ITokenGeneratorService tokenGeneratorService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _tokenGeneratorService = tokenGeneratorService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AddRefreshTokenResponse> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.CurrentRefreshToken);

            if (storedToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (user is null)
                throw new UserNotFoundException($"User doesn't exists.");

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles == null || !userRoles.Any())
                throw new UserNotFoundException($"User role not found.");

            var tokenGeneratorRequest = new TokenGeneratorRequest(user.Id, user.UserName, user.Email, userRoles.First());

            var accessToken = _tokenGeneratorService.GenerateAccessToken(tokenGeneratorRequest);

            if (accessToken is null)
                throw new InternalServerException("Access token generation failed, please try again.");

            var newRefreshToken = _tokenGeneratorService.GenerateRefreshToken();

            if (newRefreshToken is null)
                throw new InternalServerException("Refresh token generation failed, please try again.");

            storedToken.IsRevoked = true;

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            return new AddRefreshTokenResponse { AccessToken = accessToken, RefreshToken = newRefreshToken };
        }
    }
}
