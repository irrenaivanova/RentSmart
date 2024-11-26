namespace RentSmart.Services.Data
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

        public UserService(
            IDeletableEntityRepository<Owner> ownerRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.ownerRepository = ownerRepository;
            this.userRepository = userRepository;
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
    }
}
