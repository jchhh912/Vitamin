
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace Infrastructure.Presistence.Database;


internal class ApplicationDbSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ApplicationDbSeeder> _logger;
    public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<ApplicationDbSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }
    public async Task SeedDatabaseAsync()
    {
        foreach (string roleName in VitaminRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName) is not ApplicationRole role)
            {
                _logger.LogInformation("Seeding {role} Roles.", roleName);
                role = new ApplicationRole(roleName, $"Create {roleName} Role.");
                await _roleManager.CreateAsync(role);
            }

        }
        if (await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==VitaminConstants.default_username) is not ApplicationUser adminUser)
        {
            adminUser = new ApplicationUser()
            {
                UserName = VitaminConstants.default_username,
                Email = VitaminConstants.default_email
            };
            await _userManager.CreateAsync(adminUser, VitaminConstants.defaul_password);
        }
        if (!await _userManager.IsInRoleAsync(adminUser, VitaminRoles.Administrators))
        {
            _logger.LogInformation("Assigning Admin Role to Admin User.");
            await _userManager.AddToRoleAsync(adminUser, VitaminRoles.Administrators);
        }
    }
}
