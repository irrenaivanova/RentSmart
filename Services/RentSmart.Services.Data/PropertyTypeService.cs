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

    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeService;

        public PropertyTypeService(IDeletableEntityRepository<PropertyType> propertyTypeService)
        {
            this.propertyTypeService = propertyTypeService;
        }

        public async Task<IEnumerable<PropertyTypeInputModel>> AllPropertyTypesAsync()
        {
            return await this.propertyTypeService.AllAsNoTracking().To<PropertyTypeInputModel>().OrderBy(x => x.Name).ToListAsync();
        }
    }
}
