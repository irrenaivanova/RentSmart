namespace RentSmart.Data.Models
{
    public class PropertyTag
    {
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; } = null!;

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
    }
}
