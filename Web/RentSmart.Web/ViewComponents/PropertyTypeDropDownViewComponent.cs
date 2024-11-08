namespace RentSmart.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;

    public class PropertyTypeDropDownViewComponent : ViewComponent
    {
        private readonly IPropertyTypeService propertyTypeService;

        public PropertyTypeDropDownViewComponent(IPropertyTypeService propertyTypeService)
        {
            this.propertyTypeService = propertyTypeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var propertyTypes = await this.propertyTypeService.AllPropertyTypesAsync();
            return this.View(propertyTypes);
        }
    }
}
