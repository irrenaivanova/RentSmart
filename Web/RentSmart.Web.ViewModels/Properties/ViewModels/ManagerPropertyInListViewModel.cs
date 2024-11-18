namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class ManagerPropertyInListViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public ManagerPropertyInListViewModel()
        {
            this.Appointments = new HashSet<AppointmentViewModel>();
            this.Rentals = new HashSet<RentalViewModel>();
        }

        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; }

        public IEnumerable<RentalViewModel> Rentals { get; set; }

        public IEnumerable<AppointmentViewModel> Appointments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, ManagerPropertyInListViewModel>()
                       .ForMember(x => x.ImageUrl, opt =>
                           opt.MapFrom(x => x.Images.FirstOrDefault().RemoteImageUrl != null ?
                           x.Images.FirstOrDefault().RemoteImageUrl :
                           x.Images.FirstOrDefault() == null ?
                           "/images/noimage.jpg" :
                           "/images/properties/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension));

        }
    }
}
