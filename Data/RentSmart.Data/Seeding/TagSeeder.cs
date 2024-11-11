namespace RentSmart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RentSmart.Data.Models;

    internal class TagSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Tags.Any())
            {
                return;
            }

            var tags = new List<string>() { "Furnished", "Not furnished", "With a garage", "Air-condition", "With pets" };
            foreach (var tag in tags)
            {
                var newTag = new Tag() { Name = tag };
                await dbContext.Tags.AddAsync(newTag);
            }
        }
    }
}
