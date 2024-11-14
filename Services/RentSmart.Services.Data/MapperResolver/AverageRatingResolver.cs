namespace RentSmart.Services.Data.MapperResolver
{
    using System;

    using AutoMapper;
    using RentSmart.Data.Models;
    using RentSmart.Web.ViewModels.Properties;

    public class AverageRatingResolver : IValueResolver<Property, PropertyInListViewModel, string>
    {
        private readonly IPropertyService propertyService;

        public AverageRatingResolver(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        public string Resolve(Property source, PropertyInListViewModel destination, string destMember, ResolutionContext context)
        {
            var averageRating = this.propertyService.AveragePropertyRating(source.Id);
            return averageRating.ToString("0.0");
        }
    }
}
