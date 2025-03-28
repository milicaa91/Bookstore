using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Domain.Entities;

namespace AuthenticationService.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.HasOne(rt => rt.User)
                      .WithMany() 
                      .HasForeignKey(rt => rt.UserId)
                      .IsRequired();
                entity.Property(rt => rt.Token).IsRequired().HasMaxLength(256);
                entity.Property(rt => rt.ExpiryDate).IsRequired();
                entity.Property(rt => rt.IsRevoked).IsRequired();
            });
        }
    }
}
