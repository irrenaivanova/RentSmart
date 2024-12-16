namespace RentSmart.Services
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Messaging;

    using static RentSmart.Common.GlobalConstants;

    public class RentalNotificationService
    {
        private readonly IDeletableEntityRepository<Rental> rentalsRepository;
        private readonly IEmailSender sender;

        public RentalNotificationService(
           IDeletableEntityRepository<Rental> rentalsRepository,
            IEmailSender sender)
        {
            this.rentalsRepository = rentalsRepository;
            this.sender = sender;
        }

        public async Task NotifyExpiringRentals()
        {
            var today = DateTime.UtcNow.Date;
            var notificationDate = today.AddDays(3);
            var exp = this.rentalsRepository.All().Where(x => x.RentDate.AddMonths(x.DurationInMonths) == notificationDate)
                .Include(x => x.Renter).ThenInclude(x => x.User)
                .Include(x => x.Property)
                .ThenInclude(x => x.PropertyType)
                .Include(x => x.Property)
                .ThenInclude(x => x.District)
                .ToList();

            foreach (var rental in exp)
            {
                var subject = $"RentalContract {rental.Id} is expiring";
                var html = new StringBuilder();
                html.AppendLine($"<h1>Notification about RentalContract {rental.Id} / {rental.RentDate.ToString("d")}</h1>");
                html.AppendLine($"<h3>Dear {rental.Renter.User.FirstName} {rental.Renter.User.LastName},</h3>");
                html.AppendLine($"<p>Your contract for property {rental.Property.PropertyType.Name}," +
                    $" located in {rental.Property.District.Name} is going to expired in three days</p>");

                await this.sender.SendEmailAsync(SystemEmailSender, "RentSmart", rental.Renter.User.Email, subject, html.ToString());
            }

            // The mapping does not work ?

            // var expiringRentals = this.rentalsRepository.All()
            //     .Where(x => x.RentDate.AddMonths(x.DurationInMonths) == notificationDate)
            //    .To<RentDto>()
            //    .ToList();

            // foreach (var rental in expiringRentals)
            // {
            //    var subject = $"RentalContract {rental.Id} / {rental.RentDate}";
            //    var html = new StringBuilder();
            //    html.AppendLine($"<h1>Notification about RentalContract {rental.Id} / {rental.RentDate}</h1>");
            //    html.AppendLine($"<h3>Dear {rental.RenterUserName}</h3>");
            //    html.AppendLine($"<p>Your contract for property {rental.PropertyPropertyTypeName}," +
            //        $" located in {rental.PropertyDistrictName} in {rental.PropertyCityName} is going to expired in three days</p>");

            // await this.sender.SendEmailAsync(SystemEmailSender, "RentSmart", rental.RenterUserEmail, subject, html.ToString());
            // }
        }
    }
}
