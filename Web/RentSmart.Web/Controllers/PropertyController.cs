namespace RentSmart.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RentSmart.Data.Models;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Properties;
    using RentSmart.Web.ViewModels.Properties.InputModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels.Enums;
    using static RentSmart.Common.NotificationConstants;

    // TODO: Combine some service in one 
    [Authorize]
    public class PropertyController : BaseController
    {
        private const int PropertiesPerPageManager = 6;
        private readonly ICityService cityService;
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IPropertyService propertyService;
        private readonly IWebHostEnvironment environment;
        private readonly IDistrictService districtService;

        public PropertyController(
            ICityService cityService,
            IUserService userService,
            ITagService tagService,
            IPropertyTypeService propertyTypeService,
            IPropertyService propertyService,
            IWebHostEnvironment environment,
            IDistrictService districtService)
        {
            this.cityService = cityService;
            this.userService = userService;
            this.tagService = tagService;
            this.propertyTypeService = propertyTypeService;
            this.propertyService = propertyService;
            this.environment = environment;
            this.districtService = districtService;
        }

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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string userId = this.GetUserId();
            if (!this.userService.IsManagerOfTheProperty(userId, id))
            {
                this.TempData[ErrorMessage] = "Only the manager of the property can edit it!";
                return this.RedirectToAction(nameof(this.MyProperties));
            }

            EditPropertyInputModel viewModel = this.propertyService.GetById<EditPropertyInputModel>(id);
            await this.PopulateEditModelAsync(viewModel);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPropertyInputModel input)
        {
            string userId = this.GetUserId();
            if (!this.userService.IsManagerOfTheProperty(userId, input.Id))
            {
                this.TempData[ErrorMessage] = "Only the manager of the property can edit it!";
                return this.RedirectToAction(nameof(this.MyProperties));
            }

            if (!this.ModelState.IsValid)
            {
                await this.PopulateEditModelAsync(input);
                return this.View(input);
            }

            await this.propertyService.UpdateAsync(input);
            this.TempData[SuccessMessage] = "Property edited successfully!";
            return this.RedirectToAction(nameof(this.Details), "Property", new { id = input.Id });
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            DeleteViewModel prop = this.propertyService.GetById<DeleteViewModel>(id);
            if (prop == null)
            {
                this.TempData[ErrorMessage] = "You are trying to delete an Nonexisting Property!";
            }
            string userId = this.GetUserId();
            if (!this.userService.IsManagerOfTheProperty(userId, id))
            {
                this.TempData[ErrorMessage] = "Only the manager of the property can edit it!";
                return this.RedirectToAction(nameof(this.MyProperties));
            }
            return this.View(prop);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!this.IsManager())
            {
                this.TempData[ErrorMessage] = "Only managers can delete properties!";
                return this.RedirectToAction("MyProperties");
            }

            string userId = this.GetUserId();
            if (!this.userService.IsManagerOfTheProperty(userId, id))
            {
                this.TempData[ErrorMessage] = "Only the manager of the property can edit it!";
                return this.RedirectToAction(nameof(this.MyProperties));
            }

            try
            {
                await this.propertyService.DeleteAsync(id);
                this.TempData[SuccessMessage] = "You have deleted the property successfully!";
            }
            catch (Exception ex)
            {
                this.TempData[ErrorMessage] = ex.Message;
            }

            return this.RedirectToAction("MyProperties");
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]PropertiesViewModelWithPaging model, int id = 1)
        {
            if (model.CurrentPage <= 0)
            {
                return this.NotFound();
            }

            var (properties, count) = await this.propertyService.GetAllAvailableAsync(id, model);
            var viewModel = new PropertiesViewModelWithPaging
            {
                Properties = properties,
                ItemsCount = count,
                CurrentPage = id,
                ItemsPerPage = model.ItemsPerPage,
                Action = "All",
            };

            if (id > viewModel.PagesCount)
            {
                return this.NotFound();
            }

            await this.PopulateSearchModel(viewModel);
            return this.View(viewModel);
        }

        private async Task PopulateSearchModel(PropertiesViewModelWithPaging viewModel)
        {
            var tags = await this.tagService.GetAllTagsAsync();
            viewModel.AllTags = tags.Select(x => x.Name).OrderBy(x => x).ToList();

            var propertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
            viewModel.PropertyTypes = propertyTypes.Select(x => x.Name).OrderBy(x => x).ToList();

            viewModel.DistrictsAll = await this.districtService.GetAllDistrictsAsync();
            viewModel.SortingOptions = Enum.GetValues(typeof(PropertySorting))
                .Cast<PropertySorting>()
                .Select(x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = x.ToString(),
                });
            viewModel.SearchString = string.Empty;
            viewModel.PropertyType = null;
            viewModel.Sorting = null;

        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            var property = await this.propertyService.GetByIdAsync(id);
            return this.View(property);

            // return this.Json(property);
        }

        public async Task<IActionResult> MyProperties(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var userId = this.GetUserId();
            bool isManager = this.IsManager();
            bool isOwner = this.IsOwner();
            bool isRenter = this.IsRenter();
            if (!this.User.Identity.IsAuthenticated)
            {
                this.TempData[ErrorMessage] = "Access to 'My Properties' is limited to registered users!";
                return this.Redirect("/");
            }

            var allProperties = await this.propertyService.GetByIdAllProperties(userId, isManager, isOwner, isRenter, id, PropertiesPerPageManager);
            allProperties.ManagedProperties.Action = "MyProperties";

            if (id > allProperties.ManagedProperties.PagesCount && allProperties.ManagedProperties.PagesCount == 0)
            {
                return this.NotFound();
            }

            return this.View(allProperties);
        }

        private async Task PopulateInputModelAsync(AddPropertyInputModel viewModel)
        {
            viewModel.Cities = await this.cityService.GetAllCitiesAsync();
            viewModel.Tags = await this.tagService.GetAllTagsAsync();
            viewModel.Owners = await this.userService.GetAllOwnerSAsync();
            viewModel.PropertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
        }

        private async Task PopulateEditModelAsync(EditPropertyInputModel viewModel)
        {
            viewModel.Cities = await this.cityService.GetAllCitiesAsync();
            viewModel.PropertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
        }
    }
}
