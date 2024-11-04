namespace RentSmart.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RentSmart.Data.Models;

    public class PropertyTagConfiguration : IEntityTypeConfiguration<PropertyTag>
    {
        public void Configure(EntityTypeBuilder<PropertyTag> builder)
        {
            builder.HasKey(x => new { x.PropertyId, x.TagId });

            // builder.HasOne(x => x.Tag).WithMany(x => x.Properties).OnDelete(DeleteBehavior.Restrict);

            // builder.HasOne(x => x.Property).WithMany(x => x.Tags).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
