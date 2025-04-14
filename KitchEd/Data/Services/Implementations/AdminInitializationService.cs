using Microsoft.AspNetCore.Identity;
using KitchEd.Models.Entities;
using KitchEd.Data.Enums;

namespace KitchEd.Data.Services.Implementations
{
    public class AdminInitializationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminInitializationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAdmin()
        {
            var adminUser = await _userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@kitched.com",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    ShortBio = "System Administrator"
                };

                var result = await _userManager.CreateAsync(admin, "Admin123?");

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
                    }

                    await _userManager.AddToRoleAsync(admin, UserRoles.Admin.ToString());
                }
                else
                {
                    var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user. Errors:\n{errors}");
                }
            }
        }
    }
} 