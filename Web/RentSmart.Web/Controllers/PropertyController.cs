namespace RentSmart.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Properties;

    public class PropertyController : BaseController
    {
        private readonly ICityService cityService;
        private readonly IOwnerService ownerService;
        private readonly ITagService tagService;

        public PropertyController(
            ICityService cityService,
            IOwnerService ownerService,
            ITagService tagService)
        {
            this.cityService = cityService;
            this.ownerService = ownerService;
            this.tagService = tagService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return this.View(this.PopulateInputModel());
        }

        private CreatePropertyInputModel PopulateInputModel()
        {
            var viewModel = new CreatePropertyInputModel();
            viewModel.Cities = this.cityService.GetAllCities();
            return viewModel;
        }
    }
}
