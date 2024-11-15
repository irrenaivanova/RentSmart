namespace RentSmart.Web.ViewModels.Properties.InputModels
{
    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class PropertyTypeInputModel : IMapFrom<PropertyType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
