using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Domain.Enums;

namespace AuthenticationService.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Email { get; set; }

        [Required]
        [Column("user_name")]
        public required string UserName { get; set; }

        [Required]
        [Column("hashed_password")]
        public required string HashedPassword { get; set; }

        [Required]
        public required string Salt { get; set; }

        [Required]
        public required Role Role { get; set; }
    }
}
