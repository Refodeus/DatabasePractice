using DatabasePractice.Interfaces;
using DatabasePractice.Models;
using DatabasePractice.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabasePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Order>), 200)]
        [ProducesResponseType(400)]
        [ResponseCache(Duration = 60)]
        public IActionResult GetOrders([FromQuery] string filter = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Номер страницы и размер должны быть положительными.");
            var orders = _ordersRepository.GetOrdersFiltered(filter);
            orders = orders.OrderBy(p => p.Id).ToList();
            var totalCount = orders.Count();
            var pagginatedOrders = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            Response.Headers.Append("Total-Count", totalCount.ToString());
            return Ok(pagginatedOrders);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto.Id != 0)
                return BadRequest("The order ID should not be specified during creation (auto-increment is used)");
            if (orderDto == null)
                return BadRequest("Order is undefined.");
            if (!(orderDto.Статус is "Выполнен" or "В обработке" or "Отменен"))
                return BadRequest("The status should be \"Выполнен\" or \"В обработке\" or \"Отменен\"");
            try
            {
                var order = new Order
                {
                    Id = orderDto.Id,
                    Сумма = orderDto.Сумма,
                    ДатаИВремя = orderDto.ДатаИВремя,
                    Статус = orderDto.Статус,
                    Клиент = orderDto.Клиент,
                    КлиентNavigation = null
                };
                if (!_ordersRepository.CreateOrder(order))
                    return BadRequest("Failed to create order.");
                return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Ошибка при сохранении в базу данных: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                    return BadRequest("Order is undefined.");
                if (!(orderDto.Статус is "Выполнен" or "В обработке" or "Отменен"))
                    return BadRequest("The status should be \"Выполнен\" or \"В обработке\" or \"Отменен\"");
                var order = new Order
                {
                    Id = orderDto.Id,
                    Сумма = orderDto.Сумма,
                    ДатаИВремя = orderDto.ДатаИВремя,
                    Статус = orderDto.Статус,
                    Клиент = orderDto.Клиент,
                    КлиентNavigation = null
                };
                if (!_ordersRepository.isExist(order.Id))
                    return NotFound();
                if (!_ordersRepository.UpdateOrder(order))
                    return BadRequest("Failed to update order.");
                return Ok($"Id {order.Id} is Successfully updated.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Ошибка при сохранении в базу данных: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder([FromRoute] int id)
        {
            if (!_ordersRepository.isExist(id))
                return NotFound();
            if (!_ordersRepository.DeleteOrder(id))
                return BadRequest(ModelState);
            return Ok($"Id {id} is Successfully deleted.");
        }
    }
}
