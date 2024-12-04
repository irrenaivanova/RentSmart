namespace RentSmart.Web.ViewModels.Service
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class ServiceViewModel : IMapFrom<Service>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Price { get; set; }

        public string Duration { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Service, ServiceViewModel>()
                .ForMember(x => x.Price, opt =>
                       opt.MapFrom(x => x.Price.ToString("0.00") + " €"));
        }
    }
}
