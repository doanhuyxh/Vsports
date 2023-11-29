using Microsoft.AspNetCore.Identity;
using vsports.Models;

namespace vsports.Data
{
    public interface IIdentityDataInitializer
    {
        Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager);
    }

    public class IdentityDataInitializer : IIdentityDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityDataInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // Add roles          
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));

            // Add super admin user
            var superAdminEmail = _configuration["SuperAdminDefaultOption:Email"];
            var superAdminUserName = _configuration["SuperAdminDefaultOption:Username"];
            var superAdminPassword = _configuration["SuperAdminDefaultOption:Password"];
            var superAdminUser = new ApplicationUser
            {
                Email = superAdminEmail,
                UserName = superAdminUserName,
                FullName = superAdminUserName,
                IsActive = true,
                avatarImage = "/upload/img_avatar/blank_avatar.png"
            };
            var User = new ApplicationUser
            {
                Email = "user@gmail.com",
                UserName = "user12345",
                FullName = "user",
                IsActive = true,
                avatarImage = "/upload/img_avatar/blank_avatar.png"
            };
            var result1 = await userManager.CreateAsync(superAdminUser, superAdminPassword);
            var result2 = await userManager.CreateAsync(User, superAdminPassword);


            if (result1.Succeeded && result2.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "Admin");
                await userManager.AddToRoleAsync(User, "User");
            }
        }
    }

}
