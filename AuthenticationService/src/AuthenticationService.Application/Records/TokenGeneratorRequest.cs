using AuthenticationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Records
{
    public record TokenGeneratorRequest(string Id, string? Username, string? Email, string Role);
}
