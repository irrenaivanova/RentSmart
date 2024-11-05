namespace RentSmart.Data.Models
{
    using System;

    using RentSmart.Data.Common.Models;

    public class Appointment : BaseModel<int>
    {
        public string RenterId { get; set; } = null!;

        public virtual Renter Renter { get; set; } = null!;

        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public string ManagerId { get; set; } = null!;

        public virtual Manager Manager { get; set; } = null!;
    }
}
