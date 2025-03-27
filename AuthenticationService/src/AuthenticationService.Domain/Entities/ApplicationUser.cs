using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}
