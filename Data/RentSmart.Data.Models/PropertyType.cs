﻿namespace RentSmart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.PropertyType;

    public class PropertyType : BaseDeletableModel<int>
    {
        public PropertyType()
        {
            this.Properties = new HashSet<Property>();
        }

        [Required]
        [MaxLength(MaxLengthName)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Property> Properties { get; set; }
    }
}