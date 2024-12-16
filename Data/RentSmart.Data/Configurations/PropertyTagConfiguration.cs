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

            builder.HasOne(x => x.Tag).WithMany(x => x.Properties).HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Property).WithMany(x => x.Tags).HasForeignKey(x => x.PropertyId).OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.Property.IsDeleted);
        }
    }
}
