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
    public class VendingAvaliabilityController : Controller
    {
    private readonly ManagementDbContext _context;
        public VendingAvaliabilityController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVendingAvaliabilities()
        {
            var availabilities = await _context.VendingAvaliabilities
                .Include(v => v.VendingMachine)
                .Include(v => v.Product)
                .ToListAsync();

            if (availabilities == null || !availabilities.Any())
            {
                return NotFound();
            }

            return Ok(availabilities);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVendingAvaliability(long Id)
        {
            var availability = await _context.VendingAvaliabilities
                .Include(v => v.VendingMachine)
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.ID == Id);

            if (availability == null)
            {
                return NotFound();
            }

            return Ok(availability);
        }

        [HttpPost]
        public async Task<IActionResult> PostVendingAvaliability(VendingAvaliability availability)
        {
            try
            {
                _context.VendingAvaliabilities.Add(availability);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsVendingAvaliabilityExists(availability.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostVendingAvaliability), availability);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutVendingAvaliability(long Id, VendingAvaliability availability)
        {
            if (Id != availability.ID) return BadRequest("Ids does not match!");
            if (!await IsVendingAvaliabilityExists(Id)) return NotFound();
            try
            {
                _context.Entry(availability).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(availability);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteVendingAvaliability(long Id)
        {
            if (!await IsVendingAvaliabilityExists(Id))
                return NotFound();
            try
            {
                var availability = await _context.VendingAvaliabilities.FindAsync(Id);
                _context.VendingAvaliabilities.Remove(availability);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsVendingAvaliabilityExists(long Id)
        {
            bool exists = await _context.VendingAvaliabilities.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
