using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatingModeController : Controller
    {
        private readonly ManagementDbContext _context;
        public OperatingModeController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOperatingModes()
        {

            var modes = await _context.OperatingModes.ToListAsync();

            if (modes == null || !modes.Any())
            {
                return NotFound();
            }

            return Ok(modes);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOperatingMode(long Id)
        {
            var modes = await _context.OperatingModes.FindAsync(Id);

            if (modes == null)
            {
                return NotFound();
            }

            return Ok(modes);
        }

        [HttpPost]
        public async Task<IActionResult> PostMode(OperatingMode mode)
        {
            try
            {
                _context.OperatingModes.Add(mode);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsModeExists(mode.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostMode), mode);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutOperatingMode(long Id, OperatingMode mode)
        {
            if (Id != mode.ID) return BadRequest("Ids does not match!");
            if (!await IsModeExists(Id)) return NotFound();
            try
            {
                _context.Entry(mode).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(mode);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteOperatingMode(long Id)
        {
            if (!await IsModeExists(Id))
                return NotFound();
            try
            {
                var mode = await _context.OperatingModes.FindAsync(Id);
                _context.OperatingModes.Remove(mode);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsModeExists(long Id)
        {
            bool exists = await _context.OperatingModes.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
