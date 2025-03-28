using AuthenticationService.Application.Interfaces.Repositories;
using AuthenticationService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Repositories
{
    public class AuthenticationRepository : Repository<ApplicationUser, string>, IAuthenticationRepository
    {
        public AuthenticationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
        {
             return await base.GetByIdAsync(id);
        }
    }
}
