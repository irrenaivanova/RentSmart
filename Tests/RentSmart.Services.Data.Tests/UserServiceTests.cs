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

    public class UserServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Owner>> mockOwnerRepository;
        private readonly Mock<IDeletableEntityRepository<ApplicationUser>> mockUserRepository;
        private readonly Mock<IDeletableEntityRepository<Renter>> mockRenterRepository;
        private readonly Mock<IDeletableEntityRepository<Rental>> mockRentalRepository;
        private readonly Mock<IDeletableEntityRepository<Manager>> mockManagerRepository;

        private readonly UserService userService;

        public UserServiceTests()
        {
            // Initialize mocks
            this.mockOwnerRepository = new Mock<IDeletableEntityRepository<Owner>>();
            this.mockUserRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.mockRenterRepository = new Mock<IDeletableEntityRepository<Renter>>();
            this.mockRentalRepository = new Mock<IDeletableEntityRepository<Rental>>();
            this.mockManagerRepository = new Mock<IDeletableEntityRepository<Manager>>();

            // Create UserService instance with mocked dependencies
            this.userService = new UserService(
                this.mockOwnerRepository.Object,
                this.mockUserRepository.Object,
                this.mockRenterRepository.Object,
                this.mockRentalRepository.Object,
                this.mockManagerRepository.Object
            );
        }

        [Fact]
        public async Task GetTheRenterByUserIdShouldReturnRenterIdWhenRenterExists()
        {
            var userId = "user123";
            var existingRenter = new Renter { Id = "renter123", UserId = userId };
            var mockRenterQuery = new List<Renter> { existingRenter }.AsQueryable().BuildMock();
            this.mockRenterRepository.Setup(x => x.All()).Returns(mockRenterQuery);
            var renterId = await this.userService.GetTheRenterByUserId(userId);
            Assert.Equal("renter123", renterId);
        }

        [Fact]
        public async Task GetTheRenterByUserIdShouldCreateRenterWhenRenterDoesNotExist()
        {
            var userId = "user123";
            var newRenter = new Renter { UserId = userId };
            var mockRenterQuery = new List<Renter>().AsQueryable().BuildMock();
            this.mockRenterRepository.Setup(x => x.All()).Returns(mockRenterQuery);
            this.mockRenterRepository.Setup(x => x.AddAsync(It.IsAny<Renter>())).Returns(Task.CompletedTask);
            var renterId = await this.userService.GetTheRenterByUserId(userId);

            this.mockRenterRepository.Verify(x => x.AddAsync(It.IsAny<Renter>()), Times.Once);
            this.mockRenterRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.NotNull(renterId);
        }

        [Fact]
        public async Task IsThisUserPassedRentalOfThePropertyShouldReturnTrueIfRentalIsPastDue()
        {
            var userId = "user123";
            var propertyId = "property123";
            var renter = new Renter { Id = "renter123", UserId = userId };
            var rental = new Rental
            {
                RentDate = DateTime.UtcNow.AddMonths(-6),
                DurationInMonths = 5,
                RenterId = renter.Id,
                PropertyId = propertyId,
            };

            var mockRenterQuery = new List<Renter> { renter }.AsQueryable().BuildMock();
            var mockRentalQuery = new List<Rental> { rental }.AsQueryable().BuildMock();

            this.mockRenterRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRenterQuery);
            this.mockRentalRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRentalQuery);
            var result = await this.userService.IsThisUserPassedRentalOfTheProperty(userId, propertyId);
            Assert.True(result);
        }

        [Fact]
        public async Task IsThisUserPassedRentalOfThePropertyShouldReturnFalseIfRentalIsNotPastDue()
        {
            var userId = "user123";
            var propertyId = "property123";
            var renter = new Renter { Id = "renter123", UserId = userId };
            var rental = new Rental
            {
                RentDate = DateTime.UtcNow.AddMonths(-6),
                DurationInMonths = 12,
                RenterId = renter.Id,
                PropertyId = propertyId,
            };

            var mockRenterQuery = new List<Renter> { renter }.AsQueryable().BuildMock();
            var mockRentalQuery = new List<Rental> { rental }.AsQueryable().BuildMock();

            this.mockRenterRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRenterQuery);
            this.mockRentalRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRentalQuery);

            var result = await this.userService.IsThisUserPassedRentalOfTheProperty(userId, propertyId);
            Assert.False(result);
        }

        [Fact]
        public async Task GetRentalIdShouldThrowExceptionIfRatingExists()
        {
            var userId = "user123";
            var propertyId = "property123";
            var renter = new Renter { Id = "renter123", UserId = userId };
            var rental = new Rental
            {
                RentDate = DateTime.UtcNow.AddMonths(-6),
                DurationInMonths = 12,
                RenterId = renter.Id,
                PropertyId = propertyId,
                RatingId = 1,
            };

            var mockRenterQuery = new List<Renter> { renter }.AsQueryable().BuildMock();
            var mockRentalQuery = new List<Rental> { rental }.AsQueryable().BuildMock();

            this.mockRenterRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRenterQuery);
            this.mockRentalRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRentalQuery);

            var exception = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.GetRentalId(userId, propertyId)
            );

            Assert.Equal("You can rate the property only once!", exception.Message);
        }

        [Fact]
        public async Task GetRentalIdShouldThrowExceptionIfContractIsNotExpired()
        {
            var userId = "user123";
            var propertyId = "property123";
            var renter = new Renter { Id = "renter123", UserId = userId };
            var rental = new Rental
            {
                RentDate = DateTime.UtcNow.AddMonths(-1),
                DurationInMonths = 12,
                RenterId = renter.Id,
                PropertyId = propertyId,
            };

            var mockRenterQuery = new List<Renter> { renter }.AsQueryable().BuildMock();
            var mockRentalQuery = new List<Rental> { rental }.AsQueryable().BuildMock();

            this.mockRenterRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRenterQuery);
            this.mockRentalRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRentalQuery);

            var exception = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.GetRentalId(userId, propertyId)
            );

            Assert.Equal("You can rate the property only after the contract has ended!", exception.Message);
        }

        [Fact]
        public async Task GetRentalIdShouldReturnRentalIdWhenValid()
        {
            var userId = "user123";
            var propertyId = "property123";
            var renter = new Renter { Id = "renter123", UserId = userId };
            var rental = new Rental
            {
                RentDate = DateTime.UtcNow.AddMonths(-13), 
                DurationInMonths = 12,
                RenterId = renter.Id,
                PropertyId = propertyId,
            };

            var mockRenterQuery = new List<Renter> { renter }.AsQueryable().BuildMock();
            var mockRentalQuery = new List<Rental> { rental }.AsQueryable().BuildMock();

            this.mockRenterRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRenterQuery);
            this.mockRentalRepository.Setup(x => x.AllAsNoTracking()).Returns(mockRentalQuery);

            var rentalId = await this.userService.GetRentalId(userId, propertyId);

            Assert.Equal(rental.Id, rentalId);
        }
    }
}
