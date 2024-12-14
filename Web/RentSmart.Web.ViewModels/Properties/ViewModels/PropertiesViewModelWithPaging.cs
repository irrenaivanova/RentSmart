# nullable enable
namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RentSmart.Web.ViewModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels.Enums;

    using static RentSmart.Common.EntityValidationConstants.Property;
    using static RentSmart.Common.GlobalConstants;

    public class PropertiesViewModelWithPaging : PagingViewModel
    {
        public PropertiesViewModelWithPaging()
        {
            this.Properties = new List<PropertyInListViewModel>();
            this.CurrentPage = DefaultPage;
            this.ItemsPerPage = DefaultItemsPerPage;
            this.PricePerMonth = 800;

            this.PropertyTypes = new HashSet<string>();
            this.Tags = new HashSet<string>();
            this.AllTags = new HashSet<string>();
            this.Districts = new HashSet<string>();
            this.DistrictsAll = new HashSet<string>();

        }

        public IEnumerable<PropertyInListViewModel> Properties { get; set; }

        [Display(Name = "Search by word")]
        [MinLength(3, ErrorMessage = "The search string must be at least 3 characters.")]
        [MaxLength(30, ErrorMessage = "The search string cannot exceed 30 characters.")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort By")]
        public PropertySorting? Sorting { get; set; }

        [Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
        [Display(Name = "Max Monthly Price")]
        public decimal PricePerMonth { get; set; }

        public string? PropertyType { get; set; }

        public IEnumerable<string> PropertyTypes { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<string> AllTags { get; set; }

        public IEnumerable<string> Districts { get; set; }

        public IEnumerable<string> DistrictsAll { get; set; }

        public IEnumerable<SelectListItem> SortingOptions { get; set; }

    }
}
