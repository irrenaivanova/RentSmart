﻿namespace RentSmart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Owner;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<Owner> ownerRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Renter> renterRepository;
        private readonly IDeletableEntityRepository<Rental> rentalRepository;
        private readonly IDeletableEntityRepository<Manager> managerRepository;

        public UserService(
            IDeletableEntityRepository<Owner> ownerRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Renter> renterRepository,
            IDeletableEntityRepository<Rental> rentalRepository,
            IDeletableEntityRepository<Manager> managerRepository)
        {
            this.ownerRepository = ownerRepository;
            this.userRepository = userRepository;
            this.renterRepository = renterRepository;
            this.rentalRepository = rentalRepository;
            this.managerRepository = managerRepository;
        }

        public bool IsManagerOfTheProperty(string userId, string propertyId)
        {
            return this.managerRepository.AllAsNoTracking().Where(x => x.User.Id == userId && x.Properties.Any(x => x.Id == propertyId)).Any();
        }

        public async Task<IEnumerable<OwnerInputModel>> GetAllOwnerSAsync()
        {
            return await this.ownerRepository.AllAsNoTracking()
                .Where(x => x.Orders.Any(x => x.Service.Duration == "One Time" && x.PropertyId == null))
                .To<OwnerInputModel>().OrderBy(x => x.UserEmail).ToListAsync();
        }

        public async Task<IEnumerable<RenterInputModel>> GetAllFutureRentersAsync(string propertyId)
        {
            var currentTime = DateTime.UtcNow;
            var users = await this.userRepository.AllAsNoTracking()
                .Include(x => x.Appointments)
                .ToListAsync();

            var futureUsers = users
                .Where(x => x.Appointments.Any(x => x.PropertyId == propertyId && x.DateTime < currentTime))
                .Select(x => new RenterInputModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                });

            return futureUsers;
        }

        public async Task<string> GetTheRenterByUserId(string userId)
        {
            var renter = await this.renterRepository.All().Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (renter == null)
            {
                var newRenter = new Renter()
                {
                    UserId = userId,
                };
                await this.renterRepository.AddAsync(newRenter);
                await this.renterRepository.SaveChangesAsync();
                return newRenter.Id;
            }

            return renter.Id;
        }

        public async Task<bool> IsThisUserPassedRentalOfTheProperty(string userId, string propertyId)
        {
            var renter = await this.renterRepository.AllAsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
            string renterId = renter.Id;
            var rental = await this.rentalRepository.AllAsNoTracking().Where(x => x.RenterId == renterId && x.PropertyId == propertyId).FirstOrDefaultAsync();
            if (rental == null)
            {
                return false;
            }

            return rental.RentDate.AddMonths(rental.DurationInMonths) < DateTime.UtcNow;
        }

        public async Task<int> GetRentalId(string userId, string propertyId)
        {
            var renter = await this.renterRepository.AllAsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
            string renterId = renter.Id;
            var rental = await this.rentalRepository.AllAsNoTracking().Where(x => x.RenterId == renterId && x.PropertyId == propertyId).FirstOrDefaultAsync();
            if (rental.RatingId != null)
            {
                throw new Exception("You can rate the property only once!");
            }

            if (rental.RentDate.AddMonths(rental.DurationInMonths) > DateTime.UtcNow)
            {
                throw new Exception("You can rate the property only after the contract has ended!");
            }

            return rental.Id;
        }
    }
}
