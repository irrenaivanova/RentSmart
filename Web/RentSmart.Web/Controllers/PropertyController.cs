namespace RentSmart.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Properties;

    using static RentSmart.Common.NotificationConstants;

    public class PropertyController : BaseController
    {
        private readonly ICityService cityService;
        private readonly IOwnerService ownerService;
        private readonly ITagService tagService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IPropertyService propertyService;
        private readonly IWebHostEnvironment environment;

        public PropertyController(
            ICityService cityService,
            IOwnerService ownerService,
            ITagService tagService,
            IPropertyTypeService propertyTypeService,
            IPropertyService propertyService,
            IWebHostEnvironment environment)
        {
            this.cityService = cityService;
            this.ownerService = ownerService;
            this.tagService = tagService;
            this.propertyTypeService = propertyTypeService;
            this.propertyService = propertyService;
            this.environment = environment;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!this.IsManager())
            {
                this.TempData[ErrorMessage] = "Only managers can add properties!";
                return this.RedirectToAction("/");
            }

            var viewModel = new AddPropertyInputModel();
            await this.PopulateInputModelAsync(viewModel);
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddPropertyInputModel input)
        {
            if (!this.IsManager())
            {
                this.TempData[ErrorMessage] = "Only managers can add properties!";
                return this.RedirectToAction("/");
            }

            if (!this.ModelState.IsValid)
            {
                await this.PopulateInputModelAsync(input);
                return this.View(input);
            }

            var userId = this.GetUserId();
            try
            {
                await this.propertyService.AddAsync(input, userId, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                await this.PopulateInputModelAsync(input);
                return this.View(input);
            }

            this.TempData[SuccessMessage] = "Property added successfully!";
            return this.Redirect("/");
        }

        public async Task<IActionResult> All()
        {
            var properties = await this.propertyService.GetAllAvailableAsync<PropertyInListViewModel>();
            return Json(properties);
        }

        private async Task PopulateInputModelAsync(AddPropertyInputModel viewModel)
        {
            viewModel.Cities = await this.cityService.GetAllCitiesAsync();
            viewModel.Tags = await this.tagService.GetAllTagsAsync();
            viewModel.Owners = await this.ownerService.GetAllOwnerSAsync();
            viewModel.PropertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
        }
    }
}
