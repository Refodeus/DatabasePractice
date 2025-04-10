using DatabasePractice.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabasePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionsController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        public FunctionsController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
        [HttpGet("get_avg_check")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ResponseCache(Duration = 60)]
        public IActionResult GetAvgCheck()
        {
            var result = _ordersRepository.GetAvgChecks();
            if (result == null)
                return BadRequest(ModelState);
            return Ok(result);
        }
        [HttpGet("get_purchases_of_birthday")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ResponseCache(Duration = 60)]
        public IActionResult GetPurchasesOfBirthday()
        {
            var result = _ordersRepository.GetPurchasesOfBirthday();
            if (result == null)
                return BadRequest(ModelState);
            return Ok(result);
        }
    }
}
