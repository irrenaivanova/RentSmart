namespace RentSmart.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MockQueryable;
    using Moq;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using Xunit;

    public class RentServiceTests
    {
        private readonly Mock<IPropertyService> mockPropertyService;
        private readonly Mock<IUserService> mockUserService;
        private readonly Mock<IDeletableEntityRepository<Rental>> mockRentalRepository;
        private readonly RentService rentService;

        public RentServiceTests()
        {
            this.mockPropertyService = new Mock<IPropertyService>();
            this.mockUserService = new Mock<IUserService>();
            this.mockRentalRepository = new Mock<IDeletableEntityRepository<Rental>>();

            this.rentService = new RentService(this.mockPropertyService.Object, this.mockUserService.Object, this.mockRentalRepository.Object);
        }

        [Fact]
        public async Task AddRatingAsyncShouldAddRatingWhenRentalExistsAndRatingIsNull()
        {
            var rentalId = 1;
            var conditionAndMaintenanceRate = 4;
            var location = 5;
            var valueForMoney = 3;

            var rental = new Rental { Id = rentalId, RatingId = null };
            var rentals = new List<Rental> { rental };

            var mockQueryable = rentals.AsQueryable().BuildMock();
            this.mockRentalRepository.Setup(x => x.All()).Returns(mockQueryable);
            this.mockRentalRepository.Setup(x => x.SaveChangesAsync());

            await this.rentService.AddRatingAsync(rentalId, conditionAndMaintenanceRate, location, valueForMoney);

            Assert.NotNull(rental.Rating);
            Assert.Equal(conditionAndMaintenanceRate, rental.Rating.ConditionAndMaintenanceRate);
            Assert.Equal(location, rental.Rating.Location);
            Assert.Equal(valueForMoney, rental.Rating.ValueForMoney);

            this.mockRentalRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddRatingAsyncShouldDoNothingWhenRentalDoesNotExist()
        {
            var rentalId = 1;
            var rentals = new List<Rental>();
            var mockQueryable = rentals.AsQueryable().BuildMock();
            this.mockRentalRepository.Setup(x => x.All()).Returns(mockQueryable);
            await this.rentService.AddRatingAsync(rentalId, 4, 5, 3);
            this.mockRentalRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddRatingAsyncShouldDoNothingWhenRentalAlreadyHasRating()
        {
            var rentalId = 1;
            var rental = new Rental { Id = rentalId, RatingId = 123 };
            var rentals = new List<Rental> { rental };
            var mockQueryable = rentals.AsQueryable().BuildMock();
            this.mockRentalRepository.Setup(x => x.All()).Returns(mockQueryable);

            await this.rentService.AddRatingAsync(rentalId, 4, 5, 3);
            Assert.Null(rental.Rating);
            this.mockRentalRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
