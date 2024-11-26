namespace RentSmart.Web.ViewModels.Owner
{
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class RenterInputModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
