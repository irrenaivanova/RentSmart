namespace RentSmart.Services.Data
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
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
                return order.DateOfBuying.AddMonths(12) >= DateTime.UtcNow;
            }

            return false;
        }
    }
}
