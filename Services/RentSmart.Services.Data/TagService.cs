namespace RentSmart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Properties.InputModels;

    public class TagService : ITagService
    {
        private readonly IDeletableEntityRepository<Tag> tagRepository;

        public TagService(IDeletableEntityRepository<Tag> tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagInputModel>> GetAllTagsAsync()
        {
            return await this.tagRepository.AllAsNoTracking().To<TagInputModel>().OrderBy(x => x.Name).ToListAsync();
        }
    }
}
