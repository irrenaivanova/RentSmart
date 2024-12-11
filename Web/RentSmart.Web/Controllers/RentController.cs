namespace RentSmart.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Data.Models;
    using RentSmart.Services.Data;
    using RentSmart.Services.Messaging;
    using RentSmart.Web.ViewModels.RentContract;
    using Rotativa.AspNetCore;

    using static RentSmart.Common.GlobalConstants;
    using static RentSmart.Common.NotificationConstants;

    using ApplicationUser = RentSmart.Data.Models.ApplicationUser;

    [Authorize]
    public class RentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPropertyService propertyService;
        private readonly IUserService userService;
        private readonly IRentService rentService;
        private readonly IWebHostEnvironment environent;
        private readonly IEmailSender sender;

        public RentController(
            UserManager<ApplicationUser> userManager,
            IPropertyService propertyService,
            IUserService userService,
            IRentService rentService,
            IWebHostEnvironment environment,
            IEmailSender sender)
        {
            this.userManager = userManager;
            this.propertyService = propertyService;
            this.userService = userService;
            this.rentService = rentService;
            this.environent = environment;
            this.sender = sender;
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
            await this.PopulateInputModelWithFutureRenterAsync(propertyRent);
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
                return this.RedirectToAction("MyProperties", "Property");
            }

            if (!this.ModelState.IsValid)
            {
                await this.PopulateInputModelWithFutureRenterAsync(input);
                return this.View(input);
            }

            (int rentalId, string rentalContractUrl) = await this.rentService.AddRentAsync(input.Id, input.UserId, input.RentDate, input.DurationInMonths);

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
                RentalId = rentalId,
                RentalContractUrl = rentalContractUrl,
            };
            var renter = await this.userManager.FindByIdAsync(input.UserId);
            var renterName = $"{renter.FirstName} {renter.LastName}";
            contractViewModel.RenterName = renterName;
            await this.AddRenterClaim(input.UserId);

            var pdf = new ViewAsPdf("Contract", contractViewModel)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };
            var pdfBytes = await pdf.BuildFile(this.ControllerContext);
            var fileName = $"{contractViewModel.RentalContractUrl}.pdf";
            var filePath = Path.Combine(this.environent.WebRootPath, "pdf/contracts", fileName);
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

            string emailContent = "You will find your new rental contract attached!";
            string contractName = $"RentalContract {contractViewModel.RentalId} / {contractViewModel.RentDate.ToString("d")}.pdf";
            var emailAttach = new EmailAttachment()
            {
                Content = pdfBytes,
                FileName = contractName,
                MimeType = "application/pdf",
            };
            await this.sender.SendEmailAsync(SystemEmailSender, $"RentSmart {contractViewModel.ManagerName}", renter.Email, contractName, emailContent, new List<EmailAttachment>() { emailAttach });


            this.TempData[SuccessMessage] = "Rental contract is made successfully. It can be found on MyProperties page as PDF and is sent by email!";
            return this.RedirectToAction("MyProperties", "Property");
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

        private async Task PopulateInputModelWithFutureRenterAsync(MakeRentInputModel propertyRent)
        {
            propertyRent.FutureRenters = await this.userService.GetAllFutureRentersAsync(propertyRent.Id);
        }
    }
}
