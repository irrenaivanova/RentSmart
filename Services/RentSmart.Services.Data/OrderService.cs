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

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<Service> serviceRepository,
            IDeletableEntityRepository<Owner> ownersRepository)
        {
            this.orderRepository = orderRepository;
            this.serviceRepository = serviceRepository;
            this.ownersRepository = ownersRepository;
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
                owner = new Owner() { UserId = userId };
                await this.ownersRepository.AddAsync(owner);
                await this.ownersRepository.SaveChangesAsync();
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
