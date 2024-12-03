namespace RentSmart.Data.Models
{
    using RentSmart.Data.Common.Models;

    public class ContactForm : BaseModel<int>
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
