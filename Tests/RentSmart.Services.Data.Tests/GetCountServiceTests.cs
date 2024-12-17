namespace RentSmart.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using Xunit;

    public class GetCountServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Property>> mockPropertyRepository;
        private readonly Mock<IDeletableEntityRepository<Owner>> mockOwnerRepository;
        private readonly Mock<IDeletableEntityRepository<Manager>> mockManagerRepository;
        private readonly Mock<IDeletableEntityRepository<Rental>> mockRentalRepository;
        private readonly Mock<IPropertyService> mockPropertyService;
        private readonly GetCountsService getCountsService;

        public GetCountServiceTests()
        {
            this.mockPropertyRepository = new Mock<IDeletableEntityRepository<Property>>();
            this.mockOwnerRepository = new Mock<IDeletableEntityRepository<Owner>>();
            this.mockManagerRepository = new Mock<IDeletableEntityRepository<Manager>>();
            this.mockRentalRepository = new Mock<IDeletableEntityRepository<Rental>>();
            this.mockPropertyService = new Mock<IPropertyService>();

            this.getCountsService = new GetCountsService(
                this.mockPropertyRepository.Object,
                this.mockOwnerRepository.Object,
                this.mockManagerRepository.Object,
                this.mockRentalRepository.Object,
                this.mockPropertyService.Object);
        }

        [Fact]
        public void GetCountsViewModelShouldReturnCorrectCounts()
        {
            this.mockOwnerRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(new List<Owner> { new Owner(), new Owner() }.AsQueryable());

            this.mockManagerRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(new List<Manager> { new Manager() }.AsQueryable());

            this.mockRentalRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(new List<Rental> { new Rental(), new Rental(), new Rental() }.AsQueryable());

            var properties = new List<Property>
        {
            new Property { Id = "1" },
            new Property { Id = "2" },
            new Property { Id = "3" },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(properties.AsQueryable());

            this.mockPropertyService
                .Setup(service => service.IsPropertyAvailable(It.IsAny<string>()))
                .Returns<string>(id => id != "3");

            // Act
            var result = this.getCountsService.GetCountsViewModel();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ManagersCount);
            Assert.Equal(2, result.OwnersCount);
            Assert.Equal(2, result.PropertiesCount);
            Assert.Equal(3, result.RentalsCount);
        }
    }
}
