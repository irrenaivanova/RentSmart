namespace RentSmart.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
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
