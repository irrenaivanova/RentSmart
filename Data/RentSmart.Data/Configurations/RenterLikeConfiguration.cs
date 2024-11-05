namespace RentSmart.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RentSmart.Data.Models;

    public class RenterLikeConfiguration : IEntityTypeConfiguration<RenterLike>
    {
        public void Configure(EntityTypeBuilder<RenterLike> builder)
        {
            builder.HasKey(x => new { x.RenterId, x.PropertyId });

            // builder.HasOne(x => x.Renter).WithMany(x => x.LikedProperties).HasForeignKey(x => x.RenterId).OnDelete(DeleteBehavior.Restrict);

            // builder.HasOne(x => x.Property).WithMany(x => x.Likes).HasForeignKey(x => x.PropertyId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
