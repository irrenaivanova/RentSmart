namespace RentSmart.Web.ViewModels.RentContract
{
    using System;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class ContractViewModel : IMapFrom<MakeRentInputModel>
    {
        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public string PropertyTypeName { get; set; }

        public string Price { get; set; }

        public string ManagerName { get; set; }

        public string OwnerName { get; set; }

        public string RenterName { get; set; }

        public string ContractDate { get; set; }

        public int DurationInMonths { get; set; }

        public DateTime RentDate { get; set; }

    }
}
