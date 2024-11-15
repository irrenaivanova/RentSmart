namespace RentSmart.Web.ViewModels.Properties.ViewModels
{
    using System.Collections.Generic;

    public class ManagerPropertyInListViewModel : OwnerPropertyInListViewModel
    {
        public ManagerPropertyInListViewModel()
        {
            this.Appointments = new HashSet<AppointmentViewModel>();
        }

        public IEnumerable<AppointmentViewModel> Appointments { get; set; }
    }
}
