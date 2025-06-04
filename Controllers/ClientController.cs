using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCriptomonedas.DTOs;
using WebCriptomonedas.Models;

namespace WebCriptomonedas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> Get()
        {
            var clients = await _context.Clients.ToListAsync();

            var clientDtos = clients.Select(c => new ClientDTO
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email
            }).ToList();

            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> Get(int id)
        {
            var client = await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();

            var clientDtos = new ClientDTO
            { 
                Id = client.Id,
                Name = client.Name,
                Email = client.Email
            };

            return Ok(clientDtos);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Post(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
        }

        // PUT: api/notebooks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/notebooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotebook(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
