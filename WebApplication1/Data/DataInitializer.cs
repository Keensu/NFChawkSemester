using Microsoft.AspNetCore.Identity;
using NFChawk.Models.Entities;

namespace NFChawk.Data
{
    public static class DataInitializer
    {
        public static async Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new Role { Name = adminRole, NormalizedName = adminRole.ToUpper() });
            }

            string? adminEmail = configuration["AdminSettings:Email"];
            string? adminPassword = configuration["AdminSettings:Password"];

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new Exception("Admin email or password is not configured.");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Balance = 0,
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                var roleResult = await userManager.AddToRoleAsync(adminUser, adminRole);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Failed to assign admin role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

            }
        }
    }
}
