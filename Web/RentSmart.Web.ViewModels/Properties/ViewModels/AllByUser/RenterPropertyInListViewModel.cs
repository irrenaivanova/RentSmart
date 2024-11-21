namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RenterPropertyInListViewModel : IMapFrom<Property>
    {
        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string PropertyTypeName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public bool IsCurrentRental { get; set; }
    }
}
