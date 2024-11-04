namespace RentSmart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.Tag;

    public class Tag : BaseDeletableModel<int>
    {
        public Tag()
        {
            this.Properties = new HashSet<PropertyTag>();
        }

        [Required]
        [MaxLength(MaxLengthName)]
        public string Name { get; set; } = null!;

        public virtual ICollection<PropertyTag> Properties { get; set; }
    }
}
