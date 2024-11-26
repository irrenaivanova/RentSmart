namespace RentSmart.Services.Data
{
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        public async Task AddRentAsync(string propertyId, string userId, DateTime rentDate, int durationInMonths)
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
        }
    }
}
