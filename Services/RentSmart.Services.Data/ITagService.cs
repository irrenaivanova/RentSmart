namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RentSmart.Web.ViewModels.Properties.InputModels;

    public interface ITagService
    {
        public Task<IEnumerable<TagInputModel>> GetAllTagsAsync();
    }
}
