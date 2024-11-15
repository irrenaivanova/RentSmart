namespace RentSmart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.Feedback;

    public class Feedback : BaseDeletableModel<int>
    {
        public Feedback()
        {
            this.ChildFeedbacks = new HashSet<Feedback>();
        }

        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        [MaxLength(MaxLengthText)]
        public string FeedbackText { get; set; } = null!;

        public int RentalId { get; set; }

        public virtual Rental Rental { get; set; } = null!;

        public int? ParentId { get; set; }

        public virtual Feedback? Parent { get; set; }

        public virtual ICollection<Feedback> ChildFeedbacks { get; set; }
    }
}
