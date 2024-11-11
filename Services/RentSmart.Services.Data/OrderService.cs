namespace RentSmart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    using Service = RentSmart.Data.Models.Service;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly IDeletableEntityRepository<Service> serviceRepository;
        private readonly IDeletableEntityRepository<Owner> ownersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<Service> serviceRepository,
            IDeletableEntityRepository<Owner> ownersRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.orderRepository = orderRepository;
            this.serviceRepository = serviceRepository;
            this.ownersRepository = ownersRepository;
            this.userRepository = userRepository;
        }

        public async Task AddNewOrderAsync(int serviceId, string userId)
        {
            var service = await this.serviceRepository.All().FirstOrDefaultAsync(x => x.Id == serviceId);

            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {serviceId} not found.");
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
    }
}
