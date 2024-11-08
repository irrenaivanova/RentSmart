namespace RentSmart.Services.Data
{
    using RentSmart.Data.Models;

    public interface IPropertyService
    {
        double? AveragePropertyRating(string propertyId);

        bool IsPropertyFree(string propertyId);

        T GetById<T>(string id);
    }
}
