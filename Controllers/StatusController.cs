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
    public class StatusController : Controller
    {
        private readonly ManagementDbContext _context;
        public StatusController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _context.Statuses.ToListAsync();

            if (statuses == null || !statuses.Any())
            {
                return NotFound();
            }

            return Ok(statuses);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStatus(long Id)
        {
            var status = await _context.Statuses.FindAsync(Id);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        [HttpPost]
        public async Task<IActionResult> PostStatus(Status status)
        {
            try
            {
                _context.Statuses.Add(status);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsStatusExists(status.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostStatus), status);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutStatus(long Id, Status status)
        {
            if (Id != status.ID) return BadRequest("Ids does not match!");
            if (!await IsStatusExists(Id)) return NotFound();
            try
            {
                _context.Entry(status).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(status);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteStatus(long Id)
        {
            if (!await IsStatusExists(Id))
                return NotFound();
            try
            {
                var status = await _context.Statuses.FindAsync(Id);
                _context.Statuses.Remove(status);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsStatusExists(long Id)
        {
            bool exists = await _context.Statuses.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
