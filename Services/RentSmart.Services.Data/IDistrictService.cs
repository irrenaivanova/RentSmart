namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDistrictService
    {
       Task<IEnumerable<string>> GetAllDistrictsAsync();
    }
}
