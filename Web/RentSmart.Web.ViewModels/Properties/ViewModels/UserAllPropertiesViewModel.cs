using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    public class UserAllPropertiesViewModel
    {
        public string Id { get; set; }

        public List<ManagerPropertyInListViewModel> ManagedProperties { get; set; }

        public List<OwnerPropertyInListViewModel> OwnedProperties { get; set; }

        public List<RenterPropertyInListViewModel> RentedProperties { get; set; }
    }
}
