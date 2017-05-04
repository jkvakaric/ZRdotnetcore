using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZRdotnetcore.Models;

namespace ZRdotnetcore.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _appContext;
        private readonly YoctoDbContext _yoctoContext;

        public DbInitializer(ApplicationDbContext appContext, YoctoDbContext yoctoContext)
        {
            _appContext = appContext;
            _yoctoContext = yoctoContext;
        }

        public async void Seed()
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@email.com",
                NormalizedUserName = "ADMIN@EMAIL.COM",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(_appContext);

            if (!_appContext.Roles.Any(r => r.Name == "admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
            }

            if (!_appContext.Users.Any(u => u.UserName == admin.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(admin, "admin15");
                admin.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(_appContext);
                await userStore.CreateAsync(admin);
                await userStore.AddToRoleAsync(admin, "admin");

                var realUser = new User
                {
                    UserId = admin.Id,
                    FullName = "Administrator",
                    Username = "admin",
                    Email = admin.Email
                };

                _yoctoContext.Users.Add(realUser);
            }

            await _appContext.SaveChangesAsync();
            await _yoctoContext.SaveChangesAsync();
        }
    }
}