using BankAPI.BankModels;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _service;

        public ClientController(ClientService client)
        {
            _service = client;
        }

        [HttpGet("getall")]
        public async Task<IEnumerable<Client>> GetClients() => await _service.GetClients();

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _service.GetClientById(id);

            if (client is null)
            {
                return ClientNotFound(id);
            }

            return client;
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateClient(Client client) {
           
            var newClient = await _service.CreateClient(client);

            return CreatedAtAction(nameof(GetClientById), new {id = client.Id}, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Client client, int id) {

            if(id != client.Id)
            {
                return BadRequest(
                    new {message = $"El ID({id}) de la URL no coincide con el ID({client.Id}) del cuerpo de la solicitud"}     
               );
            }

            var clientToUpdate = await _service.GetClientById(id);

            if(clientToUpdate is not null) {
                await _service.UpdateClient(clientToUpdate);
                return NoContent();
            } else
            {
                return ClientNotFound(id);
            }
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var clientToDelete = await _service.GetClientById(id);

            if (clientToDelete is not null)
            {
                await _service.DeleteClient(id);
                return Ok();
            }
            else
            {
                return ClientNotFound(id);
            }
        }

        private NotFoundObjectResult ClientNotFound(int id)
        {
            return NotFound(new { message = $"El cliente con ID = {id} no existe." });
        }
    }
}
