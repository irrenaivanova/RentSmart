namespace RentSmart.Services.Data.Tests
{
    using MockQueryable;
    using Moq;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class AppointmentServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Property>> mockPropertyRepository;
        private readonly Mock<IDeletableEntityRepository<Appointment>> mockAppointmentService;
        private readonly AppointmentService appointmentService;

        public AppointmentServiceTests()
        {
            this.mockPropertyRepository = new Mock<IDeletableEntityRepository<Property>>();
            this.mockAppointmentService = new Mock<IDeletableEntityRepository<Appointment>>();
            this.appointmentService = new AppointmentService(this.mockPropertyRepository.Object, this.mockAppointmentService.Object);
        }

        [Fact]
        public async Task GetAvailableHoursAsyncShouldReturnAvailableHoursWhenValidDateIsGiven()
        {
            var propertyId = "property123";
            var date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            var property = new Property
            {
                Id = propertyId,
                Manager = new Manager { Appointments = new List<Appointment> {
                    new Appointment { DateTime = DateTime.Parse($"{date} 10:00"), UserId = "user123", PropertyId = propertyId, },
                },
                },
            };

            var mockQueryableProperty = new List<Property> { property }.AsQueryable().BuildMock();
            this.mockPropertyRepository.Setup(x => x.All()).Returns(mockQueryableProperty);
            var availableHours = await this.appointmentService.GetAvailableHoursAsync(propertyId, date);
            Assert.DoesNotContain("10:00", availableHours);
            Assert.Contains("11:00", availableHours);
            Assert.Contains("12:00", availableHours);
        }

        [Fact]
        public async Task GetAvailableHoursAsyncShouldAddPastHoursWhenTodayIsGiven()
        {
            var propertyId = "property123";
            var date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            var property = new Property
            {
                Id = propertyId,
                Manager = new Manager
                {
                    Appointments = new List<Appointment>(),
                },
            };

            var mockQueryableProperty = new List<Property> { property }.AsQueryable().BuildMock();
            this.mockPropertyRepository.Setup(x => x.All()).Returns(mockQueryableProperty);

            var currentDate = DateTime.Now;
            var busyHours = new List<string>();

            if (currentDate.Date == DateTime.Parse(date).Date)
            {
                var hourNow = currentDate.Hour;
                for (int i = 10; i <= 16; i++)
                {
                    if (i <= hourNow)
                    {
                        busyHours.Add($"{i}:00");
                    }
                }
            }

            var availableHours = await this.appointmentService.GetAvailableHoursAsync(propertyId, date);

            foreach (var busyHour in busyHours)
            {
                Assert.Contains(busyHour, availableHours);
            }
        }

        [Fact]
        public async Task CreateAppointmentAsyncShouldNotCreateAppointmentWhenPropertyNotFound()
        {
            var propertyId = string.Empty;
            var date = "2024-12-17";
            var time = "14:00";
            var userId = "user123";

            var mockQueryableProperty = new List<Property>().AsQueryable().BuildMock();
            this.mockPropertyRepository.Setup(x => x.All()).Returns(mockQueryableProperty);

            await this.appointmentService.CreateAppointmentAsync(propertyId, date, time, userId);

            this.mockAppointmentService.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
            this.mockAppointmentService.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
