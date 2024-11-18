namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System;

    public class PagingViewModel
    {
        public int CurrentPage { get; set; }

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PagesCount => (int)Math.Ceiling(this.ItemsCount * 1.0 / this.ItemsPerPage);
    }
}
