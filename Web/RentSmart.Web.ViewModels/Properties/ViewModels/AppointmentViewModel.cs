namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class AppointmentViewModel : IMapFrom<Appointment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string DateTime { get; set; }

        public string FutureRenterName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(x => x.FutureRenterName, opt => opt.MapFrom(x => $"{x.User.FirstName} {x.User.LastName}"))
                .ForMember(x => x.DateTime, opt => opt.MapFrom(x => x.DateTime.ToString("g")));
        }
    }
}
