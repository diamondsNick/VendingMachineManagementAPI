using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinePaymentMethodController : Controller
    {
        private readonly ManagementDbContext _context;
        public MachinePaymentMethodController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMachinePaymentMethod()
        {

            var machinePaymentMethods = await _context.MachinePaymentMethods.ToListAsync();

            if (machinePaymentMethods == null || !machinePaymentMethods.Any())
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(machinePaymentMethods);
        }

        [HttpGet("{VendingMachineID}")]
        public async Task<IActionResult> GetPaymentMethodFromMachine(long VendingMachineID)
        {
            var machinePaymentMethodes = await _context.MachinePaymentMethods.FindAsync(VendingMachineID);

            if (machinePaymentMethodes == null)
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(machinePaymentMethodes);
        }

        [HttpPost]
        public async Task<ActionResult<MachinePaymentMethod>> PostMachinePaymentMethod(MachinePaymentMethod method)
        {
            try
            {
                _context.MachinePaymentMethods.Add(method);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsMethodExists(method.VendingMachineID, method.PaymentMethodID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostMachinePaymentMethod), method);
        }

        [HttpPut("{VendingMachineID}/{PaymentMethodID}")]
        public async Task<ActionResult<MachinePaymentMethod>> PutMachinePaymentMethod(long VendingMachineID, long PaymentMethodID, MachinePaymentMethod method)
        {
            if (!await IsMethodExists(method.VendingMachineID, method.PaymentMethodID)) return NotFound();
            if (method.VendingMachineID != VendingMachineID && method.PaymentMethodID != PaymentMethodID) return BadRequest("Ids does not match!");
            _context.Entry(method).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error occured while saving changes!");
            }
            return Ok(method);
        }

        [HttpDelete("{VendingMachineID}/{PaymentMethodID}")]
        public async Task<ActionResult> DeleteMachinePaymentMethod(long VendingMachineID, long PaymentMethodID)
        {
            if(!await IsMethodExists(VendingMachineID, PaymentMethodID))
                return NotFound();
            var metod = await _context.MachinePaymentMethods.FirstOrDefaultAsync(e => e.VendingMachineID == VendingMachineID && e.PaymentMethodID == PaymentMethodID);
            try
            {
                _context.MachinePaymentMethods.Remove(metod);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsMethodExists(long VendingMachineID, long PaymentMethodID)
        {
            bool method = await _context.MachinePaymentMethods.AnyAsync(m => m.VendingMachineID == VendingMachineID && m.PaymentMethodID == PaymentMethodID);
            return method;
        }
    }
}
