namespace RentSmart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using MockQueryable;
    using Moq;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Web.ViewModels.Properties.InputModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels.Enums;
    using Xunit;

    public class PropertyServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Property>> mockPropertyRepository;
        private readonly Mock<IDeletableEntityRepository<District>> mockDistrictRepository;
        private readonly Mock<IDeletableEntityRepository<Tag>> mockTagRepository;
        private readonly Mock<IDeletableEntityRepository<Manager>> mockManagerRepository;
        private readonly Mock<IDeletableEntityRepository<Rental>> mockRentalRepository;
        private readonly Mock<IOrderService> mockOrderService;

        private readonly PropertyService propertyService;

        public PropertyServiceTests()
        {
            // Mock the repositories and services
            this.mockPropertyRepository = new Mock<IDeletableEntityRepository<Property>>();
            this.mockDistrictRepository = new Mock<IDeletableEntityRepository<District>>();
            this.mockTagRepository = new Mock<IDeletableEntityRepository<Tag>>();
            this.mockManagerRepository = new Mock<IDeletableEntityRepository<Manager>>();
            this.mockRentalRepository = new Mock<IDeletableEntityRepository<Rental>>();
            this.mockOrderService = new Mock<IOrderService>();

            // Initialize the service
            this.propertyService = new PropertyService(
                this.mockPropertyRepository.Object,
                this.mockDistrictRepository.Object,
                this.mockTagRepository.Object,
                this.mockManagerRepository.Object,
                this.mockRentalRepository.Object,
                this.mockOrderService.Object);
        }

        [Fact]
        public async Task AddAsyncShouldThrowExceptionIfCustomTagsExceedLimit()
        {
            var input = new AddPropertyInputModel
            {
                Description = "A beautiful property",
                Floor = 3,
                Size = 150,
                PropertyTypeId = 1,
                CityId = 1,
                OwnerId = "owner123",
                PricePerMonth = 1000,
                DistrictName = "Banishora",
                TagIds = new List<int> { 1, 2 },
                CustomTags = new List<string> { "Lift", "Furnished", "Cabling", "Cleaning" },
                Images = new List<IFormFile>(),
            };

            var exception = await Assert.ThrowsAsync<Exception>(
                async () => await this.propertyService.AddAsync(input, "user123", "/path/to/images"));

            Assert.Equal("You have reached the maximum limit of 3 customtags!", exception.Message);
        }

        [Fact]
        public async Task AddAsyncShouldThrowExceptionIfImagesExceedLimit()
        {
            var input = new AddPropertyInputModel
            {
                Description = "A beautiful property",
                Floor = 3,
                Size = 150,
                PropertyTypeId = 1,
                CityId = 1,
                OwnerId = "owner123",
                PricePerMonth = 1000,
                DistrictName = "Banishora",
                TagIds = new List<int> { 1, 2 },
                CustomTags = new List<string> { "Lift", "Cleaning" },
                Images = new List<IFormFile>
            {
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object,
            },
            };

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                async () => await this.propertyService.AddAsync(input, "user123", "/path/to/images"));
        }

        [Fact]
        public void AveragePropertyRatingShouldReturnZeroWhenNoRentals()
        {
            var propertyId = "1";
            var mockProperties = new List<Property>
        {
            new Property
            {
                Id = propertyId,
                Rentals = new List<Rental>(),
            },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(mockProperties.AsQueryable().BuildMock());

            var result = this.propertyService.AveragePropertyRating(propertyId);
            Assert.Equal(0, result);
        }

        [Fact]
        public void IsPropertyAvailableShouldReturnTrueWhenNoRentals()
        {
            var propertyId = "1";
            var mockProperties = new List<Property>
        {
            new Property
            {
                Id = propertyId,
                Rentals = new List<Rental>(),
            },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(mockProperties.AsQueryable().BuildMock());

            var result = this.propertyService.IsPropertyAvailable(propertyId);
            Assert.True(result);
        }

        [Fact]
        public void IsPropertyAvailableShouldReturnTrueWhenLastRentalHasEnded()
        {
            var propertyId = "1";
            var mockProperties = new List<Property>
        {
            new Property
            {
                Id = propertyId,
                Rentals = new List<Rental>
                {
                    new Rental
                    {
                        RentDate = DateTime.UtcNow.AddMonths(-3),
                        DurationInMonths = 3,
                    },
                },
            },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(mockProperties.AsQueryable().BuildMock());
            var result = this.propertyService.IsPropertyAvailable(propertyId);
            Assert.True(result);
        }

        [Fact]
        public void IsPropertyAvailableShouldReturnFalseWhenLastRentalIsOngoing()
        {

            var propertyId = "1";
            var mockProperties = new List<Property>
        {
            new Property
            {
                Id = propertyId,
                Rentals = new List<Rental>
                {
                    new Rental
                    {
                        RentDate = DateTime.UtcNow.AddMonths(-1),
                        DurationInMonths = 6,
                    },
                },
            },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(mockProperties.AsQueryable().BuildMock());

            var result = this.propertyService.IsPropertyAvailable(propertyId);
            Assert.False(result);
        }

        [Fact]
        public async Task PropertyCurrentRenterIdShouldReturnNullWhenRentalIsInactive()
        {
            var propertyId = "property123";

            var rentals = new List<Rental>
        {
            new Rental
            {
                PropertyId = propertyId,
                RentDate = DateTime.UtcNow.AddDays(-1),
                Renter = new Renter
                {
                    UserId = "inactiveRenter",
                },
            },
        };

            this.mockRentalRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(rentals.AsQueryable().BuildMock());

            var result = await this.propertyService.PropertyCurrentRenterId(propertyId);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeletePropertyWhenPropertyExists()
        {
            var propertyId = "property123";
            var property = new Property { Id = propertyId };
            this.mockPropertyRepository
                .Setup(repo => repo.All())
                .Returns(new List<Property> { property }.AsQueryable().BuildMock());

            await this.propertyService.DeleteAsync(propertyId);

            this.mockPropertyRepository.Verify(repo => repo.Delete(It.Is<Property>(p => p.Id == propertyId)), Times.Once);
            this.mockPropertyRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldThrowExceptionWhenPropertyDoesNotExist()
        {
            var propertyId = "nonexistentProperty";
            this.mockPropertyRepository
                .Setup(repo => repo.All())
                .Returns(Enumerable.Empty<Property>().AsQueryable().BuildMock());

            var exception = await Assert.ThrowsAsync<Exception>(() => this.propertyService.DeleteAsync(propertyId));
            Assert.Equal("You are trying to delete an Nonexisting Property!", exception.Message);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdatePropertyWhenValidInputProvided()
        {
            var propertyId = "property123";
            var input = new EditPropertyInputModel
            {
                Id = propertyId,
                PricePerMonth = 1000,
                CityId = 1,
                Description = "Updated description",
                Floor = 3,
                Size = 150,
                PropertyTypeId = 1,
                DistrictName = "Banishora",
            };

            var existingProperty = new Property { Id = propertyId };
            var district = new District { Name = "Banishora" };

            this.mockPropertyRepository
                .Setup(repo => repo.All())
                .Returns(new List<Property> { existingProperty }.AsQueryable().BuildMock());
            this.mockDistrictRepository
                .Setup(repo => repo.All())
                .Returns(new List<District> { district }.AsQueryable().BuildMock());
            await this.propertyService.UpdateAsync(input);

            Assert.Equal(input.PricePerMonth, existingProperty.PricePerMonth);
            Assert.Equal(input.CityId, existingProperty.CityId);
            Assert.Equal(input.Description, existingProperty.Description);
            Assert.Equal(input.Floor, existingProperty.Floor);
            Assert.Equal(input.Size, existingProperty.Size);
            Assert.Equal(input.PropertyTypeId, existingProperty.PropertyTypeId);
            Assert.Equal(input.DistrictName, existingProperty.District.Name);

            this.mockPropertyRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void GetPropertyLikesCountShouldReturnLikesCount()
        {
            var propertyId = "property123";
            var mockProperties = new List<Property>
        {
            new Property
            {
                Id = propertyId,
                Likes = new List<UserLike> { new UserLike(), new UserLike(), new UserLike() },
            },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(mockProperties.AsQueryable().BuildMock());

            var result = this.propertyService.GetPropertyLikesCount(propertyId);

            Assert.Equal(3, result);
        }

        [Fact]
        public void GetManagerUserIdShouldReturnManagerUserId()
        {
            var propertyId = "property123";
            var mockProperties = new List<Property>
        {
            new Property
            {
                Id = propertyId,
                Manager = new Manager { UserId = "manager123" },
            },
        };

            this.mockPropertyRepository
                .Setup(repo => repo.AllAsNoTracking())
                .Returns(mockProperties.AsQueryable().BuildMock());

            var result = this.propertyService.GetManagerUserId(propertyId);

            Assert.Equal("manager123", result);
        }
    }
}
