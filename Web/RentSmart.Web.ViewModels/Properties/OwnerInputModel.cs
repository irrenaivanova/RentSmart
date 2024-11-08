namespace RentSmart.Web.ViewModels.Properties
{
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class OwnerInputModel : IMapFrom<Owner>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
