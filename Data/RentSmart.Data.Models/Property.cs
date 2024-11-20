namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.Property;

    public class Property : BaseDeletableModel<string>
    {
        public Property()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Tags = new HashSet<PropertyTag>();
            this.Likes = new HashSet<UserLike>();
            this.Rentals = new HashSet<Rental>();
            this.Images = new HashSet<Image>();
            this.Orders = new HashSet<Order>();
        }

        [MaxLength(MaxLengthDescription)]
        public string? Description { get; set; }

        public byte? Floor { get; set; }

        public double Size { get; set; }

        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; } = null!;

        public int DistrictId { get; set; }

        public virtual District District { get; set; } = null!;

        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;

        public string? OwnerId { get; set; } = null!;

        public virtual Owner? Owner { get; set; } = null!;

        public string? ManagerId { get; set; } = null!;

        public virtual Manager? Manager { get; set; } = null!;

        public string? OriginalUrl { get; set; }

        public decimal PricePerMonth { get; set; }

        public virtual ICollection<PropertyTag> Tags { get; set; }

        public virtual ICollection<UserLike> Likes { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
