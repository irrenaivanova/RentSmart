namespace RentSmart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RentSmart.Data.Common.Models;

    using static RentSmart.Common.EntityValidationConstants;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [MaxLength(ExtensionMaxLength)]
        public string? Extension { get; set; }

        [MaxLength(UrlMaxLength)]
        public string? RemoteImageUrl { get; set; }

        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;
    }
}
