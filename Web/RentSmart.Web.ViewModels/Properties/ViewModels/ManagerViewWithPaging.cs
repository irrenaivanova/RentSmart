namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;

    public class ManagerViewWithPaging : PagingViewModel
    {
        public ManagerViewWithPaging()
        {
            Properties = new HashSet<ManagerPropertyInListViewModel>();
        }

        public IEnumerable<ManagerPropertyInListViewModel> Properties { get; set; }
    }
}
