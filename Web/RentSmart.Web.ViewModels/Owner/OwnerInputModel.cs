namespace RentSmart.Web.ViewModels.Owner
{
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class OwnerInputModel : IMapFrom<Owner>
    {
        public string Id { get; set; }

        public string UserEmail { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
    }
}
