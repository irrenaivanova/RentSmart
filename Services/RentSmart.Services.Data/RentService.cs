﻿namespace RentSmart.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    public class RentService : IRentService
    {
        private readonly IPropertyService propertyService;
        private readonly IUserService userService;
        private readonly IDeletableEntityRepository<Rental> rentalRepository;

        public RentService(
            IPropertyService propertyService,
            IUserService userService,
            IDeletableEntityRepository<Rental> rentalRepository)
        {
            this.propertyService = propertyService;
            this.userService = userService;
            this.rentalRepository = rentalRepository;
        }

        public async Task AddRatingAsync(int rentalId, int conditionAndMaintenanceRate, int location, int valueForMoney)
        {
            var rental = await this.rentalRepository.All().FirstOrDefaultAsync(x => x.Id == rentalId && x.RatingId == null);
            var newRating = new Rating()
            {
                ConditionAndMaintenanceRate = conditionAndMaintenanceRate,
                Location = location,
                ValueForMoney = valueForMoney,
            };
            if (rental != null)
            {
                rental.Rating = newRating;
                await this.rentalRepository.SaveChangesAsync();
            }
        }

        public async Task<(int RentalId, string RentalContractUrl)> AddRentAsync(string propertyId, string userId, DateTime rentDate, int durationInMonths)
        {
            var renterId = await this.userService.GetTheRenterByUserId(userId);
            var rental = new Rental()
            {
                PropertyId = propertyId,
                RenterId = renterId,
                DurationInMonths = durationInMonths,
                RentDate = rentDate,
            };

            await this.rentalRepository.AddAsync(rental);
            await this.rentalRepository.SaveChangesAsync();
            return (rental.Id, rental.ContractUrl);
        }
    }
}
