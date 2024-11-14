namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties;

    public interface IPropertyService
    {
        Task AddAsync(AddPropertyInputModel input, string userId, string imagePath);

        double AveragePropertyRating(string propertyId);

        bool IsPropertyAvailable(string propertyId);

        T GetById<T>(string id);

        Task<IEnumerable<PropertyInListViewModel>> GetAllAvailableAsync<TPropertyInListViewModel>();
    }
}
