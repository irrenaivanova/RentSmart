namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties.InputModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser;

    public interface IPropertyService
    {
        Task AddAsync(AddPropertyInputModel input, string userId, string imagePath);

        double AveragePropertyRating(string propertyId);

        bool IsPropertyAvailable(string propertyId);

        Task<string> PropertyCurrentRenterId(string propertyId);

        Task<List<PropertyInListViewModel>> GetAllAvailableAsync<TPropertyInListViewModel>(int page, int propertiesPerPage);

        Task<PropertyDetailsViewModel> GetByIdAsync(string id);

        Task<UserAllPropertiesViewModel> GetByIdAllProperties(string userId, bool isManager, bool isOwner, bool isRenter, int page, int propertiesPerPage);

        int GetCountsAvailable();

        int GetPropertyLikesCount(string propertyId);

        Task DeleteAsync(string propertyId);

        T GetById<T>(string id);

        string GetManagerUserId(string propertyId);
    }
}
