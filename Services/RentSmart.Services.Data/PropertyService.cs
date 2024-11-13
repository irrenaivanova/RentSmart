namespace RentSmart.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Properties;

    public class PropertyService : IPropertyService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<District> districtRepository;
        private readonly IDeletableEntityRepository<Tag> tagRepository;
        private readonly IDeletableEntityRepository<Manager> managerRepository;

        public PropertyService(
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<District> districtRepository,
            IDeletableEntityRepository<Tag> tagRepository,
            IDeletableEntityRepository<Manager> managerRepository)
        {
            this.propertyRepository = propertyRepository;
            this.districtRepository = districtRepository;
            this.tagRepository = tagRepository;
            this.managerRepository = managerRepository;
        }

        public async Task AddAsync(AddPropertyInputModel input, string userId, string imagePath)
        {
            var property = new Property
            {
                Name = input.Name,
                Description = input.Description,
                Floor = input.Floor,
                Size = input.Size,
                PropertyTypeId = input.PropertyTypeId,
                CityId = input.CityId,
                OwnerId = input.OwnerId,
                PricePerMonth = input.PricePerMonth,
            };

            var manager = this.managerRepository.All().FirstOrDefault(x => x.UserId == userId);
            property.Manager = manager;

            var district = this.districtRepository.All().FirstOrDefault(x => x.Name == input.DistrictName);
            if (district == null)
            {
                district = new District { Name = input.DistrictName };
            }

            property.District = district;
            foreach (var tagInput in input.TagIds)
            {
                var tag = this.tagRepository.All().FirstOrDefault(x => x.Id == tagInput);
                property.Tags.Add(new PropertyTag { Tag = tag, Property = property });
            }

            foreach (var tagName in input.CustomTags)
            {
                var tag = new Tag { Name = tagName };
                property.Tags.Add(new PropertyTag { Tag = tag, Property = property });
            }

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public double? AveragePropertyRating(string propertyId)
        {
            var property = this.propertyRepository.AllAsNoTracking()
                .Include(x => x.Rentals)
                .ThenInclude(x => x.Rating)
                .Where(x => x.Id == propertyId)
                .FirstOrDefault();
            if (property.Rentals.Count == 0)
            {
                return null;
            }

            var ratings = property.Rentals.Select(x => x.Rating?.AverageRating).Where(x => x.HasValue).ToList();
            return ratings.Count > 0 ? ratings.Average() : null;
        }

        public bool IsPropertyFree(string propertyId)
        {
            var property = this.propertyRepository.AllAsNoTracking()
                .Include(x => x.Rentals)
                .Where(x => x.Id == propertyId)
                .FirstOrDefault();
            if (property.Rentals.Count == 0)
            {
                return true;
            }

            var lastRent = property.Rentals.OrderByDescending(x => x.RentDate).FirstOrDefault();
            return lastRent!.RentDate.AddMonths(lastRent.DurationInMonths) < DateTime.UtcNow;
        }

        public T GetById<T>(string id)
        {
            return this.propertyRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
