﻿using AuthenticationService.Application.Features.Users.Commands.Refresh;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Commands.Logout
{
    public sealed record LogoutCommand(string UserId) : IRequest<string>;
}
