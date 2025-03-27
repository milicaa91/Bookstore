using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;

namespace AuthenticationService.Application.Features.Users.Commands.Register
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AddUserCommandHandler(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IAuthenticationRepository authenticationRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null)
                throw new UserAlreadyExistException($"User with e-mail {request.Email} already exists.");

            user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            return Guid.NewGuid();
        }

        private void ValidateRequest(AddUserCommand request) //TODO move to a validator
        {
            if (string.IsNullOrEmpty(request.Username))
                throw new ArgumentException("Username cannot be null or empty", nameof(request.Username));
            if (string.IsNullOrEmpty(request.Password))
                throw new ArgumentException("Password cannot be null or empty", nameof(request.Password)); 
            if (string.IsNullOrEmpty(request.Email))
                throw new ArgumentException("Email cannot be null or empty", nameof(request.Password));

        }
    }
}
