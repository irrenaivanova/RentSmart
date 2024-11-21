namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;
    using RentSmart.Web.ViewModels;

    public class PropertiesViewModelWithPaging : PagingViewModel
    {
        public PropertiesViewModelWithPaging()
        {
            this.Properties = new List<PropertyInListViewModel>();
        }

        public IEnumerable<PropertyInListViewModel> Properties { get; set; }
    }
}
