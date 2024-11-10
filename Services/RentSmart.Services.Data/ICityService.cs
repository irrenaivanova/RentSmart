namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties;

    public interface ICityService
    {
        public Task<IEnumerable<CityInputModel>> GetAllCitiesAsync();
    }
}
