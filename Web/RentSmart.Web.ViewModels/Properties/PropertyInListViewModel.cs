using Microsoft.Extensions.Configuration;
using RentSmart.Web.ViewModels.Properties;

namespace RentSmart.Web.ViewModels.Properties
{
    using AutoMapper;
    using RentSmart.Services.Mapping;

    using System.Linq;

    using Property = RentSmart.Data.Models.Property;

    public class PropertyInListViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string ImageUrl { get; set; }

        public string Price { get; set; }

        public byte Floor { get; set; }

        public string PropertyTypeName { get; set; }

        public string AverageRating { get; set; }

        public bool IsAvailable { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, PropertyInListViewModel>()
                       .ForMember(x => x.ImageUrl, opt =>
                           opt.MapFrom(x => x.Images.FirstOrDefault().RemoteImageUrl != null ?
                           x.Images.FirstOrDefault().RemoteImageUrl :
                           (x.Images.FirstOrDefault() == null ?
                           "/images/noimage.jpg/" :
                           "/images/properties/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension)))
                       .ForMember(x => x.Price, opt =>
                       opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + "€"));
        }
    }
}