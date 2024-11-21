namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RentalViewModelWithRating : RentalViewModel, IHaveCustomMappings
    {
        public string AverageRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Rental, RentalViewModelWithRating>()
                .ForMember(x => x.AverageRating, opt => opt.MapFrom(x => x.Rating == null ?
                "Not rated yet" : x.Rating.AverageRating.ToString("0,0")));
        }
    }
}
