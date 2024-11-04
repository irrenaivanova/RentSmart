namespace RentSmart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RentSmart.Data.Models;

    internal class PropertyTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PropertyTypes.Any())
            {
                return;
            }

            var types = new List<string>() { "Room", "Studio", "1 bedroom", "2 bedroom", "3 bedroom", "3+ bedroom", "Maisonette", "Attic" };

            foreach (var type in types)
            {
                var propertyType = new PropertyType()
                {
                    Name = type,
                };
                await dbContext.PropertyTypes.AddAsync(propertyType);
            }
        }
    }
}
