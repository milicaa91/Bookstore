using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Users.Commands.UpdateRole
{
    public class UpdateRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }
}
