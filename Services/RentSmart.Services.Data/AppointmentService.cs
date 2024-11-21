namespace RentSmart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    public class AppointmentService : IAppointmentService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Appointment> appointmentService;

        public AppointmentService(
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Appointment> appointmentService)
        {
            this.propertyRepository = propertyRepository;
            this.appointmentService = appointmentService;
        }

        public async Task<List<string>> GetAvailableHoursAsync(string propertyId, string date)
        {
            var appointments = await this.propertyRepository.All()
                            .Include(x => x.Manager.Appointments)
                            .Where(x => x.Id == propertyId)
                            .SelectMany(x => x.Manager.Appointments.Where(a => a.DateTime.Date == DateTime.Parse(date)))
                            .ToListAsync();
            var busyHours = appointments.Select(x => $"{x.DateTime.Hour}:00");
            var dailyHours = new List<string>() { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00" };
            var hours = dailyHours.Where(x => !busyHours.Contains(x)).ToList();
            return hours;
        }

        // TODO : chech if there is such service, are the hours really available,....
        public async Task CreateAppointmentAsync(string propertyId, string date, string time, string userId)
        {
            string dateTimeString = $"{date} {time}";
            DateTime datetime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm", null);

            var managerId = this.propertyRepository.All().Where(x => x.Id == propertyId).FirstOrDefault().ManagerId;
            var newAppointment = new Appointment
            {
                UserId = userId,
                PropertyId = propertyId,
                DateTime = datetime,
                ManagerId = managerId,
            };

            await this.appointmentService.AddAsync(newAppointment);
            await this.appointmentService.SaveChangesAsync();
        }
    }
}
