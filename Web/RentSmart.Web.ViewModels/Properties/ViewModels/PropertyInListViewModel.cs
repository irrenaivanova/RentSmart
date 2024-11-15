namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Linq;

    using AutoMapper;
    using RentSmart.Services.Mapping;

    using Property = RentSmart.Data.Models.Property;

    public class PropertyInListViewModel : BasePropertyInListViewModel
    {
        public string ImageUrl { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            base.CreateMappings(configuration);
            configuration.CreateMap<Property, PropertyInListViewModel>()
                       .ForMember(x => x.ImageUrl, opt =>
                           opt.MapFrom(x => x.Images.FirstOrDefault().RemoteImageUrl != null ?
                           x.Images.FirstOrDefault().RemoteImageUrl :
                           x.Images.FirstOrDefault() == null ?
                           "/images/noimage.jpg/" :
                           "/images/properties/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension));
        }
    }
}
