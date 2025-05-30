using order_service.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace order_service.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<Order> CreateAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(Guid orderId);
    }

    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<Guid, Order> _orders = new();

        public Task<IEnumerable<Order>> GetAllAsync() =>
            Task.FromResult(_orders.Values.AsEnumerable());

        public Task<Order?> GetByIdAsync(Guid orderId) =>
            Task.FromResult(_orders.TryGetValue(orderId, out var order) ? order : null);

        public Task<Order> CreateAsync(Order order)
        {
            order.OrderId = Guid.NewGuid();
            _orders[order.OrderId] = order;
            return Task.FromResult(order);
        }

        public Task<bool> UpdateAsync(Order order)
        {
            if (!_orders.ContainsKey(order.OrderId))
                return Task.FromResult(false);
            _orders[order.OrderId] = order;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid orderId)
        {
            return Task.FromResult(_orders.TryRemove(orderId, out _));
        }
    }
}
