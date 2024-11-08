namespace RentSmart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RentSmart.Data.Models;

    internal class CitySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Cities.Any())
            {
                return;
            }

            var cities = new List<string>() { "Sofia", "Plovdiv", "Varna" };

            foreach (var city in cities)
            {
                var newCity = new City()
                {
                    Name = city,
                };
                await dbContext.Cities.AddAsync(newCity);
            }
        }
    }
}
