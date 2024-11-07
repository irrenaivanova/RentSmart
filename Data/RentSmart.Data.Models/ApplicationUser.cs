﻿// ReSharper disable VirtualMemberCallInConstructor
namespace RentSmart.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using RentSmart.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Feedbacks = new HashSet<Feedback>();
            this.LikedProperties = new HashSet<RenterLike>();
            this.Appointments = new HashSet<Appointment>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual Renter? Renter { get; set; }

        public virtual Owner? Owner { get; set; }

        public virtual Manager? Manager { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual IEnumerable<RenterLike> LikedProperties { get; set; }

        public virtual IEnumerable<Appointment> Appointments { get; set; }
    }
}
