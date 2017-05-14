using System;
using System.Collections.Generic;
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

            if (!_yoctoContext.DeviceTypes.Any())
            {
                var types = new List<DeviceType>
                {
                    new DeviceType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "RPi3",
                        Description = "Raspberry Pi 3"
                    }
                };
                _yoctoContext.DeviceTypes.AddRange(types);
            }

            if (!_yoctoContext.ReadingTypes.Any())
            {
                var types = new List<ReadingType>
                {
                    new ReadingType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Temperature OneWire",
                        Description = "Temperature sensor reading using OneWire protocol."
                    }
                };
                _yoctoContext.ReadingTypes.AddRange(types);
            }

            await _appContext.SaveChangesAsync();
            await _yoctoContext.SaveChangesAsync();
        }
    }
}