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

    public class OrderServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Order>> mockOrderRepository;
        private readonly Mock<IDeletableEntityRepository<Service>> mockServiceRepository;
        private readonly Mock<IDeletableEntityRepository<Owner>> mockOwnersRepository;
        private readonly Mock<IDeletableEntityRepository<ApplicationUser>> mockUserRepository;
        private readonly Mock<IDeletableEntityRepository<Property>> mockPropertyRepository;
        private readonly OrderService orderService;

        public OrderServiceTests()
        {
            this.mockOrderRepository = new Mock<IDeletableEntityRepository<Order>>();
            this.mockServiceRepository = new Mock<IDeletableEntityRepository<Service>>();
            this.mockOwnersRepository = new Mock<IDeletableEntityRepository<Owner>>();
            this.mockUserRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.mockPropertyRepository = new Mock<IDeletableEntityRepository<Property>>();

            this.orderService = new OrderService(
                this.mockOrderRepository.Object,
                this.mockServiceRepository.Object,
                this.mockOwnersRepository.Object,
                this.mockUserRepository.Object,
                this.mockPropertyRepository.Object);
        }

        [Fact]
        public async Task AddNewOrderAsyncShouldThrowExceptionWhenServiceNotFound()
        {
            var serviceId = 1;
            var userId = "user123";
            this.mockServiceRepository.Setup(x => x.All()).Returns(new List<Service>() { new Service { Id = 2 }}.AsQueryable());
            await Assert.ThrowsAsync<InvalidOperationException>(() => this.orderService.AddNewOrderAsync(serviceId, userId));
        }

        [Fact]
        public async Task AddNewOrderAsyncShouldCreateOwnerWhenOwnerDoesNotExist()
        {
            var serviceId = 1;
            var userId = "user123";
            var service = new Service { Id = serviceId };
            var user = new ApplicationUser { Id = userId };
            this.mockServiceRepository.Setup(x => x.All()).Returns(new List<Service> { service }.AsQueryable().BuildMock());
            this.mockUserRepository.Setup(x => x.All()).Returns(new List<ApplicationUser> { user }.AsQueryable().BuildMock());
            this.mockOwnersRepository.Setup(x => x.AddAsync(It.IsAny<Owner>())).Returns(Task.CompletedTask);
            await this.orderService.AddNewOrderAsync(serviceId, userId);
            this.mockOwnersRepository.Verify(x => x.AddAsync(It.IsAny<Owner>()), Times.Once);
        }

        [Fact]
        public async Task AddNewOrderAsyncShouldAddOrderWhenOwnerExists()
        {
            var serviceId = 1;
            var userId = "user123";
            var service = new Service { Id = serviceId };
            var owner = new Owner { Id = "1", UserId = userId };
            this.mockServiceRepository.Setup(x => x.All()).Returns(new List<Service> { service }.AsQueryable().BuildMock());
            this.mockOwnersRepository.Setup(x => x.All()).Returns(new List<Owner> { owner }.AsQueryable().BuildMock());
            this.mockOrderRepository.Setup(x => x.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            await this.orderService.AddNewOrderAsync(serviceId, userId);
            this.mockOrderRepository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        }
    }
}
