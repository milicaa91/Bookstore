using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Queries
{
    public sealed record GetUserDetailsResponse
    {
        public string Username { get; init; }
        public string FullName { get; init; }
        public string Email { get; init; }
        public IEnumerable<string> Roles { get; init; }
    }
}
