namespace RentSmart.Data.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.Service;

    public class Service : BaseDeletableModel<int>
    {
        public Service()
        {
            this.Orders = new HashSet<Order>();
        }

        [Required]
        [MaxLength(MaxLengthName)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(MaxLengthDescription)]
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string Duration { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
