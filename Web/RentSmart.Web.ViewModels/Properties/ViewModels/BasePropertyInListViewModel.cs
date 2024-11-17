namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class BasePropertyInListViewModel : IMapFrom<Property>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public string PropertyTypeName { get; set; }

        public string AverageRating { get; set; }

        public bool IsAvailable { get; set; }
    }
}
