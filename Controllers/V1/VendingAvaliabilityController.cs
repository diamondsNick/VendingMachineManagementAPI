using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VendingAvaliabilityController : Controller
    {
        private readonly ManagementDbContext _context;
        public VendingAvaliabilityController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVendingAvaliabilities([FromQuery] long VendingMachineId = 0)
        {
            var availabilities = _context.VendingAvaliabilities.AsQueryable();
            availabilities = availabilities
                .Include(v => v.VendingMachine)
                .Include(v => v.Product);

            if (VendingMachineId != 0)
            {
                availabilities = availabilities.Where(v => v.VendingMachineID == VendingMachineId);
            }

            if (availabilities == null || !availabilities.Any())
            {
                return NotFound();
            }

            var res = await availabilities.ToListAsync();

            return Ok(res);
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
    }
}
