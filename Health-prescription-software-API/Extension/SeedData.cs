using Microsoft.AspNetCore.Identity;

public class SeedData
{
    private readonly IServiceProvider _serviceProvider;

    public SeedData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Initialize()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRoleIfNotExists(roleManager, "GP");
            await CreateRoleIfNotExists(roleManager, "Patient");
            await CreateRoleIfNotExists(roleManager, "Pharmacist");
            await CreateRoleIfNotExists(roleManager, "Pharmacy");
        }
    }

    private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
