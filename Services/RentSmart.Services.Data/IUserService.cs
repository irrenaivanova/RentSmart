namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Owner;

    public interface IUserService
    {
        Task<IEnumerable<OwnerInputModel>> GetAllOwnerSAsync();

        Task<IEnumerable<RenterInputModel>> GetAllFutureRentersAsync(string propertyId);

        Task<string> GetTheRenterByUserId(string userId);

        Task<bool> IsThisUserPassedRentalOfTheProperty(string userId, string propertyId);

        Task<int> GetRentalId(string userId, string propertyId);

        bool IsManagerOfTheProperty(string userId, string propertyId);
    }
}
