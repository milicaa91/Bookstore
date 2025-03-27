using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface ITokenGeneratorService
    {
        string GenerateToken();
        string RefreshToken(string token);
    }
}
