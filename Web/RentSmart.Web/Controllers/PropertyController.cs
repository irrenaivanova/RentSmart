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

            var model = await this.PopulateInputModel();
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddPropertyInputModel input)
        {
            return this.Json(input);
        }



        // wkarvajgi property trqbva da se izpolzva pak
        private async Task<AddPropertyInputModel> PopulateInputModel()
        {
            var viewModel = new AddPropertyInputModel();
            viewModel.Cities = await this.cityService.GetAllCitiesAsync();
            viewModel.Tags = await this.tagService.GetAllTagsAsync();
            viewModel.Owners = await this.ownerService.GetAllOwnerSAsync();
            viewModel.PropertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
            return viewModel;
        }
    }
}
