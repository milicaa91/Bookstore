using AuthenticationService.Application.Interfaces.Services.TokenGeneratorService;
using AuthenticationService.Infrastructure.Services.TokenGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Avoid redirect to login page
            }).
            AddJwtBearer(x =>
            {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Authority"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOperatorUserPolicy", policy =>
                    policy.RequireRole("Admin", "Operator", "User"));

                options.AddPolicy("AdminOperatorPolicy", policy =>
                    policy.RequireRole("Admin", "Operator"));
            });
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();

            return services;
        }
    }
}