using AuthenticationService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Commands.Login
{
    public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
}
