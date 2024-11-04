namespace RentSmart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants.District;

    public class District : BaseDeletableModel<int>
    {
        public District()
        {
            this.Properties = new HashSet<Property>();
        }

        [Required]
        [MaxLength(MaxLengthName)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Property> Properties { get; set; }
    }
}
