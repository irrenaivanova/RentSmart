namespace RentSmart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRentService
    {
        Task<(int RentalId, string RentalContractUrl)> AddRentAsync(string propertyId, string userId, DateTime rentDate, int durationInMonths);
    }
}
