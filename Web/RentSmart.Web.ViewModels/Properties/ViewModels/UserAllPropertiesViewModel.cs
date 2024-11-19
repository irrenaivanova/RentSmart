﻿namespace RentSmart.Web.ViewModels.Properties.ViewModels
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
            this.OwnedProperties = new List<OwnerPropertyInListViewModel>();
            this.RentedProperties = new List<RenterPropertyInListViewModel>();
        }

        public string Id { get; set; }

        public ManagerViewWithPaging ManagedProperties { get; set; } = new ManagerViewWithPaging();

        public IList<OwnerPropertyInListViewModel> OwnedProperties { get; set; }

        public IList<RenterPropertyInListViewModel> RentedProperties { get; set; }
    }
}