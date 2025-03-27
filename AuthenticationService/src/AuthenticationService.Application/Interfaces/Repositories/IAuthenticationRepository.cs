using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Interfaces.Repositories
{
    public interface IAuthenticationRepository: IRepository<IdentityUser, Guid>
    {

    }
}
