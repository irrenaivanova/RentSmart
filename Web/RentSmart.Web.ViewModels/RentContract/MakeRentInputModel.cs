namespace RentSmart.Web.ViewModels.RentContract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Owner;
    using RentSmart.Web.ViewModels.Properties.ViewModels;

    using static RentSmart.Common.EntityValidationConstants.Rental;

    public class MakeRentInputModel : IMapFrom<Property>, IMapTo<ContractViewModel>, IHaveCustomMappings
    {
            public MakeRentInputModel()
            {
                this.FutureRenters = new List<RenterInputModel>();
            }

            public string Id { get; set; }

            public string DistrictName { get; set; }

            public string CityName { get; set; }

            public double Size { get; set; }

            public byte Floor { get; set; }

            public string PropertyTypeName { get; set; }

            public string Price { get; set; }

            public string ManagerName { get; set; }

            public string OwnerName { get; set; }

            [Required]
            [Display(Name = "Renter")]
            public string UserId { get; set; }

            public IEnumerable<RenterInputModel> FutureRenters { get; set; }

            [Required]
            [Range(MinRentalDuration, MaxRentalDuration)]
            public int DurationInMonths { get; set; }

            [Required]
            [Display(Name = "Start Date")]
            public DateTime RentDate { get; set; }

            public void CreateMappings(IProfileExpression configuration)
            {
                configuration.CreateMap<Property, MakeRentInputModel>()
                     .ForMember(x => x.ManagerName, opt => opt.MapFrom(x => $"{x.Manager.User.FirstName} {x.Manager.User.LastName}"))
                     .ForMember(x => x.OwnerName, opt => opt.MapFrom(x => $"{x.Owner.User.FirstName} {x.Manager.User.LastName}"))
                     .ForMember(x => x.Price, opt =>
                            opt.MapFrom(x => x.PricePerMonth.ToString("0.00") + " €"));
            }
    }
}
