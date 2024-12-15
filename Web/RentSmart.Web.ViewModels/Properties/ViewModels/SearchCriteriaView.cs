namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RentSmart.Web.ViewModels.Properties.ViewModels.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static RentSmart.Common.EntityValidationConstants.Property;
    using static RentSmart.Common.GlobalConstants;

    public class SearchCriteriaView
    {
        public SearchCriteriaView()
        {
            this.PricePerMonthMax = DefaultPricePerMonth;

            this.PropertyTypes = new HashSet<string>();
            this.Tags = new HashSet<string>();
            this.AllTags = new HashSet<string>();
            this.Districts = new HashSet<string>();
            this.DistrictsAll = new HashSet<string>();

        }

        public string? SearchString { get; set; }

        [Display(Name = "Sort By")]
        public PropertySorting? Sorting { get; set; }

        [Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
        [Display(Name = "Max Monthly Price")]
        public decimal PricePerMonthMax { get; set; }

        public string? PropertyType { get; set; }

        public IEnumerable<string> PropertyTypes { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<string> AllTags { get; set; }

        public IEnumerable<string> Districts { get; set; }

        public IEnumerable<string> DistrictsAll { get; set; }

        public IEnumerable<SelectListItem> SortingOptions { get; set; }
    }
}
