namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties;

    public class OwnerService : IOwnerService
    {
        public Task<IEnumerable<OwnerInputModel>> GetAllOwnerSAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
