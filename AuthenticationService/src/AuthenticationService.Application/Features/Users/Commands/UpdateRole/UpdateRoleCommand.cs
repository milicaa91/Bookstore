using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Commands.UpdateRole
{
    public sealed record UpdateRoleCommand(Guid Id, string NewRole) : IRequest<string>;
}
