using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Queries
{
    public sealed record GetUserByEmailQuery(string email, string password) : IRequest<string>;
}
