namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.Manager;

    public class Manager : BaseDeletableModel<string>
    {
        public Manager()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Properties = new HashSet<Property>();
            this.Appointments = new HashSet<Appointment>();
        }

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        [MaxLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        public IEnumerable<Property> Properties { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
