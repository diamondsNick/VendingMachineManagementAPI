using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimCardController : Controller
    {
        private readonly ManagementDbContext _context;
        public SimCardController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSimCards()
        {
            var simCards = await _context.SimCards.ToListAsync();

            if (simCards == null || !simCards.Any())
            {
                return NotFound();
            }

            return Ok(simCards);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSimCard(long Id)
        {
            var simCard = await _context.SimCards.FindAsync(Id);

            if (simCard == null)
            {
                return NotFound();
            }

            return Ok(simCard);
        }

        [HttpPost]
        public async Task<IActionResult> PostSimCard(SimCard simCard)
        {
            try
            {
                _context.SimCards.Add(simCard);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsSimCardExists(simCard.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostSimCard), simCard);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutSimCard(long Id, SimCard simCard)
        {
            if (Id != simCard.ID) return BadRequest("Ids does not match!");
            if (!await IsSimCardExists(Id)) return NotFound();
            try
            {
                _context.Entry(simCard).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(simCard);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteSimCard(long Id)
        {
            if (!await IsSimCardExists(Id))
                return NotFound();
            try
            {
                var simCard = await _context.SimCards.FindAsync(Id);
                _context.SimCards.Remove(simCard);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsSimCardExists(long Id)
        {
            bool exists = await _context.SimCards.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
