namespace RentSmart.Data.Models
{
    using System;

    using RentSmart.Data.Common.Models;

    public class Appointment : BaseDeletableModel<int>
    {
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;

        public DateTime DateTime { get; set; }
    }
}
