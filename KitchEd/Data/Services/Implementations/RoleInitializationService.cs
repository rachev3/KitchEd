using Microsoft.AspNetCore.Identity;
using KitchEd.Data.Enums;

namespace KitchEd.Data.Services.Implementations
{
    public class RoleInitializationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleInitializationService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task InitializeRoles()
        {
            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                string roleName = role.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
} 