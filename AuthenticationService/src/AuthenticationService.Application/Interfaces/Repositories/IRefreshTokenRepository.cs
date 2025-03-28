﻿using AuthenticationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task RevokeAsync(string token);
        Task<bool> IsTokenValidAsync(string token);
    }
}
