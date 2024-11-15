namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class BasePropertyInListViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string Price { get; set; }

        public byte Floor { get; set; }

        public string PropertyTypeName { get; set; }

        public string AverageRating { get; set; }

        public bool IsAvailable { get; set; }

        public virtual void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, BasePropertyInListViewModel>()
                       .ForMember(x => x.Price, opt =>
                       opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + "€"));
        }
    }
}
