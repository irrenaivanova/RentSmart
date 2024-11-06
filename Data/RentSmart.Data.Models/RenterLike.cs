namespace RentSmart.Data.Models
{
    public class RenterLike
    {
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;
    }
}
