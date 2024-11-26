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
    }
}
