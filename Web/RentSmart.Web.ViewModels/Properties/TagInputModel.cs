namespace RentSmart.Web.ViewModels.Properties
{
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    public class TagInputModel : IMapFrom<Tag>, IMapTo<Tag>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
