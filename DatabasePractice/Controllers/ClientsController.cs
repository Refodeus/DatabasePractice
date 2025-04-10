using DatabasePractice.Interfaces;
using DatabasePractice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DatabasePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IClientsRepository _clientsRepository;
        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Client>), 200)]
        [ProducesResponseType(400)]
        [ResponseCache(Duration = 60)]
        public IActionResult GetClients([FromQuery] string filter = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Номер страницы и размер должны быть положительными.");
            var clients = _clientsRepository.GetClientsFiltered(filter);
            clients = clients.OrderBy(p => p.Id).ToList();
            var totalCount = clients.Count;
            var pagginatedClients = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            Response.Headers.Append("Total-Count", totalCount.ToString());
            return Ok(clients);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(400)]
        public IActionResult CreateClient([FromBody] Client client)
        {
            if (client == null)
                return BadRequest("Client is undefined.");
            if (!_clientsRepository.CreateClient(client))
                return BadRequest("Failed to create client.");
            return CreatedAtAction(nameof(GetClients), new { id = client.Id }, client);
        }
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClient([FromBody] Client client)
        {
            if (client == null)
                return BadRequest("Client is undefined.");
            if (!_clientsRepository.isExist(client.Id))
                return NotFound();
            if (!_clientsRepository.UpdateClient(client))
                return BadRequest("Failed to update client.");
            return Ok($"Id {client.Id} is Successfully updated.");
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteClient([FromRoute] int id)
        {
            if (!_clientsRepository.isExist(id))
                return NotFound();
            if (!_clientsRepository.DeleteClient(id))
                return BadRequest(ModelState);
            return Ok($"Id {id} is Successfully deleted.");
        }
    }
}
