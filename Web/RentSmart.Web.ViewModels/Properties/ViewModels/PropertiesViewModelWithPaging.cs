# nullable enable
namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Web.ViewModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels.Enums;
    using static RentSmart.Common.GlobalConstants;

    public class PropertiesViewModelWithPaging : PagingViewModel
    {
        public PropertiesViewModelWithPaging()
        {
            this.Properties = new List<PropertyInListViewModel>();
            this.CurrentPage = DefaultPage;
            this.ItemsPerPage = DefaultItemsPerPage;

        }

        public IEnumerable<PropertyInListViewModel> Properties { get; set; }

        [Display(Name = "Search by word")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort By")]
        public PropertySorting? Sorting { get; set; }
    }
}
