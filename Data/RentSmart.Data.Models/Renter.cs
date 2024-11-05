namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    public class Renter : BaseDeletableModel<string>
    {
        public Renter()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Rentals = new HashSet<Rental>();
            this.LikedProperties = new HashSet<RenterLike>();
        }

        [Required]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public IEnumerable<Rental> Rentals { get; set; }

        public IEnumerable<RenterLike> LikedProperties { get; set; }
    }
}
