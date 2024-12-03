namespace RentSmart.Web.ViewModels.Properties.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RentSmart.Web.ViewModels.Owner;

    public class AddPropertyInputModel : BasePropertyInputModel
    {
        public AddPropertyInputModel()
        {
            this.TagIds = new List<int>();
            this.Tags = new List<TagInputModel>();
        }

        public IEnumerable<IFormFile> Images { get; set; }

        [Required]
        [Display(Name = "Owner")]
        public string OwnerId { get; set; }

        public IEnumerable<OwnerInputModel> Owners { get; set; }

        public List<int> TagIds { get; set; }

        public IEnumerable<TagInputModel> Tags { get; set; }
    }
}
