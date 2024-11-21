namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using System;

    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RentalViewModel : IMapFrom<Rental>
    {
        public int Id { get; set; }

        public DateTime RentDate { get; set; }

        public string ContractUrl { get; set; }
    }
}
