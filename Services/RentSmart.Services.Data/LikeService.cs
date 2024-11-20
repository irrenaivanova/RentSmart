namespace RentSmart.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    public class LikeService : ILikeService
    {
        private readonly IRepository<UserLike> userLikeRepository;

        public LikeService(IRepository<UserLike> userLikeRepository)
        {
            this.userLikeRepository = userLikeRepository;
        }

        public async Task SetLikeAsync(string propertyId, string userId, bool isLiked)
        {
            if (isLiked)
            {
                if (!this.userLikeRepository.All().Any(x => x.PropertyId == propertyId && x.UserId == userId))
                {
                    await this.userLikeRepository.AddAsync(new UserLike { PropertyId = propertyId, UserId = userId });
                    await this.userLikeRepository.SaveChangesAsync();
                }
            }
            else
            {
                var userLike = await this.userLikeRepository.All().FirstOrDefaultAsync(x => x.PropertyId == propertyId && x.UserId == userId);
                if (userLike != null)
                {
                    this.userLikeRepository.Delete(userLike);
                    await this.userLikeRepository.SaveChangesAsync();
                }
            }
        }
    }
}
