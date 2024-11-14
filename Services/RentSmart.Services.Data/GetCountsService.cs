namespace RentSmart.Services.Data
{
    using System.Linq;

    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Web.ViewModels;

    public class GetCountsService : IGetCountsService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Owner> ownerRepository;
        private readonly IDeletableEntityRepository<Manager> managerRepository;
        private readonly IDeletableEntityRepository<Rental> rentalRepository;
        private readonly IPropertyService propertyService;

        public GetCountsService(
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Owner> ownerRepository,
            IDeletableEntityRepository<Manager> managerRepository,
            IDeletableEntityRepository<Rental> rentalRepository,
            IPropertyService propertyService)
        {
            this.propertyRepository = propertyRepository;
            this.ownerRepository = ownerRepository;
            this.managerRepository = managerRepository;
            this.rentalRepository = rentalRepository;
            this.propertyService = propertyService;
        }

        public CountsViewModel GetCountsViewModel()
        {
            return new CountsViewModel()
            {
                ManagersCount = this.managerRepository.AllAsNoTracking().Count(),
                OwnersCount = this.ownerRepository.AllAsNoTracking().Count(),
                PropertiesCount = this.propertyRepository.AllAsNoTracking().Select(x => this.propertyService.IsPropertyAvailable(x.Id)).Count(),
                RentalsCount = this.rentalRepository.AllAsNoTracking().Count(),
            };
        }
    }
}
