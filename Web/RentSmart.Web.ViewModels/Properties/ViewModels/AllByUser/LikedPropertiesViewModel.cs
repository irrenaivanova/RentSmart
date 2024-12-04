namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class LikedPropertiesViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string Price { get; set; }

        public string PropertyTypeName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, LikedPropertiesViewModel>()
                                              .ForMember(x => x.Price, opt =>
                            opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + " €"));
        }
    }
}
