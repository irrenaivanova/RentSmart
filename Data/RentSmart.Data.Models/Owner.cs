namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    public class Owner : BaseDeletableModel<string>
    {
        public Owner()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Properties = new HashSet<Property>();
            this.Orders = new HashSet<Order>();
        }

        [Required]
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual IEnumerable<Property> Properties { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }
    }
}
