using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route ("api/[controller]")]
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

            var paymentMethodes = await _context.PaymentMethods.ToListAsync();

            if (paymentMethodes == null || !paymentMethodes.Any())
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(paymentMethodes);
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
    }
}
