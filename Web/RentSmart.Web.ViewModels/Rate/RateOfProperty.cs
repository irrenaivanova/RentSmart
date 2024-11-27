namespace RentSmart.Web.ViewModels.Rate
{
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RateOfProperty : IMapFrom<Property>
    {
        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public double Size { get; set; }

        public byte Floor { get; set; }

        public string PropertyTypeName { get; set; }

        public int RentalId { get; set; }

        [Range(1, 5)]
        public int ConditionAndMaintenanceRate { get; set; }

        [Range(1, 5)]
        public int Location { get; set; }

        [Range(1, 5)]
        public int ValueForMoney { get; set; }
    }
}
