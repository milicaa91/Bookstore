using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Interfaces.Repositories;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefreshToken = AuthenticationService.Domain.Entities.RefreshToken;

namespace AuthenticationService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken, string>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
        }

        public async Task<RefreshToken> GetByTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiryDate > DateTime.UtcNow && !rt.IsRevoked);
            
            return storedToken;
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);

            if (refreshToken is null) 
                return;

            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);
            return refreshToken != null && !refreshToken.IsRevoked && refreshToken.ExpiryDate > DateTime.UtcNow;
        }
    }
}
