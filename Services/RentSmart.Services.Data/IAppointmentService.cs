namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppointmentService
    {
        Task<List<string>> GetAvailableHoursAsync(string propertyId, string date);

        Task CreateAppointmentAsync(string propertyId, string date, string time, string userId);
    }
}
