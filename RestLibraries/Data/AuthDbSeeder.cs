using Microsoft.AspNetCore.Identity;
using RestLibraries.Auth;

namespace RestLibraries.Data
{
    public class AuthDbSeeder
    {
        public readonly UserManager<LibrariesUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;


        public AuthDbSeeder(UserManager<LibrariesUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }
        private async Task AddAdminUser()
        {
            var newAdminUser = new LibrariesUser()
            {
                UserName = "admin",
                Email = "admin@ad.com"
            };

            var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
            if(existingAdminUser == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "!Admin.2022");
                if (createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser, LibrariesRoles.All);
                }
            }
        }
        private async Task AddDefaultRoles()
        {
            foreach(var role in LibrariesRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
