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
        private readonly IDeletableEntityRepository<City> cityService;

        public CityService(IDeletableEntityRepository<City> cityService)
        {
            this.cityService = cityService;
        }

        public IEnumerable<CityInputModel> GetAllCities()
        {
            return this.cityService.AllAsNoTracking().To<CityInputModel>().OrderBy(x => x.Name).ToList();
        }
    }
}
