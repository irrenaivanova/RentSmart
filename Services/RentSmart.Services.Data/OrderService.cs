namespace RentSmart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;

    using Service = RentSmart.Data.Models.Service;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly IDeletableEntityRepository<Service> serviceRepository;
        private readonly IDeletableEntityRepository<Owner> ownersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Property> propertyRepository;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<Service> serviceRepository,
            IDeletableEntityRepository<Owner> ownersRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Property> propertyRepository)
        {
            this.orderRepository = orderRepository;
            this.serviceRepository = serviceRepository;
            this.ownersRepository = ownersRepository;
            this.userRepository = userRepository;
            this.propertyRepository = propertyRepository;
        }

        public async Task AddNewOrderAsync(int serviceId, string userId)
        {
            var service = await this.serviceRepository.All().FirstOrDefaultAsync(x => x.Id == serviceId);

            if (service == null)
            {
                throw new KeyNotFoundException($"This service is unavailable for buying!");
            }

            var owner = this.ownersRepository.All().FirstOrDefault(x => x.UserId == userId);

            if (owner == null)
            {
                var user = this.userRepository.All().FirstOrDefault(x => x.Id == userId);
                owner = new Owner() { UserId = user.Id, User = user };
                user.Owner = owner;
                await this.ownersRepository.AddAsync(owner);
                await this.ownersRepository.SaveChangesAsync();
                await this.userRepository.SaveChangesAsync();
            }

            var order = new Order() { OwnerId = owner.Id, ServiceId = serviceId };
            await this.orderRepository.AddAsync(order);
            await this.orderRepository.SaveChangesAsync();
        }

        public async Task UsingActiveOrder(string ownerId, string propertyId)
        {
            var order = await this.HasTheOwnerRightOrderAsync(ownerId);
            var property = await this.propertyRepository.All().Where(x => x.Id == propertyId).FirstOrDefaultAsync();
            order.Property = property;
            await this.orderRepository.SaveChangesAsync();
        }

        public bool IsActive(int id)
        {
            var order = this.orderRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.Service)
                .FirstOrDefault();
            if (order.Service.Duration == "One Time")
            {
                return order.PropertyId == null;
            }
            else if (order.Service.Duration == "One Year")
            {
                return order.CreatedOn.AddMonths(12) >= DateTime.UtcNow;
            }

            return false;
        }

        public List<T> GetAllAsync<T>()
        {
            return this.serviceRepository.AllAsNoTracking().To<T>().ToList();
        }

        private async Task<Order> HasTheOwnerRightOrderAsync(string ownerId)
        {
            var order = await this.orderRepository.All()
                .Where(x => x.OwnerId == ownerId)
                .Where(x => x.Service.Duration == "One Time" && x.Property == null)
                .Include(x => x.Property)
                .FirstOrDefaultAsync();
            if (order == null)
            {
                throw new Exception("This user has not active proper service for registration a property!");
            }

            return order;
        }
    }
}
