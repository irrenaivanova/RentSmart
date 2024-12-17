namespace RentSmart.Services.Data.Tests
{
    using Microsoft.AspNetCore.Http;
    using MockQueryable;
    using Moq;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Web.ViewModels.Properties.InputModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
                this.mockOrderService.Object
            );
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
                async () => await this.propertyService.AddAsync(input, "user123", "/path/to/images")
            );

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
                async () => await this.propertyService.AddAsync(input, "user123", "/path/to/images")
            );
        }
    }
}