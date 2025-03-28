using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Commands.Refresh
{
    public sealed record AddRefreshTokenCommand(string CurrentRefreshToken) : IRequest<AddRefreshTokenResponse>;

}
