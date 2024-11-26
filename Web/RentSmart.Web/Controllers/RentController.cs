namespace RentSmart.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Data.Models;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Properties.InputModels;
    using RentSmart.Web.ViewModels.RentContract;
    using static RentSmart.Common.GlobalConstants;
    using static RentSmart.Common.NotificationConstants;

    [Authorize]
    public class RentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPropertyService propertyService;
        private readonly IUserService userService;

        public RentController(
            UserManager<ApplicationUser> userManager,
            IPropertyService propertyService,
            IUserService userService)
        {
            this.userManager = userManager;
            this.propertyService = propertyService;
            this.userService = userService;
        }

        public IActionResult Contract()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Make(string id)
        {
            var propertyManagerUserId = this.propertyService.GetManagerUserId(id);
            var userId = this.GetUserId();

            if (userId != propertyManagerUserId)
            {
                this.TempData[ErrorMessage] = "Only managers of the property can make rentals!";
                return this.RedirectToAction("MyProperties", "Property");
            }

            MakeRentInputModel propertyRent = this.propertyService.GetById<MakeRentInputModel>(id);
            await this.PopulateInputModelWithOwnersAsync(propertyRent);
            return this.View(propertyRent);
        }



        private async Task UpgradeToRenter(string userId)
        {
            // I do not handle exceptions
            var user = await this.userManager.FindByIdAsync(userId);
            var claims = await this.userManager.GetClaimsAsync(user);
            if (claims.Any(x => x.Type == RenterClaim && x.Value == "true"))
            {
                await this.userManager.AddClaimAsync(user, new Claim(RenterClaim, "true"));
            }
        }

        private async Task PopulateInputModelWithOwnersAsync(MakeRentInputModel propertyRent)
        {
            propertyRent.Renters = await this.userService.GetAllFutureRentersAsync(propertyRent.Id);
        }
    }
}
