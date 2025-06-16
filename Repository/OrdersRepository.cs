using Microsoft.EntityFrameworkCore;
using ProductTask.Data;
using ProductTask.Model;
using System;

namespace ProductTask.Repository
{
    public class OrdersRepository : IOrderRepository
    {
        private readonly DataEF _context;

        public OrdersRepository(DataEF context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.orders.Include(o => o.OrderDetails)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.orders.FindAsync(id);
            if (order != null)
            {
                _context.orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
