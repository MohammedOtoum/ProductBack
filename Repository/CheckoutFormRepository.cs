// Repositories/CheckoutFormRepository.cs
using Microsoft.EntityFrameworkCore;
using ProductTask.Data;
using ProductTask.Dto;
using ProductTask.Model;
using System;

namespace ProductTask.Repository
{
    public class CheckoutFormRepository : ICheckoutFormRepository
    {
        private readonly DataEF _context;
        private readonly IOrderRepository _orderRepository;

        public CheckoutFormRepository(DataEF context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<CheckoutForm>> GetAllAsync() =>
            await _context.checkoutForms.Include(f => f.CardInfo).ToListAsync();

        public async Task<CheckoutForm?> GetByIdAsync(int id) =>
            await _context.checkoutForms.Include(f => f.CardInfo).FirstOrDefaultAsync(f => f.Id == id);

        public async Task<CheckoutForm> AddAsync(CheckoutForm form)
        {
            _context.checkoutForms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<CheckoutForm?> UpdateAsync(int id, CheckoutForm updatedForm)
        {
            var existing = await _context.checkoutForms.Include(f => f.CardInfo).FirstOrDefaultAsync(f => f.Id == id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(updatedForm);
            _context.Entry(existing.CardInfo).CurrentValues.SetValues(updatedForm.CardInfo);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var form = await _context.checkoutForms.FindAsync(id);
            if (form == null) return false;

            _context.checkoutForms.Remove(form);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ViewOrder>> ManageOrderAsync()
        {
            List<ViewOrder> viewOrders = new List<ViewOrder>();
            List<CheckoutForm> checkoutForms = (await GetAllAsync()).ToList();

            foreach (var checkoutForm in checkoutForms)
            {
                Order order = await _orderRepository.GetOrderByIdAsync(checkoutForm.OrderId);

                if (order != null)
                {
                    var orderDto = new OrderDto
                    {
                        Id = order.Id,
                        OrderDate = order.OrderDate,
                        CustomerId = order.CustomerId,
                        OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto
                        {
                            ProductId = od.ProductId,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice
                        }).ToList()
                    };

                    decimal total = orderDto.OrderDetails?.Sum(d => d.Quantity * d.UnitPrice) ?? 0;

                    viewOrders.Add(new ViewOrder
                    {
                        CheckoutFormId = checkoutForm.Id,
                        OrderId = order.Id,
                        OrderDto = orderDto,
                        CustomerName = checkoutForm.FullName,
                        CustomerEmail = checkoutForm.Email,
                        CustomerPhone = checkoutForm.PhoneNumber,
                        CustomerLocation = $"{checkoutForm.Country}, {checkoutForm.State}, {checkoutForm.City}, {checkoutForm.Address1}",
                        PaymentMethod = checkoutForm.PaymentMethod,
                        Price = (int)total
                    });
                }
            }

            return viewOrders;
        }
    }

}

