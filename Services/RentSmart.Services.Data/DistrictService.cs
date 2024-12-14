namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    public class DistrictService : IDistrictService
    {
        private readonly IDeletableEntityRepository<District> districtRepository;

        public DistrictService(IDeletableEntityRepository<District> districtRepository)
        {
            this.districtRepository = districtRepository;
        }

        public async Task<IEnumerable<string>> GetAllDistrictsAsync()
        {
            return await this.districtRepository.AllAsNoTracking().Select(x => x.Name).OrderBy(x => x).ToListAsync();
        }
    }
}
