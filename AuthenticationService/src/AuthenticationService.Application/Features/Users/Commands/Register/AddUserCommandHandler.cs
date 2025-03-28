using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using static Common.Exceptions.ServerExceptions;
using AuthenticationService.Domain.Enums;

namespace AuthenticationService.Application.Features.Users.Commands.Register
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null)
                throw new UserAlreadyExistException($"User with e-mail {request.Email} already exists.");

            user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),//TODO maybe "NEWID()"
                UserName = request.Username,
                Email = request.Email,
                FullName = request.FullName
            };

            var createUser = await _userManager.CreateAsync(user, request.Password);

            if (!createUser.Succeeded)
            {
                if (createUser.Errors.Any())
                {
                    var error = createUser.Errors.First();
                    throw new InternalServerException(error.Description);
                }
                throw new InternalServerException($"Registration failed, please try again!");
            }

            var role = await _roleManager.FindByNameAsync(nameof(Role.User));

            if (role is not null)
            {
                var addRole = await _userManager.AddToRoleAsync(user, nameof(Role.User));

                if (!addRole.Succeeded)
                    throw new InternalServerException("Registration failed, please try again.");
            }
            return Guid.Parse(user.Id);
        }

        private void ValidateRequest(AddUserCommand request) //TODO move to a validator
        {
            if (request == null)
                throw new BadRequestException($"Request missing data");
            if (string.IsNullOrEmpty(request.Username))
                throw new BadRequestException($"Username cannot be null or empty");
            if (string.IsNullOrEmpty(request.Password))
                throw new BadRequestException("Password cannot be null or empty"); 
            if (string.IsNullOrEmpty(request.Email))
                throw new BadRequestException("Email cannot be null or empty");
        }
    }
}
