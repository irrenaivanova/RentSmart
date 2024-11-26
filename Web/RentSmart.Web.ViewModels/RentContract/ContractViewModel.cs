namespace RentSmart.Web.ViewModels.RentContract
{
    using System;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class ContractViewModel : IMapFrom<Rental>, IHaveCustomMappings
    {
        public string PropertyDistrictName { get; set; }

        public string PropertyCityName { get; set; }

        public double PropertySize { get; set; }

        public byte PropertyFloor { get; set; }

        public string PropertyTypeName { get; set; }

        public string PropertyPrice { get; set; }

        public string PropertyManagerName { get; set; }

        public string PropertyOwnerName { get; set; }

        public string ContractDate { get; set; }

        public int DurationInMonths { get; set; }

        public DateTime RentDate { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, ContractViewModel>()
                 .ForMember(x => x.PropertyManagerName, opt => opt.MapFrom(x => $"{x.Manager.User.FirstName} {x.Manager.User.LastName}"))
                 .ForMember(x => x.PropertyOwnerName, opt => opt.MapFrom(x => $"{x.Owner.User.FirstName} {x.Owner.User.LastName}"))
                 .ForMember(x => x.PropertyPrice, opt =>
                        opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + " €"));
        }
    }
}
