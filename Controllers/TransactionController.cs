using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCriptomonedas.DTOs;
using WebCriptomonedas.Models;

namespace WebCriptomonedas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("client/{clientId}")] 
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactionsByClientId(int clientId)
        {
            var transactions = await _context.Transactions
                                             .Where(t => t.ClientId == clientId) 
                                             .OrderByDescending(t => t.Datetime)
                                             .ToListAsync();

            if (!transactions.Any())
             {
                return Ok(new List<TransactionDTO>()); 
             }

            var transactionDtos = transactions.Select(t => new TransactionDTO
            {
                Id = t.Id,
                CryptoCode = t.CryptoCode,
                Action = t.Action,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                Datetime = t.Datetime,
                ClientId = t.ClientId
                // Asegúrate que tu TransactionDTO tenga los mismos campos que necesitas mostrar
            }).ToList();

            return Ok(transactionDtos); 
        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> Get()
        {
            var transactions = await _context.Transactions.ToListAsync();

            var transactionDtos = transactions.Select(t => new TransactionDTO
            {
                Id = t.Id,
                CryptoCode = t.CryptoCode,
                Action = t.Action,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                Datetime = t.Datetime,
                ClientId = t.ClientId   
            }).ToList();

            return Ok(transactionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> Get(int id)
        {
            var transaction = await _context.Transactions.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (transaction == null)
            {
                return NotFound();
            }
            var transactionDtos = new TransactionDTO
            {
                Id = transaction.Id,
                CryptoCode = transaction.CryptoCode,
                Action = transaction.Action,
                CryptoAmount = transaction.CryptoAmount,
                Money = transaction.Money,
                Datetime = transaction.Datetime,
                ClientId = transaction.ClientId




            };

            return Ok(transactionDtos);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Post(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }
            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
