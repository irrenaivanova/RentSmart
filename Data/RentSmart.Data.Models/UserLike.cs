namespace RentSmart.Data.Models
{
    using RentSmart.Data.Common.Models;

    public class UserLike : BaseModel<int>
    {
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;
    }
}
