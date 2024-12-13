namespace RentSmart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using RentSmart.Data.Models;

    using static RentSmart.Common.GlobalConstants;

    internal class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            //if (dbContext.Users.Any())
            //{
            //    return;
            //}

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string adminEmail = "admin@rentsmart.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Email = adminEmail,
                    UserName = adminEmail,
                };
                var result = await userManager.CreateAsync(adminUser, adminUser.Email);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, AdministratorRoleName);
                }
            }

            var random = new Random();
            var names = new List<string>()
            {
                "Dimitar", "Georgi", "Ivan", "Nikola", "Stoyan", "Petar", "Hristo", "Vasil", "Todor", "Kaloyan",
            };

            var familyNames = new List<string>
            {
                "Dimitrov", "Georgiev", "Ivanov", "Nikolov", "Stoyanov", "Petrov", "Hristov", "Vasilev", "Todorov", "Kaloyanov",
            };

            // Seed 3 Managers
            for (int i = 1; i <= 3; i++)
            {
                var firstName = names[random.Next(names.Count)];
                var lastName = familyNames[random.Next(familyNames.Count)];
                var managerUser = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"manager{i}@rentsmart.com",
                    UserName = $"manager{i}@rentsmart.com",
                };
                var password = managerUser.Email;
                var userResult = await userManager.CreateAsync(managerUser, password);
                if (userResult.Succeeded)
                {
                    var manager = new Manager { UserId = managerUser.Id };
                    dbContext.Managers.Add(manager);
                }
            }

            // Seed 10 Owners
            for (int i = 1; i <= 10; i++)
            {
                var firstName = names[random.Next(names.Count)];
                var lastName = familyNames[random.Next(familyNames.Count)];
                var ownerUser = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"owner{i}@example.com",
                    UserName = $"owner{i}@example.com",
                };
                var password = ownerUser.Email;
                var userResult = await userManager.CreateAsync(ownerUser, password);
                if (userResult.Succeeded)
                {
                    var owner = new Owner { UserId = ownerUser.Id };
                    dbContext.Owners.Add(owner);
                }
            }

            // Seed 10 Renters
            for (int i = 1; i <= 10; i++)
            {
                var firstName = names[random.Next(names.Count)];
                var lastName = familyNames[random.Next(familyNames.Count)];
                var renterUser = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"renter{i}@example.com",
                    UserName = $"renter{i}@example.com",
                };
                var password = renterUser.Email;
                var userResult = await userManager.CreateAsync(renterUser, password);
                if (userResult.Succeeded)
                {
                    var renter = new Renter { UserId = renterUser.Id };
                    dbContext.Renters.Add(renter);
                }
            }

            // Seed user with my own email for testing purposes
            var userMe = new ApplicationUser
            {
                FirstName = "Irena",
                LastName = "Ivanova",
                Email = "irrenka_@abv.bg",
                UserName = "irrenka_@abv.bg",
            };
            await userManager.CreateAsync(userMe, userMe.Email);
        }
    }
}
