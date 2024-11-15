using AutoMapper;
using RentSmart.Data.Models;
using RentSmart.Services.Mapping;
using System;

namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    public class AppointmentViewModel : IMapFrom<Appointment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public string FutureRenterName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(x => x.FutureRenterName, opt => opt.MapFrom(x => $"{x.User.FirstName} {x.User.LastName}"));
        }
    }
}