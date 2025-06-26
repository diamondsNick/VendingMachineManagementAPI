using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs;
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

        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<PagedSimCards>> GetPagedSims(int amount, int page, [FromQuery] long CompanyId, [FromQuery] bool linked)
        {
            var query = _context.SimCards.AsQueryable();

            if (CompanyId != 0)
            {
                query = query.Where(c => c.CompanyID == CompanyId);
            }
            if (linked)
            {
                query = query.Where(sim => !_context.Modems.Any(m => m.SimCardID == sim.ID));
            }
                var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.ID)
                .Skip((page - 1) * amount)
                .Take(amount)
            .ToListAsync();

            if (items == null || items.Count == 0)
            {
                return NotFound("Page does not exist!");
            }

            var result = new PagedSimCards
            {
                Sims = items,
                TotalCount = totalCount
            };

            return Ok(result);
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
