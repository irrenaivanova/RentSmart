namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;

    using AutoMapper;
    using RentSmart.Data.Models;

    public class PropertyDetailsViewModel : BasePropertyInListViewModel
    {
        public PropertyDetailsViewModel()
        {
            this.PropertyTagsTagNames = new HashSet<string>();
            this.ImagesUrls = new HashSet<string>();
            this.Comments = new HashSet<string>();
        }

        public string Description { get; set; }

        public string OriginalUrl { get; set; }

        public string ManagerName { get; set; }

        public string OwnerName { get; set; }

        public IEnumerable<string> PropertyTagsTagNames { get; set; }

        public IEnumerable<string> ImagesUrls { get; set; }

        public IEnumerable<string> Comments { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            base.CreateMappings(configuration);
            configuration.CreateMap<Property, PropertyDetailsViewModel>()
                .ForMember(x => x.ManagerName, opt => opt.MapFrom(x => $"{x.Manager.User.FirstName} {x.Manager.User.LastName}"))
                .ForMember(x => x.OwnerName, opt => opt.MapFrom(x => $"{x.Owner.User.FirstName} {x.Owner.User.LastName}"));
        }
    }
}
