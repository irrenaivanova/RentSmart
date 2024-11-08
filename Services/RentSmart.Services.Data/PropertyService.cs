namespace RentSmart.Services.Data
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class PropertyService : IPropertyService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;

        public PropertyService(
            IDeletableEntityRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
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
