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
    public class PaymentMethodController : Controller
    {
        private readonly ManagementDbContext _context;
        public PaymentMethodController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {

            var methods = await _context.PaymentMethods.ToListAsync();

            if (methods == null || !methods.Any())
            {
                return NotFound();
            }

            return Ok(methods);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPaymentMethod(long Id)
        {
            var methods = await _context.PaymentMethods.FindAsync(Id);

            if (methods == null)
            {
                return NotFound();
            }

            return Ok(methods);
        }

        [HttpPost]
        public async Task<IActionResult> PostPaymentMethod(PaymentMethod method)
        {
            try
            {
                _context.PaymentMethods.Add(method);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsPaymentMethodExists(method.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostPaymentMethod), method);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutPaymentMethodOperatingMode(long Id, PaymentMethod method)
        {
            if (Id != method.ID) return BadRequest("Ids does not match!");
            if (!await IsPaymentMethodExists(Id)) return NotFound();
            try
            {
                _context.Entry(method).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(method);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeletePaymentMethod(long Id)
        {
            if (!await IsPaymentMethodExists(Id))
                return NotFound();
            try
            {
                var method = await _context.PaymentMethods.FindAsync(Id);
                _context.PaymentMethods.Remove(method);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsPaymentMethodExists(long Id)
        {
            bool exists = await _context.PaymentMethods.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
