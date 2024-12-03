namespace RentSmart.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;
    using RentSmart.Web.Infrastructure.Attributes;

    public class ContactFormViewModel
    {
        [Required]
        [Display(Name = "Your name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(50)]
        [MaxLength(1000)]
        public string Content { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
