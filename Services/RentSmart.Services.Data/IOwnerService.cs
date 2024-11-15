namespace RentSmart.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties.InputModels;

    public interface IOwnerService
    {
        public Task<IEnumerable<OwnerInputModel>> GetAllOwnerSAsync();
    }
}
