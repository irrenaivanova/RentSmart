namespace RentSmart.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RentSmart.Data.Models;

    internal class ServicesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Services.Any())
            {
                return;
            }

            await dbContext.Services.AddAsync(new Service
            {
                Name = "Find My New Tenants",
                Description = "Looking for great tenants? Our Find My New Tenants service has got you covered! " +
                "We’ll help you connect with reliable renters, so your property stays occupied and you can relax.",
                Price = 500m, Duration = "One Time",
            });

            await dbContext.Services.AddAsync(new Service
            {
                Name = "One-Year Maintenance Package",
                Description = "Keep your property in top condition with our One Year Maintenance Package. " +
                "Enjoy peace of mind with comprehensive maintenance services that protect your investment year-round.",
                Price = 1200m,
                Duration = "One Year",
            });
        }
    }
}
