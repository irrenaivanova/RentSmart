namespace RentSmart.Web.ViewModels.Properties.ViewModels
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
            this.ManagedProperties = new List<ManagerPropertyInListViewModel>();
            this.OwnedProperties = new List<OwnerPropertyInListViewModel>();
            this.RentedProperties = new List<RenterPropertyInListViewModel>();
        }

        public string Id { get; set; }

        public IList<ManagerPropertyInListViewModel> ManagedProperties { get; set; }

        public IList<OwnerPropertyInListViewModel> OwnedProperties { get; set; }

        public IList<RenterPropertyInListViewModel> RentedProperties { get; set; }
    }
}
