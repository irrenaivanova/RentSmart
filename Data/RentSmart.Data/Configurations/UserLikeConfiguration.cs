namespace RentSmart.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RentSmart.Data.Models;

    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasQueryFilter(x => !x.Property.IsDeleted);
        }
    }
}
