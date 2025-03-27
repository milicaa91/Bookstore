using AuthenticationService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.API.Extensions
{
    public static class SeedRolesAndAdminExtensions
    {
        public static async Task SeedRolesAndAdminAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();//TODO MN logger??

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await SeedRolesAsync(roleManager);

                    await SeedAdminUserAsync(userManager);

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
            var roles = new[] { "Admin", "Operator", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "adminuser@gmail.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    FullName = "Admin User",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "str0ngP@ssw0rd!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
