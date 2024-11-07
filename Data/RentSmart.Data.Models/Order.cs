namespace RentSmart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using RentSmart.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        public string OwnerId { get; set; } = null!;

        public virtual Owner Owner { get; set; } = null!;

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;

        public string? PropertyId { get; set; }

        public virtual Property? Property { get; set; }
    }
}
