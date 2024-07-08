using Microsoft.AspNetCore.Identity;

public static class DbInitializer
{
    public static async Task SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Officer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Officer"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    private static async Task SeedUsers(UserManager<IdentityUser> userManager)
    {
        if (userManager.Users.All(u => u.Email != "admin@prizma.com"))
        {
            var admin = new IdentityUser
            {
                UserName = "admin@prizma.com",
                Email = "admin@prizma.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin.1234");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (userManager.Users.All(u => u.Email != "officer@prizma.com"))
        {
            var user = new IdentityUser
            {
                UserName = "officer@prizma.com",
                Email = "officer@prizma.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, "Officer.1234");

            await userManager.AddToRoleAsync(user, "Officer");
        }
    }
}
