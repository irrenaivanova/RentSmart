namespace RentSmart.Data.Models
{
    public class PropertyTag
    {
        public string PropertyId { get; set; } = null!;

        public virtual Property Property { get; set; } = null!;

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
    }
}
