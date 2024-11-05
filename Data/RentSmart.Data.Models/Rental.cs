namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants;

    public class Rental : BaseDeletableModel<int>
    {
        public Rental()
        {
            this.Feedbacks = new HashSet<Feedback>();
        }

        [Required]
        public string RenterId { get; set; } = null!;

        public virtual Renter Renter { get; set; } = null!;

        [Required]
        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;

        public DateTime RentDate { get; set; }

        public int DurationInMonths { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ContractUrl { get; set; } = null!;

        public int? RatingId { get; set; }

        public virtual Rating? Rating { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }

        [NotMapped]
        public bool IsActive => this.RentDate.AddMonths(this.DurationInMonths) >= DateTime.UtcNow;
    }
}
