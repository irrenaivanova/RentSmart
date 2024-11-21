namespace RentSmart.Web.ViewModels.Properties
{
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class DeleteViewModel : IMapFrom<Property>
    {
        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public string PropertyTypeName { get; set; }
    }
}
