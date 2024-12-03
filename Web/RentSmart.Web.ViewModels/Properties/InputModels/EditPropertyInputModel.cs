namespace RentSmart.Web.ViewModels.Properties.InputModels
{
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class EditPropertyInputModel : BasePropertyInputModel, IMapFrom<Property>
    {
        public string Id { get; set; }
    }
}
