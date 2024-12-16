namespace RentSmart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RentSmart.Data.Models;

    internal class AppointmentSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Appointments.Any())
            {
                return;
            }

            var random = new Random();

            // seed 100 future appointments
            for (int i = 0; i < 100; i++)
            {
                var renter = dbContext.Renters.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                var property = dbContext.Properties.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                var appointment = new Appointment
                {
                    UserId = renter.UserId,
                    Property = property,
                    DateTime = DateTime.UtcNow.AddDays(random.Next(8, 28)),
                    ManagerId = property.ManagerId,
                };
                dbContext.Appointments.Add(appointment);
            }

            // seed 100 past appointments
            for (int i = 0; i < 100; i++)
            {
                var renter = dbContext.Renters.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                var property = dbContext.Properties.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                var appointment = new Appointment
                {
                    UserId = renter.UserId,
                    Property = property,
                    DateTime = DateTime.UtcNow.AddDays(-random.Next(0, 20)),
                    ManagerId = property.ManagerId,
                };
                dbContext.Appointments.Add(appointment);
            }

            // seed past appointments for irrenka_@abv.bg for every property of manager1
            var renterMe = dbContext.Renters.Where(x => x.User.Email == "irrenka_@abv.bg").FirstOrDefault();
            var propertiesManeger1 = dbContext.Properties.Where(x => x.Manager.User.Email == "manager1@rentsmart.com");
            foreach (var property in propertiesManeger1)
            {
                var appointment = new Appointment
                {
                    UserId = renterMe.UserId,
                    Property = property,
                    DateTime = DateTime.UtcNow.AddDays(-random.Next(0, 20)),
                    ManagerId = property.ManagerId,
                };
                dbContext.Appointments.Add(appointment);
                return;
            }
        }
    }
}
