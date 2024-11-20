namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class PropertyDetailsViewModel : BasePropertyInListViewModel, IHaveCustomMappings
    {
        public PropertyDetailsViewModel()
        {
            this.TagsTagNames = new List<string>();
            this.ImagesUrls = new List<string>();
        }

        public string Description { get; set; }

        public string OriginalUrl { get; set; }

        public string ManagerName { get; set; }

        public string OwnerName { get; set; }

        public string Price { get; set; }

        public int TotalLikes { get; set; }

        public DateTime AppointentDateTime { get; set; }

        public IList<string> TagsTagNames { get; set; }

        public IList<string> ImagesUrls { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, PropertyDetailsViewModel>()
                .ForMember(x => x.ManagerName, opt => opt.MapFrom(x => $"{x.Manager.User.FirstName} {x.Manager.User.LastName}"))
                .ForMember(x => x.OwnerName, opt => opt.MapFrom(x => $"{x.Owner.User.FirstName} {x.Owner.User.LastName}"))
                .ForMember(x => x.Price, opt =>
                       opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + " €"));
        }
    }
}
