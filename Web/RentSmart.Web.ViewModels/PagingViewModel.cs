namespace RentSmart.Web.ViewModels
{
    using System;

    public class PagingViewModel
    {
        public string Action { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PagesCount => (int)Math.Ceiling(ItemsCount * 1.0 / ItemsPerPage);
    }
}
