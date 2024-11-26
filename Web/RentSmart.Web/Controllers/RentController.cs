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

    using RentSmart.Web.ViewModels.RentContract;

    using static RentSmart.Common.GlobalConstants;
    using static RentSmart.Common.NotificationConstants;

    [Authorize]
    public class RentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPropertyService propertyService;
        private readonly IUserService userService;
        private readonly IRentService rentService;

        public RentController(
            UserManager<ApplicationUser> userManager,
            IPropertyService propertyService,
            IUserService userService,
            IRentService rentService)
        {
            this.userManager = userManager;
            this.propertyService = propertyService;
            this.userService = userService;
            this.rentService = rentService;
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

        [HttpPost]
        public async Task<IActionResult> Make(MakeRentInputModel input)
        {
            var propertyManagerUserId = this.propertyService.GetManagerUserId(input.Id);
            var userId = this.GetUserId();

            if (userId != propertyManagerUserId)
            {
                this.TempData[ErrorMessage] = "Only managers of the property can make rentals!";
                return this.RedirectToAction("MyProperties", "Property");
            }

            bool ifAvailable = this.propertyService.IsPropertyAvailable(input.Id);
            if (!ifAvailable)
            {
                this.TempData[ErrorMessage] = "This property is currently occupied. A new rental cannot be created at this time.";
            }

            if (!this.ModelState.IsValid)
            {
                await this.PopulateInputModelWithOwnersAsync(input);
                return this.View(input);
            }

            var contractViewModel = new ContractViewModel()
            {
                DurationInMonths = input.DurationInMonths,
                DistrictName = input.DistrictName,
                ContractDate = DateTime.UtcNow.ToString("d"),
                CityName = input.CityName,
                Floor = input.Floor,
                Price = input.Price,
                ManagerName = input.ManagerName,
                OwnerName = input.OwnerName,
                PropertyTypeName = input.PropertyTypeName,
                Size = input.Size,
                RentDate = input.RentDate,
            };
            var renter = await this.userManager.FindByIdAsync(input.UserId);
            var renterName = $"{renter.FirstName} {renter.LastName}";
            contractViewModel.RenterName = renterName;
            await this.AddRenterClaim(input.UserId);

            await this.rentService.AddRentAsync(input.Id, input.UserId, input.RentDate, input.DurationInMonths);
            this.TempData[SuccessMessage] = "Rental contract added successfully!";

            return this.View("~/Views/Rent/Contract.cshtml", contractViewModel);
        }

        private async Task AddRenterClaim(string userId)
        {
            // I do not handle exceptions
            var user = await this.userManager.FindByIdAsync(userId);
            var claims = await this.userManager.GetClaimsAsync(user);
            if (!claims.Any(x => x.Type == RenterClaim && x.Value == "true"))
            {
                await this.userManager.AddClaimAsync(user, new Claim(RenterClaim, "true"));
            }
        }

        private async Task PopulateInputModelWithOwnersAsync(MakeRentInputModel propertyRent)
        {
            propertyRent.FutureRenters = await this.userService.GetAllFutureRentersAsync(propertyRent.Id);
        }
    }
}
