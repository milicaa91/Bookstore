using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Application.Interfaces.Services.TokenGeneratorService;
using AuthenticationService.Application.Records;
using AuthenticationService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenGenerator
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration _configuration;

        public TokenGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateAccessToken(TokenGeneratorRequest tokenGeneratorRequest)
        {
            var tokenHandler = new JsonWebTokenHandler(); //30% faster then others

            var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenGeneratorRequest.Id),
                    new Claim(ClaimTypes.Email, tokenGeneratorRequest.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, tokenGeneratorRequest.Username ?? string.Empty),
                    new Claim(ClaimTypes.Role, tokenGeneratorRequest.Role)
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(double.Parse(_configuration["Jwt:TokenLifeTime"]))),
                Issuer = _configuration["Jwt:Authority"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}