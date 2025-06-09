using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : Controller
    {
        private readonly ManagementDbContext _context;
        public MaintenanceController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaintenance()
        {

            var maintenances = await _context.Maintenances.ToListAsync();

            if (maintenances == null || !maintenances.Any())
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(maintenances);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachinePaymentMethod(long id)
        {
            var paymentMethodes = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethodes == null)
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(paymentMethodes);
        }
        [HttpPost]
        public async Task<IActionResult> PostMaintenance(Maintenance maintenance)
        {
            try
            {
                _context.Maintenances.Add(maintenance);
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (await MaintenaceExists(maintenance.ID)) return BadRequest("Data already exists!");
                else throw;
            }
            return CreatedAtAction(nameof(Maintenance), maintenance);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutMaintenance(long Id, Maintenance maintenance)
        {
            if (!await MaintenaceExists(Id)) return NotFound();
            if (maintenance.ID != Id) return BadRequest("Id does not match!");
            try
            {
                _context.Entry(maintenance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException) { throw; }
            return Ok(maintenance);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMaintenance(long Id)
        { 
            if(!await MaintenaceExists(Id)) return NotFound();
            try
            {
                var maintenance = await _context.Maintenances.FindAsync(Id);
                _context.Maintenances.Remove(maintenance);
            }
            catch (DBConcurrencyException) { throw; }
            return Ok();
        }
        private async Task<bool> MaintenaceExists(long id)
        {
            bool exists = await _context.Maintenances.AnyAsync(e => e.ID == id);
            return exists;
        }
    }
}
