namespace RentSmart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants;

    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [MaxLength(ExtensionMaxLength)]
        public string? Extension { get; set; }

        [MaxLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        public int PropertyId { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}
