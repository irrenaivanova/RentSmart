namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Linq;

    using AutoMapper;
    using RentSmart.Services.Mapping;

    using Property = RentSmart.Data.Models.Property;

    public class PropertyInListViewModel : BasePropertyInListViewModel, IHaveCustomMappings
    {
        public string ImageUrl { get; set; }

        public string Price { get; set; }

        public int TotalLikes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, PropertyInListViewModel>()
                       .ForMember(x => x.Price, opt => opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + " €"))
                       .ForMember(x => x.ImageUrl, opt =>
                           opt.MapFrom(x => x.Images.FirstOrDefault().RemoteImageUrl != null ?
                           x.Images.FirstOrDefault().RemoteImageUrl :
                           x.Images.FirstOrDefault() == null ?
                           "/images/noimage.jpg" :
                           "/images/properties/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension));
        }
    }
}
