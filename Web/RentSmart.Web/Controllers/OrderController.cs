﻿namespace RentSmart.Web.Controllers
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
    using RentSmart.Web.ViewModels.Service;

    using static RentSmart.Common.GlobalConstants;
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

        [HttpGet]
        public IActionResult Make()
        {
            var services = this.orderService.GetAllAsync<ServiceViewModel>();
            return this.View(services);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Make(int id)
        {
            string userId = this.GetUserId();
            try
            {
                await this.orderService.AddNewOrderAsync(id, userId);
            }
            catch (Exception ex)
            {
                this.TempData[ErrorMessage] = ex.Message;
                return this.Redirect("/");
            }

            await this.UpgradeToOwner(userId);
            this.TempData[SuccessMessage] = "Your order is successful!";
            return this.Redirect("/");
        }

        private async Task UpgradeToOwner(string userId)
        {
            // I do not handle exceptions
            var user = await this.userManager.FindByIdAsync(userId);
            var claims = await this.userManager.GetClaimsAsync(user);
            if (!claims.Any(x => x.Type == OwnerClaim && x.Value == "true"))
            {
                await this.userManager.AddClaimAsync(user, new Claim(OwnerClaim, "true"));
            }
        }
    }
}
