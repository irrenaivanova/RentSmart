namespace RentSmart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using RentSmart.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        public DateTime DateOfBuying { get; set; }

        public string OwnerId { get; set; } = null!;

        public Owner Owner { get; set; } = null!;

        public int ServiceId { get; set; }

        public Service Service { get; set; } = null!;

        public int? PropertyId { get; set; }

        public Property? Property { get; set; }
    }
}
