﻿namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using System.Collections.Generic;

    using RentSmart.Web.ViewModels;

    public class ManagerViewWithPaging : PagingViewModel
    {
        public ManagerViewWithPaging()
        {
            this.Properties = new HashSet<ManagerPropertyInListViewModel>();
        }

        public IEnumerable<ManagerPropertyInListViewModel> Properties { get; set; }
    }
}
