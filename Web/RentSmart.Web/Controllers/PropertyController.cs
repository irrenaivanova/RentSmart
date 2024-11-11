﻿namespace RentSmart.Web.Controllers
{
    using System.Threading.Tasks;

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
        public async Task<IActionResult> Add()
        {
            var model = await this.PopulateInputModel();
            return this.View(model);
        }

        // wkarvajgi property trqbva da se izpolzva paketa
        private async Task<AddPropertyInputModel> PopulateInputModel()
        {
            var viewModel = new AddPropertyInputModel();
            viewModel.Cities = await this.cityService.GetAllCitiesAsync();
            viewModel.Tags = await this.tagService.GetAllTagsAsync();
            viewModel.Owners = await this.ownerService.GetAllOwnerSAsync();
            return viewModel;
        }
    }
}
