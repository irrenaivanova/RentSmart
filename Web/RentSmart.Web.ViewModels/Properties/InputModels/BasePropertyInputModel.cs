namespace RentSmart.Web.ViewModels.Properties.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static RentSmart.Common.EntityValidationConstants.Property;

    public class BasePropertyInputModel
    {
        public BasePropertyInputModel()
        {
            this.PropertyTypes = new HashSet<PropertyTypeInputModel>();
            this.Cities = new HashSet<CityInputModel>();
            this.TagIds = new List<int>();
            this.Tags = new List<TagInputModel>();
            this.CustomTags = new List<string>();
        }

        [MinLength(MinLengthName)]
        [MaxLength(MaxLengthName)]
        public string Name { get; set; }

        [MinLength(MinLengthDescription)]
        [MaxLength(MaxLengthDescription)]
        public string Description { get; set; }

        public byte Floor { get; set; }

        public double Size { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public int PropertyTypeId { get; set; }

        public IEnumerable<PropertyTypeInputModel> PropertyTypes { get; set; }

        [Required]
        [Display(Name = "District Name")]
        [MinLength(3)]
        [MaxLength(50)]
        public string DistrictName { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }

        public IEnumerable<CityInputModel> Cities { get; set; }

        [Required]
        [Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
        [Display(Name = "Monthly Price")]
        public decimal PricePerMonth { get; set; }

        public List<int> TagIds { get; set; }

        public IEnumerable<TagInputModel> Tags { get; set; }

        public IEnumerable<string> CustomTags { get; set; }
    }
}
