using Microsoft.AspNetCore.Mvc;
using order_service.Models;
using order_service.Repositories;

namespace order_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _repository.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var created = await _repository.CreateAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = created.OrderId }, created);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, Order order)
        {
            if (id != order.OrderId)
                return BadRequest();
            var updated = await _repository.UpdateAsync(order);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
