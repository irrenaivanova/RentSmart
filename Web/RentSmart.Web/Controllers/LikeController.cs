namespace RentSmart.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Likes;

    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : BaseController
    {
        private readonly ILikeService likeService;
        private readonly IPropertyService propertyService;

        public LikeController(
            ILikeService likeService,
            IPropertyService propertyService)
        {
            this.likeService = likeService;
            this.propertyService = propertyService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostLikeResponseModel>> Post(PostLikeInputModel input)
        {
            var userId = this.GetUserId();
            await this.likeService.SetLikeAsync(input.PropertyId,userId, input.IsLiked);
            var likes = this.propertyService.GetPropertyLikesCount(input.PropertyId);
            return new PostLikeResponseModel { TotalLikes = likes };
        }
    }
}
