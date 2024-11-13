namespace RentSmart.Services.Data
{
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties;

    public interface IPropertyService
    {
        Task AddAsync(AddPropertyInputModel input, string userId, string imagePath);

        double? AveragePropertyRating(string propertyId);

        bool IsPropertyFree(string propertyId);

        T GetById<T>(string id);
    }
}
