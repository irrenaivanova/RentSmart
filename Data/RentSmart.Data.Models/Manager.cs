﻿namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants;

    public class Manager : BaseDeletableModel<string>
    {
        public Manager()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Properties = new HashSet<Property>();
            this.Appointments = new HashSet<Appointment>();
        }

        [Required]
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual IEnumerable<Property> Properties { get; set; }

        public virtual IEnumerable<Appointment> Appointments { get; set; }
    }
}
