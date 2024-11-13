namespace RentSmart.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
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

        public PropertyController(
            ICityService cityService,
            IOwnerService ownerService,
            ITagService tagService,
            IPropertyTypeService propertyTypeService)
        {
            this.cityService = cityService;
            this.ownerService = ownerService;
            this.tagService = tagService;
            this.propertyTypeService = propertyTypeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!this.IsManager())
            {
                this.TempData[ErrorMessage] = "Only managers can add properties!";
                return this.RedirectToAction("Index", "Home");
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
                return this.RedirectToAction("Index", "Home");
            }

            //if (!this.ModelState.IsValid)
            //{
            //    await this.PopulateInputModelAsync(input);
            //    return this.View(input);
            //}



            this.TempData[SuccessMessage] = "Property added successfully!";
            return this.Redirect("/");
        }


        // wkarvajgi property trqbva da se izpolzva pak
        private async Task PopulateInputModelAsync(AddPropertyInputModel viewModel)
        {
            viewModel.Cities = await this.cityService.GetAllCitiesAsync();
            viewModel.Tags = await this.tagService.GetAllTagsAsync();
            viewModel.Owners = await this.ownerService.GetAllOwnerSAsync();
            viewModel.PropertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
        }
    }
}
