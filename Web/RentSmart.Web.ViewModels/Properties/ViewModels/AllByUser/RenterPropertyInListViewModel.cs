namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RenterPropertyInListViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string ImageUrl { get; set; }

        public string PropertyTypeName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public bool IsCurrentRental { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, RenterPropertyInListViewModel>()
                       .ForMember(x => x.ImageUrl, opt =>
                           opt.MapFrom(x => x.Images.FirstOrDefault().RemoteImageUrl != null ?
                           x.Images.FirstOrDefault().RemoteImageUrl :
                           x.Images.FirstOrDefault() == null ?
                           "/images/noimage.jpg" :
                           "/images/properties/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension));
        }
    }
}
