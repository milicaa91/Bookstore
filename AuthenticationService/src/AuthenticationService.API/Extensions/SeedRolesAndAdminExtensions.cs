using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.API.Extensions
{
    public static class SeedRolesAndAdminExtensions
    {
        public static async Task SeedRolesAndAdminAsync(this WebApplication app, IConfiguration configuration)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await SeedRolesAsync(roleManager);

                    await SeedAdminUserAsync(userManager, configuration);

                    logger.LogInformation("Seeding completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding data");
                    throw;
                }
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { nameof(Role.Admin), nameof(Role.Operator), nameof(Role.User) };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var adminEmail = configuration["Admin:Email"] ?? string.Empty;

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = configuration["Admin:UserName"],
                    FullName = configuration["Admin:FullName"],
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, configuration["Admin:Password"]);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, nameof(Role.Admin));
                }
            }
        }
    }
}
