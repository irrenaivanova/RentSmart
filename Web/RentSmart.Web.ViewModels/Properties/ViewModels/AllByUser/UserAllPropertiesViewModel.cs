namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserAllPropertiesViewModel
    {
        public UserAllPropertiesViewModel()
        {
            OwnedProperties = new List<OwnerPropertyInListViewModel>();
            RentedProperties = new List<RenterPropertyInListViewModel>();
        }

        public string Id { get; set; }

        public ManagerViewWithPaging ManagedProperties { get; set; } = new ManagerViewWithPaging();

        public IList<OwnerPropertyInListViewModel> OwnedProperties { get; set; }

        public IList<RenterPropertyInListViewModel> RentedProperties { get; set; }
    }
}
