using AuthenticationService.Application.Features.Users.Commands.Register;
using AuthenticationService.Domain.Entities;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Exceptions.ServerExceptions;

namespace AuthenticationService.Application.Features.Users.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpdateRoleCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null)
                throw new UserNotFoundException($"User with ID {request.Id} not found");

            var role = await _roleManager.FindByNameAsync(request.NewRole);

            if (role is null)
                throw new RoleNotFoundException($"Role {request.NewRole} not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, request.NewRole);

            if (!result.Succeeded)
                throw new InternalServerException("Role update failed");

            return $"User with id: {request.Id} is now a {request.NewRole}";
        }
    }
}
