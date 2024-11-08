namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties;

	public interface IPropertyTypeService
	{
		public Task<IEnumerable<PropertyTypeInputModel>> AllPropertyTypesAsync();
	}
}
