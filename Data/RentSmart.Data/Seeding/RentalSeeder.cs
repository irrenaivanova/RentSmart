namespace RentSmart.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Models;

    internal class RentalSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Rentals.Any())
            {
                return;
            }

            var random = new Random();

            // Seed two "passed contracts" for each second property to simulate completed agreements - needed for proper rating
            var properties = await dbContext.Properties.ToListAsync();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < properties.Count; j += 2)
                {
                    var renter = await dbContext.Renters.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var rental = new Rental
                    {
                        Renter = renter,
                        Property = properties[j],
                        RentDate = DateTime.UtcNow.AddMonths(-2),
                        DurationInMonths = 1,
                        Rating = new Rating
                        {
                            ConditionAndMaintenanceRate = random.Next(3, 6),
                            Location = random.Next(3, 6),
                            ValueForMoney = random.Next(3, 6),
                        },
                    };
                    dbContext.Rentals.Add(rental);
                }
            }

            // Seed one current contract for every renter
            foreach (var renter in dbContext.Renters.ToList())
            {
                var rental = new Rental
                {
                    Renter = renter,
                    Property = properties.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                    RentDate = DateTime.UtcNow.AddMonths(-2),
                    DurationInMonths = 5,
                };
                dbContext.Rentals.Add(rental);
            }

            // Seed contract expiring on 24-12-2024 for demonstration purposes during project defense
            var renterMe = new Renter { User = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == "irrenka_@abv.bg") };
            var rentalMe = new Rental
            {
                Renter = renterMe,
                Property = properties.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                RentDate = new DateTime(2024, 11, 24),
                DurationInMonths = 1,
            };
            dbContext.Rentals.Add(rentalMe);

            // Seed contract expiring on 16-12-2024 for today
            var renterMe2 = new Renter { User = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == "irrenka_@abv.bg") };
            var rentalMe2 = new Rental
            {
                Renter = renterMe,
                Property = properties.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                RentDate = new DateTime(2024, 11, 16),
                DurationInMonths = 1,
            };
            dbContext.Rentals.Add(rentalMe2);
        }
    }
}
