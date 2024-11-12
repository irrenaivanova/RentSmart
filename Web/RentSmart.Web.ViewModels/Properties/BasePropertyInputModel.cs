﻿namespace RentSmart.Web.ViewModels.Properties
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

    using static RentSmart.Common.EntityValidationConstants.Property;

    public class BasePropertyInputModel
    {
        public BasePropertyInputModel()
        {
            this.PropertyTypes = new HashSet<PropertyTypeInputModel>();
            this.Cities = new HashSet<CityInputModel>();
            this.TagIds = new List<int>();
            this.Tags = new List<TagInputModel>();
        }

        [Required]
        [MinLength(MinLengthName)]
        [MaxLength(MinLengthName)]
        public string Name { get; set; }

        [MinLength(MinLengthDescription)]
        [MaxLength(MaxLengthDescription)]
        public string Description { get; set; }

        public byte? Floor { get; set; }

        public double? Size { get; set; }

        public int PropertyTypeId { get; set; }

        [Display(Name = "Property Type")]
		public IEnumerable<PropertyTypeInputModel> PropertyTypes { get; set; }

        [Display(Name = "District Name")]
        [MinLength(3)]
        [MaxLength(50)]
        public string DistrictName { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public IEnumerable<CityInputModel> Cities { get; set; }

        [Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
		[ModelBinder(BinderType = typeof(DecimalModelBinder))]
		[Display(Name = "Monthly Price")]
        public decimal PricePerMonth { get; set; }

        public List<int> TagIds { get; set; }

        public IEnumerable<TagInputModel> Tags { get; set; }
    }
}