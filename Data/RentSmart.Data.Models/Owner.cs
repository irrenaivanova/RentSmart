namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;

    using RentSmart.Data.Common.Models;

    public class Owner : BaseDeletableModel<string>
    {
        public Owner()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Properties = new HashSet<Property>();
            this.Orders = new HashSet<Order>();
        }

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public IEnumerable<Property> Properties { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
