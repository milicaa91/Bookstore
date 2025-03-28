using AuthenticationService.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Commands.Register
{
    public sealed record AddUserCommand(string Username, string Password, string Email, string FullName) : IRequest<Guid>;
}
