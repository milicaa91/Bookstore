using AuthenticationService.Application.Records;
using AuthenticationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Interfaces.Services.TokenGeneratorService
{
    public interface ITokenGeneratorService
    {
        string GenerateAccessToken(TokenGeneratorRequest tokenGeneratorRequest);
        string GenerateRefreshToken();
    }
}
