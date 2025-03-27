using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Repositories
{
    public class AuthenticationRepository : Repository<IdentityUser, Guid>
    {
        public AuthenticationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
