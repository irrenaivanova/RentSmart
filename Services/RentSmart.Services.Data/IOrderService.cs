namespace RentSmart.Services.Data
{
    using System.Threading.Tasks;

    public interface IOrderService
    {
        bool IsActive(int id);

        Task AddNewOrderAsync(int serviceId, string userId);

        Task UsingActiveOrder(string ownerId, string propertyId);
    }
}
