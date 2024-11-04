namespace RentSmart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.Property;

    public class Property : BaseDeletableModel<int>
    {
        public Property()
        {
            this.Tags = new HashSet<PropertyTag>();
            this.Likes = new HashSet<RenterLike>();
            this.Rentals = new HashSet<Rental>();
            this.Images = new HashSet<Image>();
        }

        [MaxLength(MaxLengthName)]
        public string? Name { get; set; }

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

        public int OwnerId { get; set; }

        public virtual Owner Owner { get; set; } = null!;

        public int ManagerId { get; set; }

        public virtual Manager Manager { get; set; } = null!;

        public decimal PricePerMonth { get; set; }

        public virtual ICollection<PropertyTag> Tags { get; set; }

        public virtual ICollection<RenterLike> Likes { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
