namespace RentSmart.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Data.Models;
    using RentSmart.Services.Data;

    using static RentSmart.Common.NotificationConstants;

    public class OrderController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;

        public OrderController(
            UserManager<ApplicationUser> userManager,
            IOrderService orderService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> Make(int id)
        {
            string userId = this.GetUserId();
            await this.orderService.AddNewOrderAsync(id, userId);
            await this.UpgradeToOwner(userId);
            this.TempData[SuccessMessage] = "Your order is successful!";
            return this.Redirect("/");
        }

        private async Task UpgradeToOwner(string userId)
        {
            // I do not handle exceptions
            var user = await this.userManager.FindByIdAsync(userId);
            var claims = await this.userManager.GetClaimsAsync(user);
            if (!claims.Any(x => x.Type == "IsOwner" && x.Value == "true"))
            {
                await this.userManager.AddClaimAsync(user, new Claim("IsOwner", "true"));
            }
        }
    }
}
