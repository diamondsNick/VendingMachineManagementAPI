using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : Controller
    {
        private readonly ManagementDbContext _context;
        public ManufacturerController(ManagementDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<Manufacturer>> GetManufacturers()
        {
            if (!await _context.Manufacturers.AnyAsync()) return NotFound();
            try
            {
                var manufacturers = await _context.Manufacturers
                    .Include(e => e.VendingMachineMatrices)
                    .ToArrayAsync();
                return Ok(manufacturers);
            }
            catch (DBConcurrencyException) { throw; }
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Manufacturer>> GetManufacturer(long Id)
        {
            if (!await IsManufacturerExists(Id)) return NotFound();
            try
            {
                var manufacturers = await _context.Manufacturers
                    .Include(e => e.VendingMachineMatrices)
                    .FirstAsync(e => e.ID == Id);
                return Ok(manufacturers);
            }
            catch (DBConcurrencyException) { throw; }
        }
        [HttpPost]
        public async Task<IActionResult> PostManufacturer(Manufacturer manufacturer)
        {
            bool check = await _context.Manufacturers.AnyAsync(e => e.Name == manufacturer.Name);
            if (check) return BadRequest("Object already exists!");
            try
            {
                _context.Manufacturers.Add(manufacturer);
                _context.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
            return CreatedAtAction(nameof(GetManufacturer), new { id = manufacturer.ID }, manufacturer);

        }

        [HttpPut]
        public async Task<IActionResult> PutManufacturer(Manufacturer manufacturer)
        {
            if (!await IsManufacturerExists(manufacturer.ID)) return NotFound();
            if (await _context.Manufacturers.AnyAsync(e => e.Name == manufacturer.Name)) return BadRequest("Object with this name already exists!");
            try
            {
                _context.Entry(manufacturer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException) { throw; }
            return CreatedAtAction(nameof(PutManufacturer), new { id = manufacturer.ID }, manufacturer);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteManufacturer(long Id)
        {
            if(!await IsManufacturerExists(Id)) return NotFound();
            try
            {
                var manufacturer = await _context.Manufacturers.FindAsync(Id);
                _context.Manufacturers.Remove(manufacturer);
                await _context.SaveChangesAsync();
            }
            catch(DBConcurrencyException) { throw; }
            return Ok();
        }
        private async Task<bool> IsManufacturerExists(long Id)
        {
            var exists = await _context.Manufacturers.AnyAsync(e => e.ID == Id);
            return exists;
        }
    }
}
