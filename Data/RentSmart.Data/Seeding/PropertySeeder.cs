namespace RentSmart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using RentSmart.Data.Models;
    using RentSmart.Data.Seeding.Model;



    public class PropertySeeder : ISeeder
    {
        private readonly IWebHostEnvironment environment;

        public PropertySeeder(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            if (dbContext.PropertyTypes.Any())
            {
                return;
            }

            string wwwRootPath = this.environment.WebRootPath;
            string path = Path.Combine(wwwRootPath, "data", "properties.json");
            var jsonContent = await File.ReadAllTextAsync(path);
            var properties = JsonConvert.DeserializeObject<List<PropertyDto>>(jsonContent);

            // seeding only 200 properties
            for (int i = 0; i < 200; i++)
            {
                var propertyDto = properties[i];

                // seeding only properties which have a price
                if (propertyDto.PricePerMonth == 0)
                {
                    continue;
                }

                var property = new Property
                {
                    Floor = propertyDto.Floor,
                    Size = propertyDto.Size,
                    Images = new List<Image>() { new Image { RemoteImageUrl = propertyDto.ImageUrl } },
                    OriginalUrl = propertyDto.OriginalUrl,
                    PricePerMonth = propertyDto.PricePerMonth,
                };
                var propType = await dbContext.PropertyTypes.FirstOrDefaultAsync(x => x.Name == propertyDto.PropertyType);
                if (propType == null)
                {
                    propType = new PropertyType { Name = propertyDto.PropertyType };
                    dbContext.PropertyTypes.Add(propType);
                }

                property.PropertyType = propType;

                var city = await dbContext.Cities.FirstOrDefaultAsync(x => x.Name == propertyDto.City);
                if (city == null)
                {
                    city = new City { Name = propertyDto.City };
                    dbContext.Cities.Add(city);
                }

                property.City = city;

                var district = await dbContext.Districts.FirstOrDefaultAsync(x => x.Name == propertyDto.District);
                if (district == null)
                {
                    district = new District { Name = propertyDto.District };
                    dbContext.Districts.Add(district);
                }

                property.District = district;

                foreach (var tagName in propertyDto.Tags.Distinct())
                {
                    var tag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        dbContext.Tags.Add(tag);
                    }

                    property.Tags.Add(new PropertyTag { Tag = tag, Property = property });
                }

                var manager = await dbContext.Managers.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                var owner = await dbContext.Owners.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

                property.Manager = manager;
                property.Owner = owner;

                await dbContext.Properties.AddAsync(property);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
