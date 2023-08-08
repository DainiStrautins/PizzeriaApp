using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using PizzeriaApp.Models;
using PizzeriaApp.Models.ValueModels;

namespace PizzeriaApp
{
    public static class InitalizeDefaultUser
    {

        public static async Task DefaultTestUser(IServiceProvider serviceProvider, UserManager<User> user)
        {
            string userName = "test@user.com";
            string name = "Test";
            string lastName = "User";
            string email = "test@user.com";
            string password = "Goexanimo12#";
            if (user.FindByEmailAsync(email).Result == null)
            {
                var adminUser = Activator.CreateInstance<User>();
                adminUser.UserName = userName;
                adminUser.Email = email;
                adminUser.Name = name;
                adminUser.Lastname = lastName;

                var result = await user.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    user.AddToRoleAsync(adminUser, UserRoles.Customer).Wait();
                }
            }
        }
        public static async Task DefaultUserAdmin(IServiceProvider serviceProvider, UserManager<User> user)
        {
            string userName = "strautinsdainis@yandex.com";
            string name = "Dainis";
            string lastName = "Strautins";
            string email = "strautinsdainis@yandex.com";
            string password = "Goexanimo12#";
            if (user.FindByEmailAsync(email).Result == null)
            {
                var adminUser = Activator.CreateInstance<User>();
                adminUser.UserName = userName;
                adminUser.Email = email;
                adminUser.Name = name;
                adminUser.Lastname = lastName;

                var result = await user.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    user.AddToRoleAsync(adminUser, UserRoles.Admin).Wait();
                }
            }
        }
    }
}
