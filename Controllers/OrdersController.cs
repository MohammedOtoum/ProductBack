using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductTask.Data;
using ProductTask.Dto;
using ProductTask.Model;
using ProductTask.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                CustomerId = o.CustomerId,
                OrderDetails = o.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            });

            return Ok(orderDtos);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var dto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDto dto)
        {
            var order = new Order
            {
                OrderDate = dto.OrderDate,
                CustomerId = dto.CustomerId,
                OrderDetails = dto.OrderDetails.Select(d => new OrderDetailes
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            await _orderRepository.AddOrderAsync(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new
            {
                message = "Order created successfully.",
                orderId = order.Id
            });
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var existing = await _orderRepository.GetOrderByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.OrderDate = dto.OrderDate;
            existing.CustomerId = dto.CustomerId;

            // Update details
            existing.OrderDetails = dto.OrderDetails.Select(d => new OrderDetailes
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            }).ToList();

            await _orderRepository.UpdateOrderAsync(existing);

            return NoContent();
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var existing = await _orderRepository.GetOrderByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _orderRepository.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
