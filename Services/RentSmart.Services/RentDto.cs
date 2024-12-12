namespace RentSmart.Services
{
    using System;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RentDto : IMapFrom<Rental>, IHaveCustomMappings
    {

        public int Id { get; set; }

        public DateTime RentDate { get; set; }

        public int DurationInMonths { get; set; }

        public string RenterUserName { get; set; }

        public string RenterUserEmail { get; set; }

        public string PropertyPropertyTypeName { get; set; }

        public string PropertyDistrictName { get; set; }

        public string PropertyCityName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Rental, RentDto>()
                 .ForMember(x => x.RenterUserName, opt => opt.MapFrom(x => $"{x.Renter.User.FirstName} {x.Renter.User.LastName}"));
        }
    }
}