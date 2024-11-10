namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Properties;

    public class CityService : ICityService
    {
        private readonly IDeletableEntityRepository<City> cityRepository;

        public CityService(IDeletableEntityRepository<City> cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityInputModel>> GetAllCitiesAsync()
        {
            return await this.cityRepository.AllAsNoTracking().To<CityInputModel>().OrderBy(x => x.Name).ToListAsync();
        }
    }
}
