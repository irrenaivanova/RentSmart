namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Properties.InputModels;

    public class OwnerService : IOwnerService
    {
        private readonly IDeletableEntityRepository<Owner> ownerRepository;

        public OwnerService(
            IDeletableEntityRepository<Owner> ownerRepository)
        {
            this.ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<OwnerInputModel>> GetAllOwnerSAsync()
        {
            return await this.ownerRepository.AllAsNoTracking()
                .Where(x => x.Orders.Any(x => x.Service.Duration == "One Time" && x.PropertyId == null))
                .To<OwnerInputModel>().OrderBy(x => x.UserEmail).ToListAsync();
        }
    }
}
