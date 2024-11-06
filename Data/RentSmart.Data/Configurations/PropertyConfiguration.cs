namespace RentSmart.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RentSmart.Data.Models;

    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.Property(x => x.PricePerMonth).HasColumnType("decimal(8,2)");
        }
    }
}
