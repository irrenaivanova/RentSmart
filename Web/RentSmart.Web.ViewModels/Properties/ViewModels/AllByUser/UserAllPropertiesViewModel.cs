namespace RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser
{
    using System.Collections.Generic;

    public class UserAllPropertiesViewModel
    {
        public UserAllPropertiesViewModel()
        {
            this.OwnedProperties = new List<OwnerPropertyInListViewModel>();
            this.RentedProperties = new List<RenterPropertyInListViewModel>();
            this.LikedProperties = new List<LikedPropertiesViewModel>();
        }

        public string Id { get; set; }

        public ManagerViewWithPaging ManagedProperties { get; set; } = new ManagerViewWithPaging();

        public IList<OwnerPropertyInListViewModel> OwnedProperties { get; set; }

        public IList<RenterPropertyInListViewModel> RentedProperties { get; set; }

        public IList<LikedPropertiesViewModel> LikedProperties { get; set; }
    }
}
