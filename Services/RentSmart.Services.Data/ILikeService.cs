namespace RentSmart.Services.Data
{
    using System.Threading.Tasks;

    public interface ILikeService
    {
        Task SetLikeAsync(string propertyId, string userId, bool isLiked);
    }
}
