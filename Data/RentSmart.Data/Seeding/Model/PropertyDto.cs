namespace RentSmart.Data.Seeding.Model
{
    using System.Collections.Generic;

    public class PropertyDto
    {
        public PropertyDto()
        {
                this.Tags = new List<string>();
        }

        public string Description { get; set; } = null!;

        public byte Floor { get; set; }

        public double Size { get; set; }

        public string PropertyType { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string District { get; set; } = null!;

        public string City { get; set; } = null!;

        public string OriginalUrl { get; set; } = null!;

        public decimal PricePerMonth { get; set; }

        public IList<string> Tags { get; set; }
    }
}
