namespace RentSmart.Data.Models
{
    public class RenterLike
    {
        public int RenterId { get; set; }

        public virtual Renter Renter { get; set; } = null!;

        public int PropertyId { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}
