namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
	using RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser;

	public class OwnerPropertyInListViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public OwnerPropertyInListViewModel()
        {
            this.Rentals = new HashSet<RentalViewModel>();
        }

        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string ImageUrl { get; set; }

        public string PropertyTypeName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public string Price { get; set; }

        public bool IsAvailable { get; set; }

        public IEnumerable<RentalViewModel> Rentals { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, OwnerPropertyInListViewModel>()
                                              .ForMember(x => x.Price, opt =>
                            opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + " €"));
        }
    }
}
