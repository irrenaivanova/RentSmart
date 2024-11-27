namespace RentSmart.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Rate;

    using static RentSmart.Common.NotificationConstants;

    public class RateController : BaseController
    {
        private readonly IUserService userService;
        private readonly IPropertyService propertyService;
        private readonly IRentService rentService;

        public RateController(
            IUserService userService,
            IPropertyService propertyService,
            IRentService rentService)
        {
            this.userService = userService;
            this.propertyService = propertyService;
            this.rentService = rentService;
        }

        [HttpGet]
        public async Task<IActionResult> Make(string id)
        {
            string userId = this.GetUserId();
            if (!await this.userService.IsThisUserPassedRentalOfTheProperty(userId, id))
            {
                this.TempData[ErrorMessage] = "You must have rented the property in order to rate it!";
                return this.RedirectToAction("MyProperties", "Property");
            }

            var property = this.propertyService.GetById<RateOfProperty>(id);
            return this.View(property);
        }

        // TODO; methods IsThisUserPassedRentalOfTheProperty and GetRentalId should be in rentcontroller
        [HttpPost]
        public async Task<IActionResult> Make(RateOfProperty input)
        {
            string userId = this.GetUserId();
            if (!await this.userService.IsThisUserPassedRentalOfTheProperty(userId, input.Id))
            {
                this.TempData[ErrorMessage] = "You must have rented the property in order to rate it!";
                return this.RedirectToAction("MyProperties", "Property");
            }

            int rentalId;

            try
            {
                rentalId = await this.userService.GetRentalId(userId, input.Id);
            }
            catch (Exception ex)
            {
                this.TempData[ErrorMessage] = ex.Message;
                return this.RedirectToAction("MyProperties", "Property");
            }

            await this.rentService.AddRatingAsync(rentalId, input.ConditionAndMaintenanceRate, input.Location, input.ValueForMoney);
            this.TempData[SuccessMessage] = "You rated the property successfully!";
            return this.RedirectToAction("MyProperties", "Property");
        }
    }
}
