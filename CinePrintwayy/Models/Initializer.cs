using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinePrintwayy.Models
{
    public class DbInitializer
    {
        public static void Initialize(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@admin.com.br").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@admin.com.br",
                    Email = "admin@admin.com.br"
                };

                IdentityResult result = userManager.CreateAsync(user, "Senha123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
